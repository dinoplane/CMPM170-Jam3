using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 CameraPosition;
    [Header("Camera Settings")]
    public float CameraSpeed;
    float speed = 60.0f;

    void Start()
    {
        CameraPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            CameraPosition.y += CameraSpeed / speed;
        }
        if(Input.GetKey(KeyCode.S)){
            CameraPosition.y -= CameraSpeed / speed;
        }
        if(Input.GetKey(KeyCode.D)){
            CameraPosition.x += CameraSpeed / speed;
        }
        if(Input.GetKey(KeyCode.A)){
            CameraPosition.x -= CameraSpeed / speed;
        }

        this.transform.position = CameraPosition;
    }
}
