using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float DELAY_TO_RUN_PLAYER = 3f;

    // ====================== VARIABLES ======================

    [Header("Player Movement")]
    [SerializeField] private PlayerMovement playerMovement;

    private Camera cam;
    [HideInInspector] public bool isGameFinished = false;
    private bool hasStarted = false;

    // =======================================================


    private void Awake()
    {
        cam = Camera.main;
        playerMovement.gameManager = this;
    }


    private void Update()
    {
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && !hasStarted)
        {
            cam.GetComponent<CameraShake>().callIntroTranslation();

            StartCoroutine(IRunPlayer());

            hasStarted = true;
        }
    }

    private IEnumerator IRunPlayer()
    {
        yield return new WaitForSeconds(DELAY_TO_RUN_PLAYER);

        playerMovement.state = PlayerMovement.State.Run;
    }

    public void callStopPlayer()
    {
        isGameFinished = true;

        StartCoroutine(IStopPlayer());
    }

    private IEnumerator IStopPlayer()
    {
        while (playerMovement.getSpeed() > 0)
        {
            playerMovement.setSpeed(playerMovement.getSpeed() - 0.1f);

            yield return new WaitForSeconds(0.02f);
        }

        playerMovement.state = PlayerMovement.State.Stop;
    }
}
