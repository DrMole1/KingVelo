using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto : MonoBehaviour
{
    private const float MAX_SPEED = 8f;

    // ====================== VARIABLES ======================

    private float speed;
    private GameManager gameManager;
    private bool canMove = true;

    // =======================================================

    private void Awake()
    {
        speed = MAX_SPEED;
        gameManager = GameObject.Find("SceneManager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if (canMove && !gameManager.isGameFinished)
        {
            moveForward();
        }
    }

    private void moveForward()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destructor"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            print("hurt");
        }        
    }
}
