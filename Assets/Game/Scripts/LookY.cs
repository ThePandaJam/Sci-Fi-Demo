using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1f;
    
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x += mouseY * _sensitivity;
        transform.localEulerAngles = newRotation;
    }
}
