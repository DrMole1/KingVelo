using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum State { Stop, Run, AutoTurn};

    private const float MAX_SPEED = 5f;
    private const float LATERAL_FACTOR = 5f;
    private const float PEDALS_SPEED = 60f;

    // ====================== VARIABLES ======================

    public State state;

    [SerializeField] private float speedForward;
    private float speedLateral;
    private bool canMove = true;
    private bool isArrowPressed = false;
    private bool isBrakePressed = false;
    private bool isPedaling = false;
    private float brakeFactor = 1f;

    [HideInInspector] public GameManager gameManager;

    private Rotate pedals;
    public GameObject[] wheels = new GameObject[2];

    [Header("Legs")]
    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;
    private int cpt = 0;
    private float addLeft = -1;
    private float addRight = 1;

    // =======================================================


    public bool getCanMove() { return canMove; }
    public void setCanMove(bool _canMove) { canMove = _canMove; }
    public float getSpeed() { return speedForward; }
    public void setSpeed(float _speed) { speedForward = _speed; }
    public float getLateralSpeed() { return speedLateral; }
    public void setLateralSpeed(float _speed) { speedLateral = _speed; }
    public bool getIsArrowPressed() { return isArrowPressed; }
    public void setIsArrowPressed(bool _isPressed) { isArrowPressed = _isPressed; }
    public bool getIsBrakePressed() { return isBrakePressed; }
    public void setIsBrakePressed(bool _isPressed) { isBrakePressed = _isPressed; }
    public float getBrakeFactor() { return brakeFactor; }
    public void setBrakeFactor(float _factor) { brakeFactor = _factor; }

    private void Awake()
    {
        speedForward = MAX_SPEED;
    }

    private void Start()
    {
        if (GameObject.Find("PEDAL") != null) { pedals = GameObject.Find("PEDAL").GetComponent<Rotate>(); }

        wheels = GameObject.FindGameObjectsWithTag("Wheel");

        StartCoroutine(IMoveLegs());
    }

    private void FixedUpdate()
    {
        if (state == State.Run)
        {
            if (canMove && !gameManager.isGameFinished)
            {
                calculateLateralSpeed();
            }

            movePlayerRun();
        }

        if(state != State.Stop)
        {
            setElementRotatorWithSpeed();
        }

        if(state == State.Stop || isBrakePressed) { isPedaling = false; }
        else { isPedaling = true; }
    }


    private void movePlayerRun()
    {
        if (state == State.Run && canMove)
        {
            transform.Translate(speedLateral * Time.deltaTime, 0f, - speedForward * Time.deltaTime);
        }
    }

    private void calculateLateralSpeed()
    {
        if (!isArrowPressed && !isBrakePressed) { speedLateral = Input.GetAxis("Horizontal") * LATERAL_FACTOR * brakeFactor; }
    }

    private void setElementRotatorWithSpeed()
    {
        float rotateSpeed = speedForward * PEDALS_SPEED;

        if (pedals != null) { pedals.yAngle = rotateSpeed; }
        if (wheels[0] != null) { wheels[0].GetComponent<Rotate>().yAngle = rotateSpeed; }
        if (wheels[1] != null) { wheels[1].GetComponent<Rotate>().yAngle = rotateSpeed; }
    }

    private IEnumerator IMoveLegs()
    {
        while(isPedaling)
        {
            Vector3 currentRot;
            Quaternion currentQuaternionRot = new Quaternion();

            currentRot = new Vector3(0f, 0f, leftLeg.localRotation.eulerAngles.z + addLeft);
            currentQuaternionRot.eulerAngles = currentRot;
            leftLeg.localRotation = currentQuaternionRot;

            currentRot = new Vector3(0f, 0f, rightLeg.localRotation.eulerAngles.z + addRight);
            currentQuaternionRot.eulerAngles = currentRot;
            rightLeg.localRotation = currentQuaternionRot;

            yield return new WaitForSeconds(0.02f);

            cpt++;

            if(cpt >= 60)
            {
                cpt = 0;
                float temp = addLeft;
                addLeft = addRight;
                addRight = temp;
            }
        }

        yield return null;

        StartCoroutine(IMoveLegs());
    }
}

