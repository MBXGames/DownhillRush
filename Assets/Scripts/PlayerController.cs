using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int points;
    public bool locked;
    private float tJump;
    private float tBoost;
    private float tGrindPoints;
    private float initFOV;
    private Rigidbody rb;
    public Camera mainCamera;
    public LayerMask groundLayer;
    public float sideMovementSpeed;
    public float jumpForce;
    public bool grounded;
    [Header("Tricks")]
    public bool onTrick;
    public int closeDodgePoints;
    private bool boosting;
    [Header("Grind")]
    public bool grinding;
    public float grindSpeedIncrease;
    public float grindPointTime;
    public Transform maxGroundCheck;
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
        if (Physics.Raycast(transform.position, maxGroundCheck.position - transform.position, (maxGroundCheck.position - transform.position).magnitude, groundLayer))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        if (grounded)
        {
            grinding = false;
        }
        if(boosting || grinding)
        {
            if (grinding)
            {
                tBoost += Time.deltaTime/5;
            }
            else
            {
                tBoost += Time.deltaTime;
            }
            if (tBoost < 0.1)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV, initFOV - 5, tBoost / 0.1f);
            }
            else if(!grinding && tBoost < 1)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV - 5, initFOV, tBoost -0.1f);
            }
            else if(!grinding && tBoost >= 1)
            {
                boosting = false;
            }
        }
        else
        {
            if (mainCamera.fieldOfView < initFOV)
            {
                mainCamera.fieldOfView += Time.deltaTime * 5f;
            }
            else if(mainCamera.fieldOfView > initFOV)
            {
                mainCamera.fieldOfView = initFOV;
            }
        }
        float horizontal = 0;
        if (!grinding)
        {
            horizontal = Input.GetAxis("Horizontal");
        }
        if (locked)
        {
            rb.velocity = Vector3.zero;
        }
        if (grounded)
        {
            rb.velocity = new Vector3(sideMovementSpeed * horizontal, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(sideMovementSpeed/2 * horizontal, rb.velocity.y, rb.velocity.z);
        }
        if (grinding)
        {
            rb.velocity = rb.velocity * (1 + Time.deltaTime * grindSpeedIncrease);
            if(tGrindPoints< grindPointTime)
            {
                tGrindPoints += Time.deltaTime;
            }
            else
            {
                tGrindPoints = 0;
                IncreasePoints();
            }
        }
        if (tJump <= 0.1f)
        {
            tJump += Time.deltaTime;
        }
        if (Input.GetAxisRaw("Vertical")>0)
        {
            if (grounded || grinding)
            {
                if (tJump > 0.1f)
                {
                    tJump = 0;
                    Jump();
                }
                onTrick = false;
            }
            else
            {
                onTrick = true;
            }
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (grounded || grinding)//Agacharse
            {
                onTrick = false;
            }
            else//Truco
            {
                onTrick = true;
            }
        }
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up* jumpForce,ForceMode.Impulse);
    }

    public void SmallHit(float divisor)
    {
        tJump = 0;
        rb.velocity = rb.velocity/ divisor;
    }

    public void DodgeBoost(float boost)
    {
        IncreasePoints(closeDodgePoints);
        if (grounded)
        {
            tJump = 0;
            rb.velocity = rb.velocity * boost;
            if(rb.velocity.magnitude>2)
            {
                tBoost = 0;
                boosting=true;
            }
        }
    }

    public void IncreasePoints(int p=1)
    {
        points += p;
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
