using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float t;
    private Rigidbody rb;
    public LayerMask groundLayer;
    public float sideMovementSpeed;
    public float jumpForce;
    public bool grounded;
    public Transform maxGroundCheck;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
        {
            rb.velocity = new Vector3(sideMovementSpeed * Input.GetAxis("Horizontal"), rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(sideMovementSpeed/2 * Input.GetAxis("Horizontal"), rb.velocity.y, rb.velocity.z);
        }
        if (t <= 0.1f)
        {
            t+=Time.deltaTime;
        }
        if (Physics.Raycast(transform.position, maxGroundCheck.position - transform.position, (maxGroundCheck.position - transform.position).magnitude,groundLayer))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        if (Input.GetAxisRaw("Vertical")>0)
        {
            if (grounded)
            {
                if (t > 0.1f)
                {
                    t = 0;
                    Jump();
                }
            }
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {

        }
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up* jumpForce,ForceMode.Impulse);
    }

    public void SmallHit(float divisor)
    {
        t = 0;
        rb.velocity = rb.velocity/ divisor;
    }
}
