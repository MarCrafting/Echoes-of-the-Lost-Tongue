using UnityEngine;

public class Movement : MonoBehaviour
{

    public float walkSpeed = 2;
    public float jumpHeight = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move forward
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            transform.position += new Vector3(0, 0, walkSpeed);
        }

        // move backward
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            transform.position += new Vector3(0, 0, -walkSpeed);
        }

        // move left
        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            transform.position += new Vector3(-walkSpeed, 0, 0);
        }

        // move right
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            transform.position += new Vector3(walkSpeed, 0, 0);
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            transform.position += new Vector3(0, jumpHeight, 0);
        }
    }
}
