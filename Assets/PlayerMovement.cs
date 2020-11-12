using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Min(1.0f)]
    public float velocity = 1.0f;

    private Transform inputSpace;

    private void Awake() {
        inputSpace = GameObject.FindWithTag("MainCamera").transform;
    }
    
    private void Update() {
        Vector3 currentPos = transform.position;
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 forward = inputSpace.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Vector3 right = inputSpace.right;
        right.y = 0.0f;
        right.Normalize();
        Vector3 inputDirection = (forward * playerInput.y + right * playerInput.x) * velocity * Time.unscaledDeltaTime;

        transform.position = currentPos + inputDirection;
    }
}
