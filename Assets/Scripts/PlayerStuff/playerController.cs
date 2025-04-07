using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 5f;
   [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnSpeed = 360;


    
    private Vector3 _input;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        GatherInput();
        Look();

    }
    //fixed update for 
    private void FixedUpdate()
    {
        Move();
    }

    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Look()
    {

        //rotate 45 to account for camera rotation
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        var skewedInput = matrix.MultiplyPoint3x4(_input);

        //this prevents snapping to the default angle
        if (_input == Vector3.zero) return;


        //basically rotating the player model based on input so they can travel in that direction
        var relative = (transform.position + skewedInput) - transform.position;
        var rot = Quaternion.LookRotation(relative, Vector3.up);


        //lerp for smooth rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation,rot, turnSpeed * Time.deltaTime);
    }

    void Move()
    {

        


        Debug.Log( "current input: " + _input.normalized.magnitude);

        //move character in forward direction based on rotation and if input is present
        rb.MovePosition(transform.position + (transform.forward * _input.normalized.magnitude) * moveSpeed * Time.deltaTime);


        if (_input.magnitude > 0)
        {
            //player is moving


        }else
        {
            //player is not moving


        }

    }

    public bool IsMoving()
    {
        if (_input == Vector3.zero) return false; else return true;
    }

}
