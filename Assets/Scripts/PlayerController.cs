using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BetweenScenesCanvas betweenScenesCanvas;
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
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timerText;
    private int acumulatedPoints;
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
    private bool movingRight;
    private bool movingLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initFOV = mainCamera.fieldOfView;
        checkpointPosition = transform.position;
        startCameraTransform = mainCamera.transform;
        startCameraLocalPosition = startCameraTransform.localPosition;
        startCameraLocalRotation=startCameraTransform.rotation;
        betweenScenesCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<BetweenScenesCanvas>();
        scoreText = betweenScenesCanvas.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        timerText = betweenScenesCanvas.transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
        acumulatedPoints = betweenScenesCanvas.trackPoints.Sum();
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
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                if(movingLeft && !movingRight)
                {
                    horizontal = -1;
                }
                else if (movingRight && !movingLeft)
                {
                    horizontal = 1;
                }
                else
                {
                    horizontal = 0;
                }
            }
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
            float aux = Time.deltaTime * sideMovementInclinationSpeed / 2;
            aux = Mathf.Clamp(aux, 0, Vector3.Angle(transform.up, model.up));
            if (Vector3.SignedAngle(transform.up, model.up, transform.forward) < 0)
            {
                model.transform.Rotate(transform.forward, aux);
            }
            else if (Vector3.SignedAngle(transform.up, model.up, transform.forward) > 0)
            {
                model.transform.Rotate(transform.forward, -aux);
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
            Jump();
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            Crouch();
        }
        TimerControl();
    }

    public void RightOn()
    {
        movingRight = true;
    }
    public void RightOff()
    {
        movingRight = false;
    }

    public void LeftOn()
    {
        movingLeft = true;
    }
    public void LeftOff()
    {
        movingLeft = false;
    }

    public void Jump()
    {
        if (grounded || grinding)
        {
            if (tJump > 0.1f)
            {
                tJump = 0;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            onTrick = false;
        }
        else
        {
            onTrick = true;
        }
    }

    public void Crouch()
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
            scoreText.text = (acumulatedPoints+points).ToString();
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
        betweenScenesCanvas.HideMovementsButton();
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
            tTimer -= Time.deltaTime*10;
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
            yield return new WaitForSeconds(5/ ((180 - tTimerOnWin) / 5));
        }
        yield return new WaitForSeconds(3);
        betweenScenesCanvas.StoreData(tTimerOnWin, points);
    }
}