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

    [SerializeField, Range(1f, 1000f)]
    float rotationSpeed = 90f;

    Vector3 focusPoint;
    Vector2 rotationAngles = new Vector2(10f, 0f);
    float maxVerticalAngle = 90.0f;
    float minVerticalAngle = -90.0f;

    void Awake() {
        focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(rotationAngles);
    }

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Z)) Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.X)) Cursor.lockState = CursorLockMode.None;

        CalculateFocusPoint();
        ManualRotation();
        ClampAngles();

        Vector3 altitudeAdjust = new Vector3(0.0f, 1.0f, 0.0f);
        Quaternion lookRotation = Quaternion.Euler(rotationAngles);
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = (focusPoint + altitudeAdjust) - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);        
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

    void ManualRotation() {
        Vector2 input = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        const float epsilon = 0.001f;
        if (input.x < -epsilon || epsilon < input.x || input.y < -epsilon || epsilon < input.y)
            rotationAngles += rotationSpeed * Time.unscaledDeltaTime * input;
    }

    void ClampAngles() {
        rotationAngles.x = Mathf.Clamp(rotationAngles.x, minVerticalAngle, maxVerticalAngle);
        
        if (rotationAngles.y < 0.0f) rotationAngles.y += 360.0f;
        if (rotationAngles.y >= 360.0f) rotationAngles.y -= 360.0f;
    }
}
