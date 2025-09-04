using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpell : MonoBehaviour
{
    //this spell while doing some damage, will mainly sprout an angled platform for the player to walk on. Only usable while on the ground
    //Maybe also have this spell destroy walls

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

        }
        else if (other.gameObject.tag == "Destructible Wall")
        {
            //possibly have this spell be able to destroy walls as well
            //Destroy(other.gameObject);
            //Destroy(gameObject);
        }
    }
}
