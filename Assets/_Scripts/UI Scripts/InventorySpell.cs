using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySpell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [HideInInspector] public Image image;

    public Spell spell;
    [HideInInspector] public Transform parentAfterDrag;

    //used elsewhere, allows us to easily give spells a sprite image and have that function with the rest of the scripts
    public void InitialiseSpell(Spell newSpell)
    {
        spell = newSpell;
        image.sprite = newSpell.image;
    }

    //functionality for picking up an inventory spell with the mouse
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    //functionality for dragging an inventory spell with the mouse
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    //functionality for dropping/snapping an inventory spell with the mouse
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}