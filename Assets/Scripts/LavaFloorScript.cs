using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloorScript : MonoBehaviour
{
    private Transform itemTransform;

    private bool lavaEntered = false;

    private int lavaDamage = 10;
    private int lavaLeaveDamage = 5;
    private float lavaDamageTick = 1.0f;

    private float lavaExtensionTime = 3.0f;

    private float lavaLeaveTime = 0;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lavaEntered = true;
            StartCoroutine(LavaDamagingPlayer());
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            lavaEntered = false;
            lavaLeaveTime = Time.time;
        }
    }

    private IEnumerator LavaDamagingPlayer()
    {
        while (lavaEntered || Time.time - lavaLeaveTime <= lavaExtensionTime) {
            PlayerScript.Instance.TakeDamage(lavaEntered ? lavaDamage : lavaLeaveDamage);
            yield return new WaitForSeconds(lavaDamageTick);
        }
    }
}
