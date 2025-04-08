using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PickupScript : MonoBehaviour
{

    public Spell spell;

    public string pickupType; //maxStimIncrease, maxHealthIncrease, maxManaIncrease, 

    private GameObject inventory; //reference to the InventoryManager

    Alpha AlphaScript; //reference to the Alpha Script on the Player

    void Start()
    {
        AlphaScript = FindObjectOfType<Alpha>(); //initilize AlphaScript with the actual script

        inventory = FindObjectOfType<InventoryManager>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (spell != null)
        {

            inventory.GetComponent<InventoryManager>().AddSpell(spell);
            inventory.GetComponent<InvDataBetweenRuns>().SaveInventory();
            inventory.GetComponent<LoadoutsToFile>().saveLoadoutsToFile();
        }

        if (pickupType == "maxStimIncrease")
        {
            AlphaScript.maxStims += 1;
            AlphaScript.stimCount = AlphaScript.maxStims;
            AlphaScript.stimCountText.text = AlphaScript.stimCount + "\n\nStims";
        }
        else if (pickupType == "maxHealthIncrease")
        {
            AlphaScript.maxHealth += 1;
            AlphaScript.currentHealth = AlphaScript.maxHealth;
            AlphaScript.healthBar.SetMaxHealth(AlphaScript.maxHealth);
            AlphaScript.healthBar.SetHealth(AlphaScript.currentHealth);


        }
        else if (pickupType == "maxManaIncrease")
        {
            AlphaScript.maxMana += 1;
            AlphaScript.currentMana = AlphaScript.maxMana;
            AlphaScript.manaBar.SetMaxMana(AlphaScript.maxMana);
            AlphaScript.manaBar.SetMana(AlphaScript.currentMana);
        }

        Destroy(this.gameObject);
    }
}