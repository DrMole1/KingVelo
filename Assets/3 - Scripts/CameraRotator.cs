using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public enum XAxis { None, CenterToLeft, CenterToRight, LeftToCenter, RightToCenter };
    //public enum YAxis { None, CenterToTop, TopToCenter};

    private const float MAX_LEFT_AXIS = -1.2f;
    private const float MAX_RIGHT_AXIS = 1.2f;
    //private const float MAX_TOP_AXIS = 1.9f;
    private const float START_AXIS_X = 0f;
    //private const float START_AXIS_Y = 2.5f;
    private const float ADD_TRANSLATE = 0.02f;

    private const float MAX_LEFT_AXIS_ROTATE = 155f;
    private const float MAX_RIGHT_AXIS_ROTATE = 205f;
    //private const float MAX_TOP_AXIS_ROTATE = 25f;
    //private const float START_AXIS_X_ROTATE = 35f;
    private const float START_AXIS_Y_ROTATE = 180f;
    private const float ADD_ROTATE = 0.4f;

    // ====================== VARIABLES ======================

    [SerializeField] private XAxis xAxis = XAxis.None;
    //[SerializeField] private YAxis yAxis = YAxis.None;

    private Coroutine currentCoroutineX;
    //private Coroutine currentCoroutineY;

    private float currentX = START_AXIS_X;
    //private float currentY = START_AXIS_Y;

    private Coroutine currentCoroutineX_Rotate;
    //private Coroutine currentCoroutineY_Rotate;

    //private float currentX_Rotate = START_AXIS_X_ROTATE;
    private float currentY_Rotate = START_AXIS_Y_ROTATE;

    private Vector3 currentRot;
    private Quaternion currentQuaternionRot;

    // =======================================================


    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if(objectHit.name == "LeftCollider" && xAxis != XAxis.CenterToLeft)
            {
                if(currentCoroutineX != null) { StopCoroutine(currentCoroutineX); }
                currentCoroutineX = StartCoroutine(IMoveCenterToLeft());

                if(currentCoroutineX_Rotate != null) { StopCoroutine(currentCoroutineX_Rotate); }
                currentCoroutineX_Rotate = StartCoroutine(IRotateCenterToLeft());
            }
            else if(xAxis != XAxis.LeftToCenter)
            {
                if (currentCoroutineX != null) { StopCoroutine(currentCoroutineX); }
                currentCoroutineX = StartCoroutine(IMoveLeftToCenter());

                if (currentCoroutineX_Rotate != null) { StopCoroutine(currentCoroutineX_Rotate); }
                currentCoroutineX_Rotate = StartCoroutine(IRotateLeftToCenter());
            }


            if (objectHit.name == "RightCollider" && xAxis != XAxis.CenterToRight)
            {
                if (currentCoroutineX != null) { StopCoroutine(currentCoroutineX); }
                currentCoroutineX = StartCoroutine(IMoveCenterToRight());

                if (currentCoroutineX_Rotate != null) { StopCoroutine(currentCoroutineX_Rotate); }
                currentCoroutineX_Rotate = StartCoroutine(IRotateCenterToRight());
            }
            else if(xAxis != XAxis.RightToCenter)
            {
                if (currentCoroutineX != null) { StopCoroutine(currentCoroutineX); }
                currentCoroutineX = StartCoroutine(IMoveRightToCenter());

                if (currentCoroutineX_Rotate != null) { StopCoroutine(currentCoroutineX_Rotate); }
                currentCoroutineX_Rotate = StartCoroutine(IRotateRightToCenter());
            }


            //if (objectHit.name == "TopCollider" && yAxis != YAxis.CenterToTop)
            //{
            //    if (currentCoroutineY != null) { StopCoroutine(currentCoroutineY); }
            //    currentCoroutineY = StartCoroutine(IMoveCenterToTop());

            //    if (currentCoroutineY_Rotate != null) { StopCoroutine(currentCoroutineY_Rotate); }
            //    currentCoroutineY_Rotate = StartCoroutine(IRotateCenterToTop());
            //}
            //else if(yAxis != YAxis.TopToCenter)
            //{
            //    if (currentCoroutineY != null) { StopCoroutine(currentCoroutineY); }
            //    currentCoroutineY = StartCoroutine(IMoveTopToCenter());

            //    if (currentCoroutineY_Rotate != null) { StopCoroutine(currentCoroutineY_Rotate); }
            //    currentCoroutineY_Rotate = StartCoroutine(IRotateTopToCenter());
            //}
        }
    }



    private void setPosition()
    {
        transform.localPosition = new Vector3(currentX, transform.localPosition.y, transform.localPosition.z);
    }

    private void setRotation()
    {
        currentRot = new Vector3(transform.localRotation.eulerAngles.x, currentY_Rotate, transform.localRotation.eulerAngles.z);
        currentQuaternionRot.eulerAngles = currentRot;
        transform.localRotation = currentQuaternionRot;
    }


    // ============ TRANSLATE ============
    private IEnumerator IMoveCenterToLeft()
    {
        while(transform.localPosition.x > MAX_LEFT_AXIS)
        {
            currentX -= ADD_TRANSLATE;

            setPosition();

            yield return null;
        }
    }

    private IEnumerator IMoveLeftToCenter()
    {
        while (transform.localPosition.x < START_AXIS_X)
        {
            currentX += ADD_TRANSLATE;

            setPosition();

            yield return null;
        }
    }

    private IEnumerator IMoveCenterToRight()
    {
        while (transform.localPosition.x < MAX_RIGHT_AXIS)
        {
            currentX += ADD_TRANSLATE;

            setPosition();

            yield return null;
        }
    }

    private IEnumerator IMoveRightToCenter()
    {
        while (transform.localPosition.x > START_AXIS_X)
        {
            currentX -= ADD_TRANSLATE;

            setPosition();

            yield return null;
        }
    }

    //private IEnumerator IMoveCenterToTop()
    //{
    //    while (transform.localPosition.y > MAX_TOP_AXIS)
    //    {
    //        currentY -= ADD_TRANSLATE;

    //        setPosition();

    //        yield return null;
    //    }
    //}

    //private IEnumerator IMoveTopToCenter()
    //{
    //    while (transform.localPosition.y < START_AXIS_Y)
    //    {
    //        currentY += ADD_TRANSLATE;

    //        setPosition();

    //        yield return null;
    //    }
    //}



    // ============ ROTATE ============

    private IEnumerator IRotateCenterToLeft()
    {
        while (currentY_Rotate > MAX_LEFT_AXIS_ROTATE)
        {
            currentY_Rotate -= ADD_ROTATE;

            setRotation();

            yield return null;
        }
    }

    private IEnumerator IRotateLeftToCenter()
    {
        while (currentY_Rotate < START_AXIS_Y_ROTATE)
        {
            currentY_Rotate += ADD_ROTATE;

            setRotation();

            yield return null;
        }
    }

    private IEnumerator IRotateCenterToRight()
    {
        while (transform.localRotation.eulerAngles.y < MAX_RIGHT_AXIS_ROTATE)
        {
            currentY_Rotate += ADD_ROTATE;

            setRotation();

            yield return null;
        }
    }

    private IEnumerator IRotateRightToCenter()
    {
        while (transform.localRotation.eulerAngles.y > START_AXIS_Y_ROTATE)
        {
            currentY_Rotate -= ADD_ROTATE;

            setRotation();

            yield return null;
        }
    }

    //private IEnumerator IRotateCenterToTop()
    //{
    //    while (currentX_Rotate > MAX_TOP_AXIS_ROTATE)
    //    {
    //        currentX_Rotate -= ADD_ROTATE;

    //        setRotation();

    //        yield return null;
    //    }
    //}

    //private IEnumerator IRotateTopToCenter()
    //{
    //    while (currentX_Rotate < START_AXIS_X_ROTATE)
    //    {
    //        currentX_Rotate += ADD_ROTATE;

    //        setRotation();

    //        yield return null;
    //    }
    //}
}
