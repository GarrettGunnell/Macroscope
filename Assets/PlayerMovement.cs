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
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction;
        if (playerInputSpace) {
            Vector3 forward = playerInputSpace.forward;
            forward.y = 0.0f;
            forward.Normalize();
            Vector3 right = playerInputSpace.right;
            right.y = 0.0f;
            right.Normalize();
            direction = forward * input.y + right * input.x;
        } else
            direction = new Vector3(input.x, 0.0f, input.y);
        
        InterpolateRotationToCamera();
        transform.localPosition += direction * velocity * Time.deltaTime;
    }

    void InterpolateRotationToCamera() {
        Vector3 targetRotation = playerInputSpace.rotation.eulerAngles;
        targetRotation.x = 0.0f;
        transform.rotation = Quaternion.Slerp(inertialFrameRotation, Quaternion.Euler(targetRotation), time);
        time += Time.deltaTime;
        if (transform.rotation.eulerAngles == targetRotation) {
            inertialFrameRotation = Quaternion.Euler(targetRotation);
            time = 0;
        }
    }
}
