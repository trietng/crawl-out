using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    GameObject UIScene;
    Text text;
    private void Awake()
    {
        UIScene = GameObject.FindGameObjectWithTag("UI_InScene");
        text = UIScene.GetComponentInChildren<Text>();
    }
    private void Start()
    {
        UIScene.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TextAppear(new List<string>() {"You", " Win" }));
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    IEnumerator TextAppear(List<string> sen)
    {
        UIScene.SetActive(true);
        text.text = "";
        yield return new WaitForSeconds(2);
        for (int i = 0; i < sen.Count; i++)
        {
            text.text += sen[i];
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(2);
        text.enabled = false;
        StartCoroutine(ServiceScript._instance.TurnOnLight());
    }
}
