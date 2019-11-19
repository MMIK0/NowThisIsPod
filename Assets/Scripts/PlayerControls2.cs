using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls2 : MonoBehaviour
{
    private GameManagerScript GMS;
    public float thrustSpeed;
    public float turnSpeed;
    public float hoverPower;
    public float hoverHeight;

    private float thrustInput;
    private float turnInput;
    private Rigidbody shipRigidBody;

    // Use this for initialization
    void Start()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript> ();
        shipRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
         if(gameObject.tag == "Player2")
        {
            thrustInput = Input.GetAxis("Vertical_2");
            turnInput = Input.GetAxis("Horizontal_2");
        }

    }

    void FixedUpdate()
    {
        if(GMS.countDownDone == true && gameObject.tag == "Player2") {
        // Turning the ship
        shipRigidBody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);

        // Moving the ship
        shipRigidBody.AddRelativeForce(0f, 0f, thrustInput * thrustSpeed);

        // Hovering
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverPower;
            shipRigidBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        }
    }
}
