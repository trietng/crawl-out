using System.Collections;using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ServiceScript : MonoBehaviour
{
    public static ServiceScript _instance { get; private set; }
    [SerializeField] Light2D globalLight;
    public float bulletCount;
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this);
        else _instance = this;
        globalLight.intensity = 1;
    }
    public void PlaySound(AudioClip _audiclip)
    {
        GetComponent<AudioSource>().PlayOneShot(_audiclip);
    }
    public IEnumerator TempRemoveCollider(GameObject coll, float sec)
    {
        coll.GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(sec);
        }
        coll.GetComponent<BoxCollider2D>().enabled = true;

    }
    public IEnumerator TurnOffLight()
    {
        for (int i = 0; i < 10; i++ )
        {
            globalLight.intensity -= 0.1f;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void ExitGame()
    {
        Application.Quit(); 
    }
}
