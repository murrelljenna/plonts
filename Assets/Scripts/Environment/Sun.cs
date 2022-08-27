using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField, Header("Sun")]
    public float rotationSpeed;

    [SerializeField]
    public float time;

    [SerializeField]
    public AnimationCurve lightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(rotationSpeed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Light sunLight = GetComponent<Light>();
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = lightIntensity.Evaluate(dotProduct);
    }
}
