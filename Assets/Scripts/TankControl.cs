using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    public enum MovementDirection {left, right, forward, back}
    public MovementDirection direction;
    public Rigidbody playerBody;

    [Range(0.0f, 1.0f)]
    public float gameSpeed = 1f;
    public float movementStep = 2f;
    public bool inCollision;

    public bool inMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) && !inMovement)
        {
            direction = Vector3.forward;
            inMovement = true;
            
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);                             
        }

        if (Input.GetKey(KeyCode.DownArrow) && !inMovement)
        {
            direction = Vector3.back;
            inMovement = true;

            transform.rotation = Quaternion.Euler(180f, 0f, 180f);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && !inMovement)
        {
            direction = Vector3.left;
            inMovement = true;

            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !inMovement)
        {
            direction = Vector3.right;
            inMovement = true;
            
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        playerBody.velocity = direction * movementStep * gameSpeed;
        inMovement = false; 
    }
}
