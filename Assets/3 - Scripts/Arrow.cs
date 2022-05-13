using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float speed;
    [SerializeField] private Sprite normalArrow;
    [SerializeField] private Sprite blockedArrow;

    private bool isOnButton = false;

    // =====================================================

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isOnButton)
        {
            playerMovement.setIsArrowPressed(true);
            playerMovement.setLateralSpeed(speed);
        }

        if (Input.GetMouseButtonUp(0) && playerMovement.getIsArrowPressed())
        {
            playerMovement.setIsArrowPressed(false);
            playerMovement.setLateralSpeed(0);
        }
    }

    void OnMouseOver()
    {
        isOnButton = true;
    }

    void OnMouseExit()
    {
        isOnButton = false;
    }

    public void setBlockedSprite()
    {
        GetComponent<Image>().sprite = blockedArrow;
    }

    public void setNormalSprite()
    {
        GetComponent<Image>().sprite = normalArrow;
    }
}
