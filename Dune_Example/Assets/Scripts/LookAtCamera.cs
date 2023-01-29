using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private bool invert;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 directionCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position +(directionCamera * -1));
        }
        else
            transform.LookAt(cameraTransform);
    }
}
