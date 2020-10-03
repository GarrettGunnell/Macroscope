using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    Transform playerInputSpace = default;
    
    public float velocity = 3.0f;

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
        
        direction.y = 0.0f;
        transform.localPosition += direction * velocity * Time.deltaTime;
    }
}
