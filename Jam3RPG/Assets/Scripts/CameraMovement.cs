using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 CameraPosition;
    [Header("Camera Settings")]
    public float CameraSpeed;

    void Start()
    {
        CameraPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            CameraPosition.y += CameraSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S)){
            CameraPosition.y -= CameraSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D)){
            CameraPosition.x += CameraSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A)){
            CameraPosition.x -= CameraSpeed * Time.deltaTime;
        }

        this.transform.position = CameraPosition;
    }
}
