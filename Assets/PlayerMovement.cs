using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float velocity = 3.0f;
    private Vector3 direction;

    void Update() {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = 0.0f;
        direction.z = Input.GetAxis("Vertical");

        transform.localPosition += direction * velocity * Time.deltaTime;
    }
}
