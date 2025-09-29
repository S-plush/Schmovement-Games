using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics.Tracing;
using UnityEngine.SceneManagement;

public class CutsceneText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;

    public Image showCutsceneArt;
    public Sprite[] cutsceneArt;
    public int[] switchArtAtLine;
    private int counter = 0;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        text.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
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
        Debug.Log(counter + " before");

        
        if (counter < switchArtAtLine.Length)
        {
            if (switchArtAtLine[counter] == index)
            {
                if (counter < cutsceneArt.Length)
                {
                    SwitchCutsceneArt(counter);
                }

                counter++;
                Debug.Log(counter + "after");
            }
        }

        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            Debug.Log(index);
            StartCoroutine (TypeLine());
        }
        else
        {
            SceneManager.LoadScene("DetentionCenter");
        }
    }

    private void SwitchCutsceneArt(int art)
    {
        showCutsceneArt.sprite = cutsceneArt[art];
    }
}
