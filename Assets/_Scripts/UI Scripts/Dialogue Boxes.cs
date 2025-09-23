using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DialogueBoxes : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI characterName1;
    public TextMeshProUGUI characterName2;
    public GameObject dialogueBox;
    public string[] lines;
    public float textSpeed;

    public Image talkingCharacter1;
    public Image talkingCharacter2;
    //public Sprite character1Art;
    //public Sprite character2Art;

    private int[] switchFocusAtLine;
    private int characterSpeaking = 1;
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
        Debug.Log(counter + " before");


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
            StartCoroutine(TypeLine());
        }
        else
        {
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
