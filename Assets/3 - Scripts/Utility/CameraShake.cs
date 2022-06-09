using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2D CAMERA SHAKE
public class CameraShake : MonoBehaviour
{
    // ===================== VARIABLES =====================

    public static CameraShake Instance;
    public bool isShaking = false;
    [HideInInspector] public Coroutine shakeCoroutine;

    private bool forcedShaking = false;

    private Camera cam;

    // ====================================================

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void Shake(float _duration, float _intensity)
    {
        if(!isShaking)
        {
            shakeCoroutine = StartCoroutine(IShake(_duration, _intensity));
        }
    }

    IEnumerator IShake(float _duration, float _intensity)
    {
        Quaternion originalRotation = cam.transform.localRotation;
        isShaking = true;

        float cpt = 0;

        while (cpt <= _duration && !forcedShaking)
        {
            float x = Random.Range(-_intensity, _intensity);
            float y = Random.Range(-_intensity, _intensity);
            float z = Random.Range(-_intensity, _intensity);

            cam.transform.localRotation = Quaternion.Euler(originalRotation.eulerAngles.x + x, originalRotation.eulerAngles.y + y, originalRotation.eulerAngles.z + z);

            cpt += Time.deltaTime;
            yield return null;
        }

        cam.transform.localRotation = originalRotation;
        isShaking = false;
    }

    public void StopShaking()
    {
        if(shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
    }

    private IEnumerator IZoom(float _speedZoom, float _value)
    {
        float speed = (1 / _speedZoom) / 100f;

        while(cam.orthographicSize > _value)
        {
            yield return new WaitForSeconds(speed);

            cam.orthographicSize -= 0.05f;
        }
    }

    private IEnumerator IUnzoom(float _speedZoom, float _value)
    {
        float speed = (1 / _speedZoom) / 100f;

        while (cam.orthographicSize < _value)
        {
            yield return new WaitForSeconds(speed);

            cam.orthographicSize += 0.05f;
        }
    }

    private IEnumerator ITranslate(float _speed, float _xPos, float _yPos)
    {
        float zPos = transform.position.z;
        Vector2 target = new Vector3(_xPos, _yPos, zPos);

        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);

            yield return null;
        }
    }

    public void callIntroTranslation()
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        transform.localPosition = new Vector3(0f, 6.9f, 7.5f);

        currentRot = new Vector3(45f, 180f, 0f);
        currentQuaternionRot.eulerAngles = currentRot;
        transform.localRotation = currentQuaternionRot;

        StartCoroutine(IIntroTranslation());
    }

    private IEnumerator IIntroTranslation()
    {
        int cpt = 0;
        float xRot = 45f;
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        yield return new WaitForSeconds(1f);

        while (cpt < 50)
        {
            yield return new WaitForSeconds(0.02f);

            transform.localPosition = new Vector3(0f, transform.localPosition.y - 0.1f, transform.localPosition.z - 0.1f);

            xRot -= 0.4f;
            currentRot = new Vector3(xRot, 180f, 0f);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;
        }

        gameObject.GetComponent<CameraRotator>().enabled = true;
    }
}