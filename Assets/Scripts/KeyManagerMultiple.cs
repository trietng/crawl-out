using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManagerMultiple : MonoBehaviour
{
    public static int collectedKeys = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collectedKeys++;
            gameObject.SetActive(false);
        }
    }
}
