using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueBoxes : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI characterName1;
    public TextMeshProUGUI characterName2;
    public GameObject dialogueBox;
    public DBHolder DBHolder;
    public string[] lines;
    public int[] switchFocusAtLine;
    public float textSpeed;

    public Image talkingCharacter1;
    public Image talkingCharacter2;

    private int characterSpeaking = 1;
    private int counter = 0;
    private int index;

    private void OnEnable()
    {
        text.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (text.text == lines[index])
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

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {

        if (counter < switchFocusAtLine.Length)
        {
            if (switchFocusAtLine[counter] == index)
            {
                //if (counter < cutsceneArt.Length)
                //{
                //    SwitchSpeaker();
                //}
                SwitchSpeaker();
                counter++;
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
            StartCoroutine(TypeLine());
        }
        else
        {
            DBHolder.GoToNextDialogue();
            dialogueBox.SetActive(false);
        }
    }

    private void SwitchSpeaker()
    {
        if(characterSpeaking == 1)
        {
            characterSpeaking = 2;
            talkingCharacter1.color = Color.gray;
            talkingCharacter2.color = Color.white;
        }
        else if(characterSpeaking == 2)
        {
            characterSpeaking = 1;
            talkingCharacter2.color = Color.gray;
            talkingCharacter1.color = Color.white;
        }
    }
}
