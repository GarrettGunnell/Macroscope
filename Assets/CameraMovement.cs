using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    Transform focus = default;
    
    [SerializeField, Range(1.0f, 20.0f)]
    float distance = 5.0f;

    [SerializeField, Min(0f)]
    float focusRadius = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    Vector3 focusPoint;

    void Awake() {
        focusPoint = focus.position;
    }

    void LateUpdate() {
        CalculateFocusPoint();
        Vector3 lookDirection = transform.forward;
        transform.localPosition = focusPoint - lookDirection * distance;        
    }

    void CalculateFocusPoint() {
        float t = 1f;
        Vector3 targetPoint = focus.position;
        if (focusRadius > 0f) {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            if (distance > 0.01f && focusCentering > 0f) {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius) {
                t = Mathf.Min(t, focusRadius / distance);
            }
        }

        focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
    }
}
