using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{

    public Spell spell;

    public GameObject inventory;

    private void OnTriggerEnter(Collider other) {
        if(spell != null) {

            inventory.GetComponent<InventoryManager>().AddSpell(spell);
            Destroy(this.gameObject);

        }
    }

}
