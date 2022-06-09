using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoHelmet : MonoBehaviour
{
    [SerializeField] private Material[] mat;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ISelectFaces());
    }

    private IEnumerator ISelectFaces()
    {
        for(int i = 0; i < 8; i++)
        {
            int choice = Random.Range(0, transform.childCount);
            changeMat(transform.GetChild(choice).GetComponent<MeshRenderer>());
        }

        yield return new WaitForSeconds(0.01f);

        StartCoroutine(ISelectFaces());
    }

    private void changeMat(MeshRenderer _rd)
    {
        int choiceMat = Random.Range(0, mat.Length);
        _rd.material = mat[choiceMat];
    }
}
