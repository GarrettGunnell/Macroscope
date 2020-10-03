using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    Transform playerInputSpace = default;
    
    public float velocity = 3.0f;
    private float time = 0.0f;
    private Quaternion inertialFrameRotation;

    void Awake() {
        inertialFrameRotation = transform.rotation;
    }

    void Update() {
        Vector3 direction = DetermineDirection();
        InterpolateRotationToCamera();
        transform.localPosition += direction * velocity * Time.deltaTime;
    }

    Vector3 DetermineDirection() {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 forward = playerInputSpace.forward;
        forward.y = 0.0f;
        forward.Normalize();
        
        Vector3 right = playerInputSpace.right;
        right.y = 0.0f;
        right.Normalize();

        return forward * input.y + right * input.x;
    }

    void InterpolateRotationToCamera() {
        Vector3 targetRotationEulers = playerInputSpace.rotation.eulerAngles;
        targetRotationEulers.x = 0.0f;

        Quaternion targetRotation = Quaternion.Euler(targetRotationEulers);
        transform.rotation = Quaternion.Slerp(inertialFrameRotation, targetRotation, time);
        time += Time.deltaTime;
        if (transform.rotation == targetRotation) {
            inertialFrameRotation = targetRotation;
            time = 0;
        }
    }
}
