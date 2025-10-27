using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
public class InventorySpell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [HideInInspector] public Image image;

    public Spell spell;
    [HideInInspector] public Transform parentAfterDrag;

    private Vector3 originalPosition;

    //used elsewhere, allows us to easily give spells a sprite image and have that function with the rest of the scripts
    public void InitialiseSpell(Spell newSpell)
    {
        spell = newSpell;
        image.sprite = newSpell.image;
    }

    //functionality for picking up an inventory spell with the mouse
    public void OnBeginDrag(PointerEventData eventData)
    {
        //set parentAfterDrag when dragging starts, only if it hasn't been set already
        if (parentAfterDrag == null)
        {
            parentAfterDrag = transform.parent;
        }

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; //follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag); //reset parent to the original parent
        transform.position = originalPosition; //reset position
        image.raycastTarget = true; //re-enable raycast for interaction
    }

    //force the spell to reset to its original parent and position (when closing inventory)
    public void ResetSpell()
    {
        transform.SetParent(parentAfterDrag); //reset parent
        transform.localPosition = Vector3.zero; //reset position relative to the parent
        image.raycastTarget = true; //ensure raycasting is enabled
    }
}