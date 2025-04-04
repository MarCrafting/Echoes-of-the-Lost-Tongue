using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6; // char movement speed

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  // left, right (a, d)
        float vertical = Input.GetAxisRaw("Vertical");  // up, down (w, s)
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;    // direction to move

        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed *= 1.5f;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = 6;
    }
}
