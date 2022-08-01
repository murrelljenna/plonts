using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 1)]
public class LightingPresets : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient SunColor;
    public Gradient MoonColor;
    public Gradient FogColor;
    public AnimationCurve sunIntensityCurve;
    public AnimationCurve moonIntensityCurve;
}