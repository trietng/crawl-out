using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnterScript : MonoBehaviour
{
    private Transform itemTransform;

    private bool spikeEntered = false;

    private int spikeDamage = 10;
    private float spikeDamageTick = 1.0f;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spikeEntered = true;
            StartCoroutine(SpikeDamagingPlayer());
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        spikeEntered = false;
    }

    private IEnumerator SpikeDamagingPlayer()
    {
        while (spikeEntered) {
            PlayerScript.Instance.TakeDamage(spikeDamage);
            yield return new WaitForSeconds(spikeDamageTick);
        }
    }
}
