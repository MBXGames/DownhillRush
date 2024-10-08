using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool locked;
    private float t;
    private float t2;
    private float initFOV;
    private Rigidbody rb;
    public Camera mainCamera;
    public LayerMask groundLayer;
    public float sideMovementSpeed;
    public float jumpForce;
    public bool grounded;
    public Transform maxGroundCheck;
    private bool boosting;
    public Vector3 checkpointPosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initFOV = mainCamera.fieldOfView;
        checkpointPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(boosting)
        {
            t2+=Time.deltaTime;
            if (t2 < 0.1)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV, initFOV - 5, t2 / 0.1f);
            }
            else if(t2 < 1)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV - 5, initFOV, t2 -0.1f);
            }
            else if(t2 >= 1)
            {
                boosting = false;
            }
        }
        if (locked)
        {
            rb.velocity = Vector3.zero;
        }
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
        if (Input.GetAxisRaw("Vertical") < 0)
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

    public void Boost(float boost)
    {
        if (grounded)
        {
            t = 0;
            rb.velocity = rb.velocity * boost;
            if(rb.velocity.magnitude>2)
            {
                t2 = 0;
                boosting=true;
            }
        }
    }

    public void SetCheckpointPosition(Vector3 pos)
    {
        checkpointPosition = pos;
    }

    public void ResetToCheckpoint()
    {
        transform.position=checkpointPosition;
    }

    public void LockPlayer()
    {
        rb.velocity = Vector3.zero;
        locked = true;
    }

    public void UnlockPlayer()
    {
        locked = false;
    }
}
