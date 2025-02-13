using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpell : MonoBehaviour
{
    public Spell spell;

    [HideInInspector] public Alpha alpha;

    public bool pushed = false;
    public bool pushedRight = false;
    public bool pushedLeft = false;

    public void Aiming()
    {
        if (!pushed)
        {
            if (alpha.rotationPoint.rotation.z < .2f && alpha.rotationPoint.rotation.z > -.19f)
            {
                //spell is aimed right
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, 0, 0);
                pushedLeft = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.2f && alpha.rotationPoint.rotation.z > -.49f)
            {
                //spell is aimed down right
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, spell.knockbackValue, 0);
                pushedLeft = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.5f && alpha.rotationPoint.rotation.z > -.79f)
            {
                //spell is aimed down
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(0, spell.knockbackValue * 1.5f, 0);
            }
            else if (alpha.rotationPoint.rotation.z < -.8f && alpha.rotationPoint.rotation.z > -.94f)
            {
                //spell is aimed down left
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, spell.knockbackValue, 0);
                pushedRight = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.95f || alpha.rotationPoint.rotation.z > .95f)
            {
                //spell is aimed left
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, 0, 0);
                pushedRight = true;
            }
            else if (alpha.rotationPoint.rotation.z < .94f && alpha.rotationPoint.rotation.z > .81f)
            {
                //spell is aimed up left
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, spell.knockbackValue, 0);
                pushedRight = true;
            }
            else if (alpha.rotationPoint.rotation.z < .8f && alpha.rotationPoint.rotation.z > .5f)
            {
                //spell is aimed up
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(0, -spell.knockbackValue, 0);
            }
            else if (alpha.rotationPoint.rotation.z < .49f && alpha.rotationPoint.rotation.z > .21f)
            {
                //spell is aimed up right
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, -spell.knockbackValue, 0);
                pushedLeft = true;
            }

            pushed = true;
        }
    }
}
