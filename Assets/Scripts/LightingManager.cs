using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    // References
    [SerializeField] public Light Sun;
    [SerializeField] public Light Moon;
    [SerializeField] private LightingPresets Preset;

    // Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    const short morningTime = 6;
    const short nightTime = 18;

    public UnityEvent sunUp;

    private bool rotationComplete = true;

    private void Update()
    {
        if (Preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; // clamp
            UpdateLighting(TimeOfDay / 24f);


            if (rotationComplete && (int)TimeOfDay == morningTime)
            {
                sunUp.Invoke();
                rotationComplete = false; // Lets not trigger this again till the day is over
            }
            else if (!rotationComplete && (int)TimeOfDay == nightTime)
                rotationComplete = true;
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (Sun != null)
        {
            Sun.color = Preset.SunColor.Evaluate(timePercent);
            Sun.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            Sun.intensity = Preset.sunIntensityCurve.Evaluate(timePercent);
        }

        if (Moon != null)
        {
            Moon.color = Preset.MoonColor.Evaluate(timePercent);
            Moon.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            Moon.intensity = Preset.moonIntensityCurve.Evaluate(timePercent);
        }
    }

    private void OnValidate()
    {
        if (Sun != null && Moon != null)
        {
            return;
        }

        Light[] lights = GameObject.FindObjectsOfType<Light>();
        ArrayList directionalLights = new ArrayList();
        foreach(Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                directionalLights.Add(light);
            }
        }

        if (directionalLights.Count >= 1)
        {
            Sun = (Light)directionalLights[0];
        }
        else if (directionalLights.Count >= 2)
        {
            Moon = (Light)directionalLights[1];
        }

        if (RenderSettings.sun != null)
        {
            Sun = RenderSettings.sun;
        }
    }

    public static LightingManager Get()
    {
        return GameObject.Find("Post-Process Volume")?.GetComponent<LightingManager>();
    }
}
