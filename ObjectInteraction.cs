using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public float grabDistance = 5.0f;
    public LayerMask grabbableLayer;

    private GameObject grabbedObject;
    private bool isGrabbing = false;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetMouseButtonDown(0))
        {
            if (!isGrabbing)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit, grabDistance, grabbableLayer))
                {
                    grabbedObject = hit.collider.gameObject;
                    isGrabbing = true;
                }
            }
        }  
     if (Input.GetMouseButtonUp(0) && isGrabbing)
        {
            ReleaseObject();
        }

        if (isGrabbing)
        {
            MoveObject();
        }
    }
    void MoveObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, grabDistance))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -2, 2);
            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, targetPosition, Time.deltaTime * 10.0f);
        }
    }

    void ReleaseObject()
    {
        grabbedObject = null;
        isGrabbing = false;
    }
}
