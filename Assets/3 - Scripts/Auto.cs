using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto : MonoBehaviour
{
    private const float MAX_SPEED = 8f;
    private const float MIN_SPEED = 0f;

    public enum State { Stop, Run, AutoTurn };

    // ====================== VARIABLES ======================

    private State state = State.Run;
    private float speed;
    private int currentNodeToReach = 0;
    private bool isOnAutoTurn = false;
    private bool canMove = true;
    private Coroutine decreaseCoroutine;
    private Coroutine increaseCoroutine;
    private GameManager gameManager;

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

    public void callDecreaseSpeed()
    {
        if(increaseCoroutine != null) { StopCoroutine(increaseCoroutine); }
        decreaseCoroutine = StartCoroutine(IDecreaseSpeed());
    }

    public void callIncreaseSpeed()
    {
        if (decreaseCoroutine != null) { StopCoroutine(decreaseCoroutine); }
        increaseCoroutine = StartCoroutine(IIncreaseSpeed());
    }

    private IEnumerator IDecreaseSpeed()
    {
        while(speed > MIN_SPEED)
        {
            yield return new WaitForSeconds(0.01f);

            speed -= 0.2f;
        }

        speed = MIN_SPEED;
    }

    private IEnumerator IIncreaseSpeed()
    {
        while(speed < MAX_SPEED)
        {
            yield return new WaitForSeconds(0.01f);

            speed += 0.2f;
        }

        speed = MAX_SPEED;
    }
}
