using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    Transform playerInputSpace = default;
    
    public float velocity = 3.0f;

    void Update() {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        transform.localPosition += direction * velocity * Time.deltaTime;
    }
}
