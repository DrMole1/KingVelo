using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // ========================== VARIABLES ==========================

    [Header("Properties")]
    [SerializeField] private float xAngle;
    [SerializeField] private float yAngle;
    [SerializeField] private float zAngle;

    private bool canRotate = true;

    // ===============================================================

    public void Update()
    {
        if (canRotate) { RotateObject(); }
    }

    private void RotateObject()
    {
        transform.Rotate(xAngle * Time.deltaTime, yAngle * Time.deltaTime, zAngle * Time.deltaTime, Space.Self);
    }
}
