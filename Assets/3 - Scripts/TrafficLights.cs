using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{
    public enum Color { Red, Yellow, Green };

    private const float MIN_DELAY = 3f;
    private const float MAX_DELAY = 7f;

    // ====================== VARIABLES ======================

    public bool hasSign = false;

    [Header("Signs for the Player")]
    [SerializeField] private Material[] m_Color;
    [SerializeField] private Sprite[] spr_Color;
    [SerializeField] private MeshRenderer[] mesh_lights;
    [SerializeField] private SpriteRenderer[] rd_lights;

    private Color color;

    // =======================================================

    public void setColor(Color _color) { color = _color; }
    public Color getColor() { return color; }


    private void Start()
    {
        StartCoroutine(IChangeColor());
    }

    private void updateLights(Color _color)
    {
        color = _color;

        updateMeshes();
        updateSprites();
    }

    private void updateMeshes()
    {
        for(int i = 0; i < mesh_lights.Length; i++)
        {
            if((int)color == i) { mesh_lights[i].material = m_Color[i]; }
            else { mesh_lights[i].material = m_Color[3]; }
        }
    }

    private void updateSprites()
    {
        for (int i = 0; i < rd_lights.Length; i++)
        {
            if ((int)color == i) { rd_lights[i].sprite = spr_Color[i]; }
            else { rd_lights[i].sprite = spr_Color[3]; }
        }
    }

    private IEnumerator IChangeColor()
    {
        float delay = Random.Range(MIN_DELAY, MAX_DELAY);

        updateLights(Color.Green);

        yield return new WaitForSeconds(delay);

        updateLights(Color.Yellow);

        yield return new WaitForSeconds(5f);

        updateLights(Color.Red);

        yield return new WaitForSeconds(3f);

        StartCoroutine(IChangeColor());
    }
}
