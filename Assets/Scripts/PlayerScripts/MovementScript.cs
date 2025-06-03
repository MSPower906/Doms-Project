using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=b1uoLBp2I1w
public class MovementScript : MonoBehaviour
{
    private Vector3 MovementInput;
    private Vector2 MouseInput;

    public Rigidbody Rb;
    public Transform Camera;
    public float Speed;
    public float Sensitivity;
    private float Vertical;
    private float Horizontal;

    // Start is called before the first frame update
    //void Start()
    //{
    //    Rb = GetComponent<Rigidbody>();
    //}

    // Update is called once per frame
    void Update()
    {
        //Vertical = Input.GetAxis("Vertical");
        //Horizontal = Input.GetAxis("Horizontal");
        //Rb.velocity = (transform.forward * Vertical) * Speed;
        //Rb.velocity = (transform.right * Horizontal) * Speed;
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(MovementInput) * Speed;
        Rb.velocity = new Vector3(MoveVector.x, Rb.velocity.y, MoveVector.z);
    }

    void MoveCamera()
    {

    }
}
