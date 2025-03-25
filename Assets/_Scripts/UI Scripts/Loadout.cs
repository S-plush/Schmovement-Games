using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Loadout : MonoBehaviour
{

    private Image child; //holds ref of child object

    public Image HUDSlot1; //ref to the left spell slot on HUD
    public Image HUDSlot2; //ref to the right spell slot on HUD

    public int whichSlotIsThis; //1 or 2 depending on if this is the first or second slot of a given loadout
    public int whichLoudoutSetIsThis; //1-4 depending on which loadout this slot is from

    [HideInInspector] public int currentLoadoutSelected; //needs to be updated in Alpha Script when player presses keys to swap loadouts !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public Sprite defaultBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    void Update()
    {
        
        //gives the selected loadout sprites to the UI boxes that show which spells are equipped and ready to shoot
        if (whichLoudoutSetIsThis == currentLoadoutSelected)
        {
            if (this.transform.childCount > 0)
            {
                child = this.transform.GetChild(0).gameObject.GetComponent<Image>();

                if (whichSlotIsThis == 1)
                {
                    HUDSlot1.sprite = child.sprite;
                }
                else
                {
                    HUDSlot2.sprite = child.sprite;
                }
            }
            else
            {
                if (whichSlotIsThis == 1)
                {
                    HUDSlot1.sprite = defaultBox;
                }
                else
                {
                    HUDSlot2.sprite = defaultBox;
                }
            }
        }
    }
    
}
