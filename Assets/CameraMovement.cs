using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [Min(0.0f)]
    public float focusRadius = 1.0f;
    public float distance = 5.0f;

    [Range(0.0f, 1f)]
    public float rateOfRecenter = 0.5f;

    [Min(0.0f)]
    public float alignDelay = 5.0f;

    [Range(1.0f, 360.0f)]
    public float rotationSpeed = 90.0f;

    [Min(1.0f)]
    public float horizontalSensitivity = 1.0f;

    [Min(1.0f)]
    public float verticalSensitivity = 1.0f;

    private Transform focus;
    private Vector3 focusPoint;
    private Vector2 viewAngles = new Vector2(45.0f, 0.0f);

    void Awake() {
        focus = GameObject.FindWithTag("Player").transform;
        focusPoint = focus.position;
        focusPoint.y += 1;

        transform.localRotation = Quaternion.Euler(viewAngles);
    }

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Z)) Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.X)) Cursor.lockState = CursorLockMode.None;
        
        Quaternion lookRotation;

        UpdateFocusPoint();
        if (HandleInput()) {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(viewAngles);
        } else lookRotation = transform.localRotation;

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint() {
        Vector3 targetPoint = focus.position;
        targetPoint.y += 1;
        if (focusRadius > 0.0f) {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1.0f;
            if (distance > 0.01f && rateOfRecenter > 0.0f) {
                t = Mathf.Pow(1f - rateOfRecenter, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius) {
                t = Mathf.Min(t, focusRadius / distance);
            }

            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        } else
            focusPoint = targetPoint;
    }

    bool HandleInput() {
        const float e = 0.0001f;
        Vector2 input = new Vector2(Input.GetAxis("Mouse Y") * verticalSensitivity, Input.GetAxis("Mouse X") * horizontalSensitivity);
        
        if (input.x < -e || e < input.x || input.y < -e || e < input.y) {
            viewAngles += input * rotationSpeed * Time.unscaledDeltaTime;
            return true;
        }

        return false;
    }


    void ConstrainAngles() {
        viewAngles.x = Mathf.Clamp(viewAngles.x, -90, 90);
        if (viewAngles.y < 0.0f) viewAngles.y += 360.0f;
        else if (viewAngles.y >= 360.0f) viewAngles.y -= 360.0f;
    }
}
