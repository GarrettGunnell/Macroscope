using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    Transform focus = default;
    
    [SerializeField, Range(1.0f, 20.0f)]
    float distance = 5.0f;

    void Awake() {
        
    }

    void Update() {
        
    }
}
