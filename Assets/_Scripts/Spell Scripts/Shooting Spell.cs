using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we can rename this script to an appropriate spell name later
public class ShootingSpell : MonoBehaviour
{
    public Spell spell;
    public GameObject spellObject;
    public Transform spellSpawnPoint;

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
}
