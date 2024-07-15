using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public bool isPickedUp = false;
    private Vector3 vel;
    public float smoothTime = 0.3f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isPickedUp = false;
    }

    void Update()
    {
        if (isPickedUp && player != null)
        {
            Vector3 offset = new Vector3(0, 1, 0);
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
        }
    }
}
