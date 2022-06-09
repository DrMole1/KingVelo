using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Brake : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private const float MAX_SPEED = 5f;
    private const float ADD_SPEED = 0.05f;
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private PlayerMovement playerMovement;

    private float speed;

    private Coroutine increaseSpeed;
    private Coroutine decreaseSpeed;

    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        speed = MAX_SPEED;
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerMovement.setIsBrakePressed(false);

        if (decreaseSpeed != null) { StopCoroutine(decreaseSpeed); }

        increaseSpeed = StartCoroutine(IIncreaseSpeed());

        currentRot = new Vector3(0, 0, 5f);
        currentQuaternionRot.eulerAngles = currentRot;
        transform.localRotation = currentQuaternionRot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (soundManager != null) { soundManager.playAudioClip(16); }

        playerMovement.setIsBrakePressed(true);
        playerMovement.setLateralSpeed(0f);
        playerMovement.setBrakeFactor(0f);

        if (increaseSpeed != null) { StopCoroutine(increaseSpeed); }

        decreaseSpeed = StartCoroutine(IDecreaseSpeed());

        currentRot = new Vector3(0, 0, -6f);
        currentQuaternionRot.eulerAngles = currentRot;
        transform.localRotation = currentQuaternionRot;
    }

    private IEnumerator IIncreaseSpeed()
    {
        yield return new WaitForSeconds(DELAY);

        while (speed < MAX_SPEED)
        {
            speed = speed + ADD_SPEED;
            playerMovement.setSpeed(speed);

            if (playerMovement.getBrakeFactor() < 1f) { playerMovement.setBrakeFactor(playerMovement.getBrakeFactor() + 0.005f); }

            yield return new WaitForSeconds(DELAY);
        }

        playerMovement.setBrakeFactor(1f);
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
