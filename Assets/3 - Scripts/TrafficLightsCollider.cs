using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsCollider : MonoBehaviour
{
    public enum UserCollider { Player, Car };

    // ====================== VARIABLES ======================

    [SerializeField] private UserCollider user;

    private TrafficLights trafficLight;

    // =======================================================


    private void Awake()
    {
        trafficLight = transform.parent.GetComponent<TrafficLights>();
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
            stopCar();
        }
    }

    private void playerExceed()
    {
        print("le joueur brûle un feu rouge");
    }

    private void stopCar()
    {
        print("une voiture doit s'arrêter");
    }
}
