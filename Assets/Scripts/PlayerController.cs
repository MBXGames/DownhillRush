using System;
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
    private float tRadical;
    private float tGrindPoints;
    private float initFOV;
    private Rigidbody rb;
    public Camera mainCamera;
    public Transform model;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timerText;
    private int acumulatedPoints;
    public float sideMovementSpeed;
    public float sideMovementInclination = 3;
    public float sideMovementInclinationSpeed=0.5f;
    public float jumpForce;
    public LayerMask groundLayer;
    public bool grounded;
    [Header("Animators")]
    public Animator skateAnimator;
    [Header("BodyModels")]
    public GameObject standingModel;
    public GameObject jumpModel;
    public GameObject crouchingModel;
    public GameObject grindingModel;
    public GameObject[] radicalCapModels;
    [Header("Crouch")]
    public bool crouching;
    [Header("Tricks")]
    public LayerMask rampLayer;
    public bool onTrick;
    private bool failedTrick;
    private bool boosting;
    public bool radicalCap;
    [Header("Grind")]
    public bool grinding;
    private Vector2 startGrindPos;
    private Vector2 endGrindPos;
    public float grindSpeedIncrease;
    public float grindDownForce;
    public float grindPointTime;
    public Transform maxGroundCheck;
    public Vector3 checkpointPosition;
    [Header("Win")]
    public float winMoveCameraTime;
    private Transform startCameraTransform;
    private Vector3 startCameraLocalPosition;
    private Quaternion startCameraLocalRotation;
    public Transform endCameraTransform;
    [Header("Other")]
    public ParticleSystem energyDrinkParticles;
    public ParticleSystem boostParticles;
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
        AnimatorsAdmin();

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
        RaycastHit hit;
        if(Physics.Raycast(transform.position, maxGroundCheck.position - transform.position, out hit, (maxGroundCheck.position - transform.position).magnitude, rampLayer))
        {
            grounded = false;
        }
        if (Physics.Raycast(transform.position, maxGroundCheck.position - transform.position, out hit, (maxGroundCheck.position - transform.position).magnitude, groundLayer))
        {
            if (hit.collider.gameObject.GetComponent<GrindBarController>() != null)
            {
                onTrick = false;
                grinding = true;
                grounded = false;
                startGrindPos = hit.collider.gameObject.GetComponent<GrindBarController>().start.position;
                endGrindPos = hit.collider.gameObject.GetComponent<GrindBarController>().end.position;
            }
            else 
            {
                grounded = true;
                grinding = false;
                onTrick = false;
            }
        }
        else
        {
            grounded = false;
            grinding = false;
        }
        if (grounded)
        {
            grinding = false;
        }
        else
        {
            Uncrouch();
        }
        if (radicalCap)
        {
            if (tRadical < 10)
            {
                tRadical += Time.deltaTime;
            }
            else
            {
                tRadical = 0;
                radicalCap = false;
            }
        }
        if (failedTrick)
        {
            tBoost += Time.deltaTime;
            if (tBoost < 0.25)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV, initFOV +5, tBoost / 0.1f);
            }
            else if (tBoost < 1)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV + 5, initFOV, tBoost - 0.25f);
            }
            if (tBoost >= 1)
            {
                failedTrick = false;
            }
        }
        else if(boosting || grinding)
        {
            if (failedTrick)
            {
                failedTrick = false;
            }
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
            if (failedTrick)
            {
                failedTrick = false;
            }
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
            if (grounded)
            {
                if (!standingModel.gameObject.activeSelf && !crouching)
                {
                    standingModel.gameObject.SetActive(true);
                    crouchingModel.gameObject.SetActive(false);

                }
                else if (!crouchingModel.gameObject.activeSelf && crouching)
                {
                    crouchingModel.gameObject.SetActive(true);
                    standingModel.gameObject.SetActive(false);
                }
                if (grindingModel.gameObject.activeSelf)
                {
                    grindingModel.gameObject.SetActive(false);
                }
                if (jumpModel.gameObject.activeSelf)
                {
                    jumpModel.gameObject.SetActive(false);
                }
            }
            else
            {
                if (standingModel.gameObject.activeSelf)
                {
                    standingModel.gameObject.SetActive(false);
                }
                if (crouchingModel.gameObject.activeSelf)
                {
                    crouchingModel.gameObject.SetActive(false);
                }
                if (grindingModel.gameObject.activeSelf)
                {
                    grindingModel.gameObject.SetActive(false);
                }
                if (!jumpModel.gameObject.activeSelf)
                {
                    jumpModel.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if(standingModel.gameObject.activeSelf)
            {
                standingModel.gameObject.SetActive(false);
            }
            if (crouchingModel.gameObject.activeSelf)
            {
                crouchingModel.gameObject.SetActive(false);
            }
            if (!grindingModel.gameObject.activeSelf)
            {
                grindingModel.gameObject.SetActive(true);
            }
            if (jumpModel.gameObject.activeSelf)
            {
                jumpModel.gameObject.SetActive(false);
            }
            rb.velocity = rb.velocity * (1 + Time.deltaTime * grindSpeedIncrease);
            if (tGrindPoints < grindPointTime)
            {
                tGrindPoints += Time.deltaTime;
            }
            else
            {
                tGrindPoints = 0;
                IncreasePoints();
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
            rb.velocity = new Vector3(sideMovementSpeed / 3 * horizontal, rb.velocity.y, rb.velocity.z);
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
        if (tJump <= 0.25f)
        {
            tJump += Time.deltaTime;
        }
        if (betweenScenesCanvas != null)
        {
            if(!Application.isMobilePlatform && !betweenScenesCanvas.simulateMobile)
            {
                if (Input.GetAxisRaw("Vertical")>0)
                {
                    Jump();
                }
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    Crouch();
                }
                else
                {
                    if (crouching)
                    {
                        Uncrouch();
                    }
                }
            }
        }

        TimerControl();
    }

    private void FixedUpdate()
    {
        if (grinding)
        {
            Vector3 aux = (ClosestPointOnLineToPlayer(startGrindPos, endGrindPos) - transform.position);
            rb.AddForce(new Vector3(aux.x,0,0).normalized*grindDownForce*3, ForceMode.Force);
            rb.AddForce(-transform.up * grindDownForce, ForceMode.Force);
        }
    }

    private void AnimatorsAdmin()
    {
        //Skate
        skateAnimator.SetBool("Grounded",grounded || end);
        skateAnimator.SetBool("Grinding",grinding && !end);
        if (grounded)
        {
            skateAnimator.ResetTrigger("JumpTrick");
            skateAnimator.ResetTrigger("CrouchTrick");
        }
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
            if (tJump > 0.25f)
            {
                tJump = 0;
                skateAnimator.SetTrigger("JumpTrick");
                onTrick = true;
            }
        }
        Uncrouch();
    }

    public void Crouch()
    {
        if (grounded || grinding)//Agacharse
        {
            onTrick = false;
            crouching = true;
            standingModel.SetActive(false);
            grindingModel.SetActive(false);
            crouchingModel.SetActive(true);
        }
        else//Truco
        {
            if (tJump > 0.25f)
            {
                tJump = 0;
                skateAnimator.SetTrigger("CrouchTrick");
                onTrick = true;
            }
        }
    }

    public void Uncrouch()
    {
        if (crouching)
        {
            crouchingModel.SetActive(false);
            grindingModel.SetActive(false);
            standingModel.SetActive(true);
            crouching = false;
        }
    }

    public void SmallHit(float divisor)
    {
        tJump = 0;
        rb.velocity = rb.velocity/ divisor;
    }

    public void DodgeBoost(float boost,int points)
    {
        if (points > 0)
        {
            boostParticles.Play();
        }
        if(rb.velocity.z > 40)
        {
            boost = 1+(boost-1)/2;
        }
        if (rb.velocity.z > 60)
        {
            boost = 1 + (boost - 1) / 2;
        }
        if (grounded)
        {
            tJump = 0;
            rb.velocity = rb.velocity * boost;
            if(rb.velocity.z>5)
            {
                IncreasePoints(points);
                tBoost = 0;
                boosting=true;
            }
        }
    }

    public void FailTrick(int points,bool success)
    {
        IncreasePoints(points);
        if(!success)
        {
            rb.velocity = rb.velocity * 0.8f;
            tBoost = 0;
            failedTrick = true;
        }
        else
        {
            float b = 1.025f;
            if (rb.velocity.z > 40)
            {
                b = 1.0125f;
            }
            rb.velocity = rb.velocity * b;
            if (rb.velocity.z > 5)
            {
                tBoost = 0;
                boosting = true;
            }
        }
    }

    public void IncreasePoints(int p=1)
    {
        if (radicalCap)
        {
            p *= 2;
        }
        points += p;
        if (points < 0)
        {
            points = 0;
        }
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
        radicalCap = false;
        end = true;
        betweenScenesCanvas.HideMovementsButton();
        tTimerOnWin = tTimer;
    }

    public void GetRadicalCap()
    {
        tRadical = 0;
        radicalCap = true;
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

    public void PlayEnergyParticles()
    {
        energyDrinkParticles.Play();
    }

    Vector3 ClosestPointOnLineToPlayer(Vector3 A, Vector3 B)
    {
        Vector3 AB = B - A; // Vector que va de A a B
        Vector3 jugadorA = transform.position - A; // Vector que va de A al jugador

        // Proyecta el vector jugadorA sobre el vector AB
        float t = Vector3.Dot(jugadorA, AB) / Vector3.Dot(AB, AB);

        // Limitar t para que el punto esté en la línea infinita (sin cortar)
        t = Mathf.Clamp01(t);

        // Calcula el punto más cercano
        return A + t * AB;
    }
}