using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;



public class NewMonoBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public string DialogueTag;
    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //gameObject.SetActive(false);
            if (DialogueTag == "Dialogue 0")
            {
                SceneManager.LoadScene("Battle 0");
            }
            else if (DialogueTag == "Dialogue 1")
            {
                SceneManager.LoadScene("Battle 1");
            }
            else if (DialogueTag == "Dialogue P1")
            {
                SceneManager.LoadScene("Battle P1");
            }
            else if (DialogueTag == "Dialogue P2")
            {
                SceneManager.LoadScene("Dialogue P2-2");
            }
            else if (DialogueTag == "Dialogue P2-2")
            {
                SceneManager.LoadScene("Battle P2");
            }
            else if (DialogueTag == "Dialogue P3")
            {
                SceneManager.LoadScene("Dialogue 0");
            }
            else if (DialogueTag == "Victory" || DialogueTag == "Defeat")
            {
                SceneManager.LoadScene("Start");
            }
            else if (DialogueTag == "Tutorial D1")
            {
                SceneManager.LoadScene("Tutorial Battle");
            }
            else if (DialogueTag == "Tutorial D2")
            {
                SceneManager.LoadScene("Start");
            }
        }
    }
}
