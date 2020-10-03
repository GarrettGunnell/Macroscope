using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float velocity = 1.0f;
    private Vector2 direction;

    void Update() {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        transform.localPosition = new Vector3(direction.x, transform.localPosition.y, direction.y);
    }
}
