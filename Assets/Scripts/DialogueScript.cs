using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI nameTextComponent;
    public TextMeshProUGUI contentTextComponent;
    public string[] lines;
    public float textSpeed = 15f;

    private int index = 0;
    
    void Start()
    {
        nameTextComponent.text = string.Empty;
        contentTextComponent.text = string.Empty;
        gameObject.SetActive(false);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && gameObject.activeSelf == true)
        {
            if (contentTextComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                contentTextComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(string name)
    {
        index = 0;
        gameObject.SetActive(true);
        nameTextComponent.text = name;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            contentTextComponent.text += c;
            yield return new WaitForSeconds(1/textSpeed);
        }
    }

    void NextLine()
    {
        contentTextComponent.text = string.Empty;

        if (index < lines.Length-1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
