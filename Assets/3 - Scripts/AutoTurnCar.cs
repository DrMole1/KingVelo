using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurnCar : MonoBehaviour
{
    private const float SPEED_ROTATE = 1f;

    // ====================== VARIABLES ======================

    private Vector3[] posNodes;

    // =======================================================

    private void Awake()
    {
        setPosNodes();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Auto auto = other.transform.GetComponent<Auto>();

            if (auto.getState() == Auto.State.Run) { startAutoTurn(auto); }
            else if (auto.getState() == Auto.State.AutoTurn) { endAutoTurn(auto); }
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

    private void startAutoTurn(Auto _auto)
    {
        _auto.setState(Auto.State.AutoTurn);

        StartCoroutine(IReachNextNode(_auto));
    }

    private void endAutoTurn(Auto _auto)
    {
        _auto.setState(Auto.State.Run);
    }

    private IEnumerator IReachNextNode(Auto _auto)
    {
        _auto.setIsOnTurn(true);

        if (_auto.getState() == Auto.State.AutoTurn) { StartCoroutine(IReachRotation(_auto)); }

        while (_auto.getCurrentNodeToReach() < posNodes.Length && _auto.getState() == Auto.State.AutoTurn)
        {
            Vector3 target = new Vector3(posNodes[_auto.getCurrentNodeToReach()].x, _auto.transform.position.y, posNodes[_auto.getCurrentNodeToReach()].z);

            while (Vector3.Distance(_auto.transform.position, target) > 0.01f && _auto.getState() == Auto.State.AutoTurn)
            {
                _auto.transform.position = Vector3.MoveTowards(_auto.transform.position, target, _auto.getSpeed() / 2 * Time.deltaTime);

                yield return null;
            }

            _auto.setCurrentNodeToReach(_auto.getCurrentNodeToReach() + 1);
        }

        _auto.setIsOnTurn(false);
        _auto.setCurrentNodeToReach(0);
    }

    private IEnumerator IReachRotation(Auto _auto)
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        while (_auto.getIsOnTurn())
        {
            int nodeToLookAt = _auto.getCurrentNodeToReach() + 2;
            if (nodeToLookAt >= transform.childCount) { nodeToLookAt = transform.childCount - 1; }
            Vector3 targetDirection = posNodes[nodeToLookAt] - _auto.transform.position;

            float singleStep = SPEED_ROTATE * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(_auto.transform.forward, targetDirection, singleStep, 0.0f);

            _auto.transform.rotation = Quaternion.LookRotation(newDirection);

            currentRot = new Vector3(0f, _auto.transform.rotation.eulerAngles.y, 0f);
            currentQuaternionRot.eulerAngles = currentRot;
            _auto.transform.rotation = currentQuaternionRot;

            yield return null;
        }
    }
}
