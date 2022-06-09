using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomHelmet : MonoBehaviour
{
    [SerializeField] private Material white;
    [SerializeField] private Material red;
    [SerializeField] private Material yellow;
    [SerializeField] private Material green;
    [SerializeField] private Material blue;

    [SerializeField] private MeshRenderer helmet;
    [SerializeField] private MeshRenderer[] points;

    private void Start()
    {
        StartCoroutine(IChangeColor());
    }

    private IEnumerator IChangeColor()
    {
        yield return new WaitForSeconds(3f);

        int choice = Random.Range(0, 4);

        if(choice == 0) { colorA(); }
        if(choice == 1) { colorB(); }
        if(choice == 2) { colorC(); }
        if(choice == 3) { colorD(); }

        StartCoroutine(IChangeColor());
    }

    private void colorA()
    {
        helmet.material = red;

        for(int i = 0; i < points.Length; i++)
        {
            points[i].material = white;
        }
    }

    private void colorB()
    {
        helmet.material = green;

        for (int i = 0; i < points.Length; i++)
        {
            points[i].material = white;
        }
    }

    private void colorC()
    {
        helmet.material = blue;

        for (int i = 0; i < points.Length; i++)
        {
            points[i].material = white;
        }
    }

    private void colorD()
    {
        helmet.material = yellow;

        for (int i = 0; i < points.Length; i++)
        {
            points[i].material = red;
        }
    }
}
