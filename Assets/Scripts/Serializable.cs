using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializable : MonoBehaviour
{
    public string description;
    // Start is called before the first frame update
    void Start()
    {
        description = gameObject.name;
    }
}
