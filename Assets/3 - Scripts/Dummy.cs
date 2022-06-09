using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private const float SPEED_ROTATE = 0.75f;
    private const float MIN_ROTATE = -5f;
    private const float MAX_ROTATE = 5f;
    private const float DELAY_TO_WAIT = 0.05f;
    private const float DELAY_TO_ADD = 0.02375f;
    private const float MIN_ROTATE_ARM = -40f;
    private const float MAX_ROTATE_ARM = 40f;
    private const float ADD_ROTATE_ARM = 1.5f;
    private const float ADD_ROTATE_ARM_TURN = 3f;
    private const float ADD_ROTATE_ARM_ALERT = 6f;

    enum Movement { Tic, Tac };

    // ====================== VARIABLES ======================

    [SerializeField] private int id = 0;
    [SerializeField] private Material[] m_Skin;
    [SerializeField] private MeshRenderer[] meshesToPaint;
    [SerializeField] private GameObject alertSign;
    [SerializeField] private Transform alertedPosRoot;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;

    private Movement movement = Movement.Tac;
    private Coroutine coroutineMove;
    private Coroutine coroutineRotate;
    private Coroutine coroutineTurn;
    private Movement movementLeftArm = Movement.Tac;
    private Movement movementRightArm = Movement.Tic;
    private float rotLeft = 0;
    private Vector3 currentRotLeft;
    private Quaternion currentQuaternionRotLeft;
    private float rotRight = 0;
    private Vector3 currentRotRight;
    private Quaternion currentQuaternionRotRight;

    private float rot = 0;
    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;
    private bool isOnTurn = false;

    private bool isAlerted = false;
    private bool onAlert = false;

    private ScoreManager scoreManager;
    private PlayerManager playerManager;
    private SoundManager soundManager;

    // =======================================================


    private void Awake()
    {
        scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    private void Start()
    {
        coroutineTurn = StartCoroutine(ITurn());
        StartCoroutine(IMoveArmLeft());
        StartCoroutine(IMoveArmRight());
    }


    public void paintObject()
    {
        Material mat = m_Skin[Random.Range(0, m_Skin.Length)];

        for (int i = 0; i < meshesToPaint.Length; i++)
        {
            meshesToPaint[i].material = mat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAlerted) { return; }

        if (other.CompareTag("Player"))
        {
            if(coroutineMove != null) { StopCoroutine(coroutineMove); }
            if(coroutineRotate != null) { StopCoroutine(coroutineRotate); }
            if (coroutineTurn != null) { StopCoroutine(coroutineTurn); }

            hurtDummy();
            scoreManager.substractScore(100);
            playerManager.takeDamage();
        }
    }

    private void hurtDummy()
    {
        if (soundManager != null) { soundManager.playAudioClip(18); }
        int choice = Random.Range(21, 24);
        if (soundManager != null) { soundManager.playAudioClip(choice); }

        Rigidbody rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = false;
        rb.isKinematic = false;
        rb.AddForce(transform.up * 1500f);
        rb.AddForce(transform.forward * -500f);
        rb.AddTorque(transform.forward * 1500f);
        rb.AddTorque(transform.up * 1500f);
        gameObject.layer = 10;
    }

    private IEnumerator ITurn()
    {
        isOnTurn = true;

        coroutineRotate = StartCoroutine(IReachRotation());
        coroutineMove = StartCoroutine(IMoveBody());

        yield return new WaitForSeconds(2f);

        isOnTurn = false;

        yield return new WaitForSeconds(1f);

        coroutineTurn = StartCoroutine(ITurn());
    }

    private IEnumerator IReachRotation()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;
        Vector3 posToSee = new Vector3(Random.Range(posX - 50f, posX + 50f), transform.position.y, Random.Range(posZ - 50f, posZ + 50f));
        Vector3 targetDirection = transform.position - posToSee;

        while (isOnTurn)
        {
            float singleStep = SPEED_ROTATE * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return null;
        }
    }

    private IEnumerator IMoveBody()
    {
        if (movement == Movement.Tic)
        {
            rot += 0.5f;
        }
        else
        {
            rot -= 0.5f;
        }

        currentRot = new Vector3(rot, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        currentQuaternionRot.eulerAngles = currentRot;
        transform.localRotation = currentQuaternionRot;


        yield return new WaitForSeconds(DELAY_TO_ADD);

        if (movement == Movement.Tic && rot >= MAX_ROTATE)
        {
            yield return new WaitForSeconds(DELAY_TO_WAIT);
            movement = Movement.Tac;
        }

        if (movement == Movement.Tac && rot <= MIN_ROTATE)
        {
            yield return new WaitForSeconds(DELAY_TO_WAIT);
            movement = Movement.Tic;
        }

        if (isOnTurn) { coroutineMove = StartCoroutine(IMoveBody()); }
    }

    public void alertDummy()
    {
        if(isAlerted) { return; }

        isAlerted = true;
        if (coroutineMove != null) { StopCoroutine(coroutineMove); }
        if (coroutineRotate != null) { StopCoroutine(coroutineRotate); }
        if (coroutineTurn != null) { StopCoroutine(coroutineTurn); }

        alertSign.SetActive(true);
        Vector3 posToReach = alertedPosRoot.GetChild(id).position;

        scoreManager.addScore(100, false);

        StartCoroutine(IMoveToAlertedPos(posToReach));
    }

    private IEnumerator IMoveToAlertedPos(Vector3 _pos)
    {
        onAlert = true;
        StartCoroutine(IRotateAlerted(_pos));

        while (Vector3.Distance(transform.position, _pos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pos, 5f * Time.deltaTime);

            yield return null;
        }

        onAlert = false;
        alertSign.SetActive(false);
    }

    private IEnumerator IRotateAlerted(Vector3 _pos)
    {
        Vector3 targetDirection = _pos - transform.position;

        while (onAlert)
        {
            float singleStep = 50 * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return null;
        }
    }

    private IEnumerator IMoveArmLeft()
    {
        if (movementLeftArm == Movement.Tic)
        {
            if (isAlerted) { rotLeft += ADD_ROTATE_ARM_ALERT; }
            else if (isOnTurn) { rotLeft += ADD_ROTATE_ARM_TURN; }
            else { rotLeft += ADD_ROTATE_ARM; }
        }
        else
        {
            if (isAlerted) { rotLeft -= ADD_ROTATE_ARM_ALERT; }
            else if (isOnTurn) { rotLeft -= ADD_ROTATE_ARM_TURN; }
            else { rotLeft -= ADD_ROTATE_ARM; }
        }

        currentRotLeft = new Vector3(leftArm.localRotation.eulerAngles.x, leftArm.localRotation.eulerAngles.y, rotLeft);
        currentQuaternionRotLeft.eulerAngles = currentRotLeft;
        leftArm.localRotation = currentQuaternionRotLeft;


        yield return new WaitForSeconds(DELAY_TO_ADD);

        if (movementLeftArm == Movement.Tic && rotLeft >= MAX_ROTATE_ARM)
        {
            yield return new WaitForSeconds(DELAY_TO_WAIT);
            movementLeftArm = Movement.Tac;
        }

        if (movementLeftArm == Movement.Tac && rotLeft <= MIN_ROTATE_ARM)
        {
            yield return new WaitForSeconds(DELAY_TO_WAIT);
            movementLeftArm = Movement.Tic;
        }

        StartCoroutine(IMoveArmLeft());
    }

    private IEnumerator IMoveArmRight()
    {
        if (movementRightArm == Movement.Tic)
        {
            if (isAlerted) { rotRight += ADD_ROTATE_ARM_ALERT; }
            else if (isOnTurn) { rotRight += ADD_ROTATE_ARM_TURN; }
            else { rotRight += ADD_ROTATE_ARM; }
        }
        else
        {
            if (isAlerted) { rotRight -= ADD_ROTATE_ARM_ALERT; }
            else if (isOnTurn) { rotRight -= ADD_ROTATE_ARM_TURN; }
            else { rotRight -= ADD_ROTATE_ARM; }
        }

        currentRotRight = new Vector3(rightArm.localRotation.eulerAngles.x, rightArm.localRotation.eulerAngles.y, rotRight + 180);
        currentQuaternionRotRight.eulerAngles = currentRotRight;
        rightArm.localRotation = currentQuaternionRotRight;


        yield return new WaitForSeconds(DELAY_TO_ADD);

        if (movementRightArm == Movement.Tic && rotRight >= MAX_ROTATE_ARM)
        {
            yield return new WaitForSeconds(DELAY_TO_WAIT);
            movementRightArm = Movement.Tac;
        }

        if (movementRightArm == Movement.Tac && rotRight <= MIN_ROTATE_ARM)
        {
            yield return new WaitForSeconds(DELAY_TO_WAIT);
            movementRightArm = Movement.Tic;
        }

        StartCoroutine(IMoveArmRight());
    }
}
