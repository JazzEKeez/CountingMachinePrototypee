using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public Text CounterText;

    public float count = 0;

    private void Start()
    {
        count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        SphereScript sphere = other.GetComponent<SphereScript>();
        if(sphere != null)
        {
            sphere.transform.SetParent(transform);
            count += sphere.value;
            CounterText.text = "" + count;
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        SphereScript sphere = other.GetComponent<SphereScript>();
        if(sphere != null)
        {
            sphere.transform.SetParent(null);
            count -= sphere.value;
            CounterText.text = "" + count;
        }
        
    }
}
