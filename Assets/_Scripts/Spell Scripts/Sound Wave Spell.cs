using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveSpell : MonoBehaviour
{
    public Spell spell;
    public Rigidbody rb;

    [HideInInspector] public Alpha alpha;

    private int bounce = 3;
    private Vector3 aimingDirection;

    public void Aiming(Vector3 direction)
    {
        aimingDirection = direction;
        rb.velocity = new Vector3(aimingDirection.x, aimingDirection.y, 0) * 20f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("starting bounce " + bounce);
        bounce--;
        spell.damageValue++;
        //Debug.Log(bounce);

        if (bounce < 0)
        {
            Destroy(gameObject);
            spell.damageValue = 1;
            return;
        }

        var contact = collision.contacts[0];
        Vector3 newVelocity = Vector3.Reflect(aimingDirection.normalized, contact.normal);
        Aiming(newVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

        }
        else if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
