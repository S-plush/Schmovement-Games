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

    [SerializeField] private Animator transition;
    [SerializeField] private Animator dialogueBoxTransition;
    [SerializeField] private Animator dialogueTextTransition;

    private int index;
    private bool preventInput = false;
    private bool begining = true;

    // Start is called before the first frame update
    void Start()
    {
        text.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !preventInput)
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
                    preventInput = true;

                    if (!begining)
                    {
                        transition.Play("Fade Out");
                    }

                    yield return new WaitForSeconds(1f);
                    SwitchCutsceneArt(counter);
                    transition.Play("Fade In");
                    yield return new WaitForSeconds(.5f);
                    begining = false;
                    preventInput = false;
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
            MiscDataToFile.newGame = true;
            StartCoroutine(SwitchScene());
        }
    }

    private void SwitchCutsceneArt(int art)
    {
        showCutsceneArt.sprite = cutsceneArt[art];
    }

    IEnumerator SwitchScene()
    {
        transition.Play("Fade Out");
        dialogueBoxTransition.Play("Fade Out Box");
        dialogueTextTransition.Play("Fade Out Text");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("DetentionCenter");
    }
}
