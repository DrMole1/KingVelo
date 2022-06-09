using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonBike : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject railPref;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        StartCoroutine(ISpawnRail());
    }

    private IEnumerator ISpawnRail()
    {
        if(playerMovement.getSpeed() >= 4.5f && playerMovement.getLateralSpeed() == 0)
        {
            Vector3 currentRot;
            Quaternion currentQuaternionRot = new Quaternion();
            GameObject rail;

            float yRot = playerMovement.transform.localRotation.eulerAngles.y;
            currentRot = new Vector3(90f, yRot, 0f);
            currentQuaternionRot.eulerAngles = currentRot;
            rail = Instantiate(railPref, new Vector3(transform.position.x, 0f, transform.position.z), currentQuaternionRot);
            Destroy(rail, 5f);
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(ISpawnRail());
    }
}
