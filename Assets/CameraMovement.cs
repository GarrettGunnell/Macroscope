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
    float alignDelay = 5.0f;

    [Range(1.0f, 360.0f)]
    public float rotationSpeed = 90.0f;

    [Min(1.0f)]
    public float horizontalSensitivity = 1.0f;

    [Min(1.0f)]
    public float verticalSensitivity = 1.0f;

    private Transform focus;
    private Vector3 focusPoint, previousFocusPoint;
    private Vector2 viewAngles = new Vector2(45.0f, 0.0f);
    private float lastManualRotationTime;

    void Awake() {
        focus = GameObject.FindWithTag("Player").transform;
        focusPoint = focus.position;
        focusPoint.y += 1;

        transform.localRotation = Quaternion.Euler(viewAngles);
    }

    void LateUpdate() {
        Quaternion lookRotation;

        UpdateFocusPoint();
        if (HandleInput() || AutomaticRotation()) {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(viewAngles);
            lastManualRotationTime = Time.unscaledTime;
        } else lookRotation = transform.localRotation;

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint() {
        previousFocusPoint = focusPoint;
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

    bool AutomaticRotation() {
        if (Time.unscaledTime - lastManualRotationTime < alignDelay) {
            return false;
        }

        Vector2 movement = new Vector2(focusPoint.x - previousFocusPoint.x, focusPoint.z - previousFocusPoint.z);
        float movementDeltaSqr = movement.sqrMagnitude;
        if (movementDeltaSqr < 0.000001f)
            return false;

        float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(viewAngles.y, headingAngle));
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        if (deltaAbs < 45.0f)
            rotationChange *= deltaAbs / 45.0f;
        else if (180.0f - deltaAbs < 45.0f)
            rotationChange *= (180.0f - deltaAbs) / 45.0f;
        viewAngles.y = Mathf.MoveTowardsAngle(viewAngles.y, headingAngle, rotationChange);

        return true;
    }

    void ConstrainAngles() {
        viewAngles.x = Mathf.Clamp(viewAngles.x, -90, 90);
        if (viewAngles.y < 0.0f) viewAngles.y += 360.0f;
        else if (viewAngles.y >= 360.0f) viewAngles.y -= 360.0f;
    }
}
