using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour
{
    public float value;

    private Renderer sphereRenderer;

    public void SetValue(float newValue)
    {
        value = newValue;

        sphereRenderer = GetComponent<Renderer>();

        if (value < 0)
        {
            // Set color of sphere to black
            sphereRenderer.material.color = Color.black;
        }
        else
        {
            sphereRenderer.material.color = Color.white;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       
    }
}
