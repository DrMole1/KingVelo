using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurn : MonoBehaviour
{
    public enum Role { Start, End };

    private const float SPEED_ROTATE = 0.75f;

    // ====================== VARIABLES ======================

    [SerializeField] private Role role;
    [SerializeField] private GameObject ptcCheckPref;
    [SerializeField] private GameObject nextPtc;

    private bool isOnTurn = false;
    private int currentNodeToReach = 0;
    private Vector3[] posNodes;
    private Transform player;
    private PlayerMovement playerMovement;
    private ScoreManager scoreManager;
    private SoundManager soundManager;
    private MusicManager musicManager;

    // =======================================================


    private void Awake()
    {
        scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>();
        if (role == Role.Start) { setPosNodes(); }
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
        if (GameObject.Find("MusicManager") != null) { musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform.parent;

            if(role == Role.Start) { startAutoTurn(); animateCheckPoint(); }
            else if (role == Role.End) { endAutoTurn(); }
        }
    }

    private void setPosNodes()
    {
        posNodes = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            posNodes[i] = transform.GetChild(i).position;
        }
    }

    private void startAutoTurn()
    {
        if (musicManager != null) { musicManager.callDecreasePitch(0.85f); }

        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.state = PlayerMovement.State.AutoTurn;

        GameObject.Find("[BTN]Left_Arrow").GetComponent<Arrow>().setBlockedSprite();
        GameObject.Find("[BTN]Right_Arrow").GetComponent<Arrow>().setBlockedSprite();

        StartCoroutine(IReachNextNode());
    }

    private void endAutoTurn()
    {
        if (musicManager != null) { musicManager.callIncreasePitch(1f); }


        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.state = PlayerMovement.State.Run;

        GameObject.Find("[BTN]Left_Arrow").GetComponent<Arrow>().setNormalSprite();
        GameObject.Find("[BTN]Right_Arrow").GetComponent<Arrow>().setNormalSprite();
    }

    private IEnumerator IReachNextNode()
    {
        isOnTurn = true;

        if (playerMovement.state == PlayerMovement.State.AutoTurn) { StartCoroutine(IReachRotation()); }

        while (currentNodeToReach < posNodes.Length && playerMovement.state == PlayerMovement.State.AutoTurn)
        {
            Vector3 target = new Vector3(posNodes[currentNodeToReach].x, player.position.y, posNodes[currentNodeToReach].z);

            while (Vector3.Distance(player.position, target) > 0.01f && playerMovement.state == PlayerMovement.State.AutoTurn)
            {
                player.position = Vector3.MoveTowards(player.position, target, playerMovement.getSpeed() * Time.deltaTime);

                yield return null;
            }

            currentNodeToReach++;
        }

        isOnTurn = false;
    }

    private IEnumerator IReachRotation()
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        while (isOnTurn)
        {
            int nodeToLookAt = currentNodeToReach + 2;
            if (nodeToLookAt >= transform.childCount) { nodeToLookAt = transform.childCount - 1; }
            Vector3 targetDirection = player.position - posNodes[nodeToLookAt];

            float singleStep = SPEED_ROTATE * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(player.forward, targetDirection, singleStep, 0.0f);

            player.rotation = Quaternion.LookRotation(newDirection);

            currentRot = new Vector3(0f, player.rotation.eulerAngles.y, 0f);
            currentQuaternionRot.eulerAngles = currentRot;
            player.rotation = currentQuaternionRot;

            yield return null;
        }
    }

    private void animateCheckPoint()
    {
        if (soundManager != null) { soundManager.playAudioClip(19); }

        scoreManager.addScore(50, false);

        Transform ptcRoot = transform.parent.GetChild(2);
        Quaternion quat = ptcRoot.rotation;
        Vector3 pos = ptcRoot.position;

        GameObject ptcCheck;
        ptcCheck = Instantiate(ptcCheckPref, pos, quat);
        Destroy(ptcCheck, 3f);

        Destroy(ptcRoot.gameObject);

        if(nextPtc != null) { nextPtc.SetActive(true); }
    }
}
