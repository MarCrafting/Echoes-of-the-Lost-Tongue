using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6;
    public float jumpHeight = 3;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    // Update is called once per frame
    void Update()
    {
        determineDirection();

        // If spacebar is pressed and the player is on the ground, jump
        if(Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            transform.position += new Vector3(0, jumpHeight, 0);
        }

        // If left shift is pressed, increase speed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 12;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 6;
        }
    }

    void determineDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
