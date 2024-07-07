using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{

    Text title;
    List<string> listInTitle = new() { "Crawl", " Out" };
    AudioSource audi;
    [SerializeField] AudioClip bulletDropClip;
    [SerializeField] AudioClip fireClip;
    void Awake()
    {
        title = GetComponent<Text>();
        audi = GetComponent<AudioSource>();
    }
    private void Start()
    {
        StartCoroutine(TextAppear(listInTitle));
        StartCoroutine(GameManager.Instance.TurnOffLight());
    }
 
        IEnumerator TextAppear(List<string> sen)
    {
        title.text = "";
        audi.PlayOneShot(bulletDropClip);
        yield return new WaitForSeconds(3);
        for (int i = 0; i < sen.Count; i++)
        {
            audi.PlayOneShot(fireClip);
            title.text += sen[i];
            yield return new WaitForSeconds(1);
        }                      
    }                                                               
}                         
                                