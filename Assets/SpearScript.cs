using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour
{
    [SerializeField] float spearTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBound"))
        {
            StartCoroutine(ThrowSpear(collision.gameObject));
        }
    }
    IEnumerator ThrowSpear(GameObject obj)
    {
        GetComponent<Animator>().SetTrigger("trap_trigger");
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(spearTime);
        }
        obj.transform.parent.GetComponent<PlayerScript>().PlayerDead();
    }
}
