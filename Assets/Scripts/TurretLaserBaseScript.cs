using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretLaserBaseScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;

    private bool destroyed = false;

	public void passAnimator (Animator animator) {
		anim = animator;
	}

	public bool isDestroyed() {
		return destroyed;
	}

    public void DisableAttack() {
        destroyed = true;
        if (anim != null) anim.SetBool("Alive", false);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t != transform)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}
