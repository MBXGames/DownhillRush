using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int points;
    public bool locked;
    private float tTimer;
    private float tTimerOnWin;
    private float tTimeTransform;
    public float tCameraEnd;
    private float tJump;
    private float tBoost;
    private float tGrindPoints;
    private float initFOV;
    private Rigidbody rb;
    public Camera mainCamera;
    public LayerMask groundLayer;
    public Transform model;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public float sideMovementSpeed;
    public float sideMovementInclination = 3;
    public float sideMovementInclinationSpeed=0.5f;
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
    [Header("Win")]
    public float winMoveCameraTime;
    private Transform startCameraTransform;
    private Vector3 startCameraLocalPosition;
    private Quaternion startCameraLocalRotation;
    public Transform endCameraTransform;
    private bool end;
    private bool timeAdded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initFOV = mainCamera.fieldOfView;
        checkpointPosition = transform.position;
        startCameraTransform = mainCamera.transform;
        startCameraLocalPosition = startCameraTransform.localPosition;
        startCameraLocalRotation=startCameraTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            if (tCameraEnd< 1)
            {
                tCameraEnd += Time.deltaTime / winMoveCameraTime;
                mainCamera.transform.localPosition = Vector3.Lerp(startCameraLocalPosition, endCameraTransform.localPosition, tCameraEnd);
                mainCamera.transform.localRotation = Quaternion.Lerp(startCameraLocalRotation, endCameraTransform.localRotation, tCameraEnd);
            }
            else
            {
                TimeToPoints();
            }
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            return;
        }
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
            horizontal = Input.GetAxisRaw("Horizontal");
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
        if (horizontal < 0)
        {
            if (Vector3.SignedAngle(transform.up,model.up,transform.forward)<sideMovementInclination)
            {
                model.transform.Rotate(transform.forward, Time.deltaTime * sideMovementInclinationSpeed);
            }
        }
        if (horizontal > 0)
        {
            if (Vector3.SignedAngle(transform.up, model.up, transform.forward) > -sideMovementInclination)
            {
                model.transform.Rotate(transform.forward, -Time.deltaTime * sideMovementInclinationSpeed);
            }
        }
        else if (horizontal == 0)
        {
            if (Vector3.SignedAngle(transform.up, model.up, transform.forward) < 0)
            {
                model.transform.Rotate(transform.forward, Time.deltaTime * sideMovementInclinationSpeed / 2);
            }
            else if (Vector3.SignedAngle(transform.up, model.up, transform.forward) > 0)
            {
                model.transform.Rotate(transform.forward, -Time.deltaTime * sideMovementInclinationSpeed / 2);
            }
            if (Vector3.SignedAngle(transform.up, model.up, transform.forward) != 0 && Vector3.Angle(transform.up, model.up) < 0.05)
            {
                model.eulerAngles = new Vector3(0, 0, 0);
            }
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
        TimerControl();
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
            if(rb.velocity.z>5)
            {
                tBoost = 0;
                boosting=true;
            }
        }
    }

    public void IncreasePoints(int p=1)
    {
        points += p;
        if (scoreText != null)
        {
            scoreText.text = points.ToString();
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

    public void PlayerEnd()
    {
        end = true;
        tTimerOnWin = tTimer;
    }

    public void TimerControl()
    {
        if (!locked && !end)
        {
            tTimer += Time.deltaTime;
            TimerShow();
        }
    }

    public void TimerShow()
    {
        float min = 0;
        string minst = "00";
        float sec = 0;
        string secst = "00";
        if (tTimer >= 60)
        {
            min = Mathf.FloorToInt(tTimer / 60);
            if (min < 10)
            {
                minst = "0" + min.ToString();
            }
            else
            {
                minst = min.ToString();
            }
        }
        sec = tTimer % 60;
        if (sec < 10)
        {
            secst = "0" + sec.ToString("F2");
        }
        else
        {
            secst = sec.ToString("F2");
        }
        timerText.text = minst + ":" + secst;
    }

    public void TimeToPoints()
    {
        if (tTimer > 0)
        {
            tTimer -= Time.deltaTime*5;
            TimerShow();
        }
        if(tTimer < 0)
        {
            tTimer =0;
            TimerShow();
        }
        if (!timeAdded)
        {
            timeAdded= true;
            StartCoroutine(TimeToPointsAddition());
        }
    }

    private IEnumerator TimeToPointsAddition()
    {
        for (int i = 0; i < (180 - tTimerOnWin) / 5; i ++)
        {
            IncreasePoints();
            yield return new WaitForSeconds(10/ ((180 - tTimerOnWin) / 5));
        }
    }
}