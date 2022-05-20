using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto : MonoBehaviour
{
    private const float MAX_SPEED = 8f;

    public enum State { Stop, Run, AutoTurn };

    // ====================== VARIABLES ======================

    private State state = State.Run;
    private bool isOnAutoTurn = false;
    private float speed;
    private GameManager gameManager;
    private bool canMove = true;
    public int currentNodeToReach = 0;

    // =======================================================

    public void setState(State _state) { state = _state; }
    public State getState() { return state; }
    public void setIsOnTurn(bool _isOnTurn) { isOnAutoTurn = _isOnTurn; }
    public bool getIsOnTurn() { return isOnAutoTurn; }
    public void setSpeed(float _speed) { speed = _speed; }
    public float getSpeed() { return speed; }
    public void setCurrentNodeToReach(int _node) { currentNodeToReach = _node; }
    public int getCurrentNodeToReach() { return currentNodeToReach; }

    private void Awake()
    {
        speed = MAX_SPEED;
        gameManager = GameObject.Find("SceneManager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if (state == State.Run && canMove && !gameManager.isGameFinished)
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
            other.transform.parent.gameObject.GetComponent<PlayerManager>().takeDamage();
        }        
    }
}
