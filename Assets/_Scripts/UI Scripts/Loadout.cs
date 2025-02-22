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
        //now need to add more statements to check which loudout is active with whichLoudoutSetIsThis, so it knows which to pull from... this will have to be saved some other way though,
        //as currently this gameObject is disabled during gameplay and it is unknown as to which loadout the player has selected (add 1-4 keys in player script)
        //and other issues since writing to a file about implemetation here...
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
