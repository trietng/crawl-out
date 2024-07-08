using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashLightScript : MonoBehaviour
{
    [NonSerialized] public static Camera mainCam;
    [SerializeField] float rotateSpeed;
    Vector2 currMousePoint;
    [SerializeField] float alignAngle;
    private Light2D light2D;

    void Awake()
    {
        mainCam = Camera.main;
        light2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            light2D.enabled = !light2D.enabled;
        }

        currMousePoint = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position ;

        float angle = Mathf.Atan2(currMousePoint.y, currMousePoint.x) * Mathf.Rad2Deg  + alignAngle;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}
