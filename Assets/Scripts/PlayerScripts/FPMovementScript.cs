using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///https://www.youtube.com/watch?v=b1uoLBp2I1w

public class FPMovementScript : MonoBehaviour
{
    private Vector3 MovementInput;
    private Vector2 MouseInput;
    private float xRotation;

    public Rigidbody Rb;
    public Transform Camera;
    public float Speed;
    public float Sensitivity;
    private float Vertical;
    private float Horizontal;

 
    void Update()
    {
        
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MoveCamera();
    }

    void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(MovementInput) * Speed;
        Rb.velocity = new Vector3(MoveVector.x, Rb.velocity.y, MoveVector.z);
    }

    void MoveCamera()
    {
        xRotation -= MouseInput.y * Sensitivity;

        transform.Rotate(0f, MouseInput.x * Sensitivity, 0f);
        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
