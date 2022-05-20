using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    // ===================== VARIABLES =====================

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float speed;
    [SerializeField] private Sprite normalArrow;
    [SerializeField] private Sprite blockedArrow;

    private bool isOnButton = false;

    // =====================================================

    public void OnPointerUp(PointerEventData eventData)
    {
        playerMovement.setIsArrowPressed(false);
        playerMovement.setLateralSpeed(0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        playerMovement.setIsArrowPressed(true);
        playerMovement.setLateralSpeed(speed);
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0) && isOnButton)
        //{
        //    playerMovement.setIsArrowPressed(true);
        //    playerMovement.setLateralSpeed(speed);
        //}

        //if (Input.GetMouseButtonUp(0) && playerMovement.getIsArrowPressed())
        //{
        //    playerMovement.setIsArrowPressed(false);
        //    playerMovement.setLateralSpeed(0);
        //}
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
