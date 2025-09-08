using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpell : MonoBehaviour
{
    public Spell spell;
    public Rigidbody rb;

    [HideInInspector] public Alpha alpha;

    private Vector3 aimingDirection;

    public void Aiming(Vector3 direction)
    {
        aimingDirection = direction;
        rb.velocity = new Vector3(aimingDirection.x, aimingDirection.y, 0) * 20f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //possibly remove this as spell might be used for puzzle solving or movement than for attacking?
        }
        else if (other.gameObject.tag == "Pushable Object")
        {
            //add code that'll push these "pushable objects" in the direction of where the spell is casted by a few units
            if(aimingDirection.x > 0)
            {
                other.gameObject.transform.Translate(new Vector3(spell.knockbackValue, 0, 0));
                Destroy(gameObject, .2f);
            }
            else if(aimingDirection.x < 0)
            {
                other.gameObject.transform.Translate(new Vector3(-spell.knockbackValue, 0, 0));
                Destroy(gameObject, .2f);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
