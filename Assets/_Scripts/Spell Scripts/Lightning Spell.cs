using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : MonoBehaviour
{
    public Spell spell;
    public Transform spellSpawnPoint;
    public Rigidbody rb;

    [HideInInspector] public Alpha alpha;

    private Vector3 aimingDirection;

    public void Aiming()
    {
        GameObject g = Instantiate(alpha.spellAttack2, alpha.spellSpawn.position, alpha.spellSpawn.rotation);
        Rigidbody rg = g.GetComponent<Rigidbody>();
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        rg.velocity = new Vector3(aimingDirection.x, aimingDirection.y, 0) * 20f;
        Destroy(g, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            
        }
        else if(other.gameObject.tag == "Destructible Wall")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
