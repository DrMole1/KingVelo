using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brake : MonoBehaviour
{
    private const float MAX_SPEED = 5f;
    private const float ADD_SPEED = 0.05f;
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private PlayerMovement playerMovement;

    private float speed;
    private bool isOnButton = false;

    private Coroutine increaseSpeed;
    private Coroutine decreaseSpeed;

    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    // =====================================================


    private void Awake()
    {
        speed = MAX_SPEED;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isOnButton && playerMovement.getCanMove())
        {
            playerMovement.setIsBrakePressed(true);
            playerMovement.setLateralSpeed(0f);

            if (increaseSpeed != null) { StopCoroutine(increaseSpeed); }

            decreaseSpeed = StartCoroutine(IDecreaseSpeed());

            currentRot = new Vector3(0, 0, -6f);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;
        }

        if (Input.GetMouseButtonUp(0) && playerMovement.getIsBrakePressed())
        {
            playerMovement.setIsBrakePressed(false);

            if (decreaseSpeed != null) { StopCoroutine(decreaseSpeed); }

            increaseSpeed = StartCoroutine(IIncreaseSpeed());

            currentRot = new Vector3(0, 0, 5f);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;
        }
    }

    void OnMouseOver()
    {
        isOnButton = true;
    }

    void OnMouseExit()
    {
        isOnButton = false;
    }

    private IEnumerator IIncreaseSpeed()
    {
        yield return new WaitForSeconds(DELAY);

        while (speed < MAX_SPEED)
        {
            speed = speed + ADD_SPEED;
            playerMovement.setSpeed(speed);

            yield return new WaitForSeconds(DELAY);
        }

        speed = MAX_SPEED;
        playerMovement.setSpeed(speed);
    }

    private IEnumerator IDecreaseSpeed()
    {
        while(speed > 0)
        {
            speed -= ADD_SPEED;
            playerMovement.setSpeed(speed);

            yield return new WaitForSeconds(DELAY);
        }

        speed = 0f;
        playerMovement.setSpeed(speed);
    }
}
