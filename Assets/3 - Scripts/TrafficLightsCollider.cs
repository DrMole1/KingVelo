using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsCollider : MonoBehaviour
{
    public enum UserCollider { Player, Car, StopZone };

    // ====================== VARIABLES ======================

    [SerializeField] private UserCollider user;

    private TrafficLights trafficLight;
    private Coroutine coroutine;
    private bool canAddScore = true;
    private Auto currentAuto;

    private SoundManager soundManager;
    private ScoreManager scoreManager;

    // =======================================================


    private void Awake()
    {
        trafficLight = transform.parent.GetComponent<TrafficLights>();
        scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>();
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(trafficLight.getColor() != TrafficLights.Color.Red) { return; }

        if (other.CompareTag("Player") && user == UserCollider.Player && !trafficLight.hasSign)
        {
            playerExceed();
        }

        if (other.CompareTag("Car") && user == UserCollider.Car)
        {
            stopCar(other);
        }

        if (other.CompareTag("Player") && user == UserCollider.Car)
        {
            coroutine = StartCoroutine(ICheckPlayer(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && user == UserCollider.Car)
        {
            if (coroutine != null) { StopCoroutine(coroutine); }
        }
    }

    private void playerExceed()
    {
        if (soundManager != null) { soundManager.playAudioClip(17); }

        scoreManager.substractScore(100);
    }

    private void stopCar(Collider other)
    {
        currentAuto = other.transform.GetComponent<Auto>();

        currentAuto.callDecreaseSpeed();
    }

    public void startCar()
    {
        if(currentAuto == null) { return; }

        currentAuto.callIncreaseSpeed();

        currentAuto = null;
    }

    private IEnumerator ICheckPlayer(Collider other)
    {
        yield return new WaitForSeconds(1f);

        if(other.transform.parent.GetComponent<PlayerMovement>().getSpeed() < 0.5f && canAddScore)
        {
            playerStop();
            canAddScore = false;
        }

        coroutine = StartCoroutine(ICheckPlayer(other));
    }

    private void playerStop()
    {
        scoreManager.addScore(100, false);
    }
}
