using UnityEngine;
using System.Collections;
using TMPro;

public class MainSceneManager : MonoBehaviour
{

    public GameObject dialouge;

    bool cutscene = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
{
    if (!cutscene)
    {
        int isStart = PlayerPrefs.GetInt("isStarting", 0);

        if (isStart == 0)
        {
            PlayerPrefs.SetInt("isStarting", 1);
            StartCoroutine(StartDialogue());
        }
    }
}

IEnumerator StartDialogue()
{
    dialouge.GetComponent<TMP_Text>().text = "Which Planet am I even on?";
    yield return new WaitForSeconds(2f);

    dialouge.GetComponent<TMP_Text>().text = "";
    yield return new WaitForSeconds(0.2f);

    dialouge.GetComponent<TMP_Text>().text = "That crash was brutal, but the spaceship is fine.";
    yield return new WaitForSeconds(2f);

    dialouge.GetComponent<TMP_Text>().text = "";
    yield return new WaitForSeconds(0.2f);

    dialouge.GetComponent<TMP_Text>().text = "I need to check inside.";
    yield return new WaitForSeconds(2f);

    dialouge.GetComponent<TMP_Text>().text = "";
    cutscene = true;
}
}
