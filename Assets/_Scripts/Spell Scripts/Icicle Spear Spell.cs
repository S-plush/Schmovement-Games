using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpearSpell : MonoBehaviour
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
            spell.damageValue++;

        }
        else
        {
            spell.damageValue = 1;
            Destroy(gameObject);
        }
    }
}
