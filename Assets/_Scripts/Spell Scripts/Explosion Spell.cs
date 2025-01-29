using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpell : MonoBehaviour
{
    //public Transform rotationAimingPoint;
    public Spell spell;

    [HideInInspector]
    public Alpha alpha;

    public void Aiming()
    {
        spell.knockbackValue = 3f;

        if (alpha.rotationPoint.rotation.z < .2f && alpha.rotationPoint.rotation.z > -.19f)
        {
            //spell goes right
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, 0, 0);
        }
        else if (alpha.rotationPoint.rotation.z < -.2f && alpha.rotationPoint.rotation.z > -.49f)
        {
            //spell goes down right
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, spell.knockbackValue, 0);
        }
        else if (alpha.rotationPoint.rotation.z < -.5f && alpha.rotationPoint.rotation.z > -.79f)
        {
            //spell goes down
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(0, spell.knockbackValue, 0);
        }
        else if (alpha.rotationPoint.rotation.z < -.8f && alpha.rotationPoint.rotation.z > -.94f)
        {
            //spell goes down left
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, spell.knockbackValue, 0);
        }
        else if (alpha.rotationPoint.rotation.z < -.95f || alpha.rotationPoint.rotation.z > .95f)
        {
            //spell goes left
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, 0, 0);
        }
        else if (alpha.rotationPoint.rotation.z < .94f && alpha.rotationPoint.rotation.z > .81f)
        {
            //spell goes up left
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, spell.knockbackValue, 0);
        }
        else if (alpha.rotationPoint.rotation.z < .8f && alpha.rotationPoint.rotation.z > .5f)
        {
            //spell goes up
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(0, -spell.knockbackValue, 0);
        }
        else if (alpha.rotationPoint.rotation.z < .49f && alpha.rotationPoint.rotation.z > .21f)
        {
            //spell goes up right
            alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, -spell.knockbackValue, 0);
        }
    }
}
