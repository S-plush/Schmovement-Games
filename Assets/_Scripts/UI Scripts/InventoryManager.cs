using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Spell[] startSpells; //spells player will have when game loads

    public InventorySlot[] inventorySlots;
    public GameObject inventorySpellPrefab;

    int selectedSlot = 1;

    public GameObject Settings;

    //auxilary code/functionality for cursor selecting slots
    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();

            inventorySlots[newValue].Select();
            selectedSlot = newValue;
        }
    }

    //for putting a new spell in the next availible inventory slot when it is picked up
    public void AddSpell(Spell spell)
    {
        bool freeSlotFound = false;
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventorySpell spellInSlot = slot.GetComponentInChildren<InventorySpell>();
            if(spellInSlot == null && freeSlotFound == false)
            {
                SpawnNewSpell(spell, slot);
                freeSlotFound = true;
            }
        }
    }

    //making a new spell from the spell prefab
    public void SpawnNewSpell(Spell spell, InventorySlot slot)
    {
        //InventorySpell inventorySpell = Instantiate(inventorySpellPrefab, slot.transform).GetComponent<InventorySpell>();
        
        GameObject newSpellGo = Instantiate(inventorySpellPrefab, slot.transform);
        InventorySpell inventorySpell = newSpellGo.GetComponent<InventorySpell>();
        inventorySpell.transform.SetParent(slot.transform);
        inventorySpell.InitialiseSpell(spell);

        
    }

    //functionality for adding new spells to players inventory (elsewhere puts it in the next available slot)
    public Spell GetSelectedSpell()
    {
        InventorySlot slot = inventorySlots[(selectedSlot)];
        InventorySpell spellInSlot = slot.GetComponentInChildren<InventorySpell>();
        if (spellInSlot != null)
        {
            return spellInSlot.spell;
        }

        return null;
    }

    public void SettingsResumeButton()
    {
        Settings.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
