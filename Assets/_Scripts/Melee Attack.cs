using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [HideInInspector] public Alpha alpha;
    private Vector3 aimingDirection;

    private void Start()
    {
        alpha = FindObjectOfType<Alpha>();
    }

    public void Aiming(Vector3  direction)
    {
        aimingDirection = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if (alpha.currentMana < alpha.maxMana)
            {
                alpha.currentMana += 1;
                alpha.manaBar.SetMana(alpha.currentMana);
            }
        }
    }
}
