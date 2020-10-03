﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float velocity = 1.0f;
    private Vector3 direction;

    void Update() {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = transform.localPosition.y;
        direction.z = Input.GetAxis("Vertical");

        transform.localPosition = direction;
    }
}
