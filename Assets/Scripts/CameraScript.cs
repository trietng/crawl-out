using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player; 
    Vector3 veloc;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        veloc = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
       transform.position =  Vector3.SmoothDamp(transform.position, player.transform.position, ref veloc, 0.5f);
    }
}
