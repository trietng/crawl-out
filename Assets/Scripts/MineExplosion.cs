using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    private Transform itemTransform;

    private bool destroyed = false;

    [SerializeField] int mineDamage = 80;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destroyed && collision.CompareTag("Player"))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        destroyed = true;
        PlayerScript.Instance.TakeDamage(mineDamage);
    }
}
