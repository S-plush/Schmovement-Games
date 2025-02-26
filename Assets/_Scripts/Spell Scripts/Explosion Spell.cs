using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpell : MonoBehaviour
{
    public Spell spell;

    [HideInInspector] public Alpha alpha;

    public bool pushed = false;
    public bool preventMoving = false;
    public bool pushedRight = false;
    public bool pushedLeft = false;

    private Coroutine coroutine;

    public void Aiming()
    {
        if (!pushed)
        {
            if (alpha.rotationPoint.rotation.z < .2f && alpha.rotationPoint.rotation.z > -.19f)
            {
                //spell is aimed right
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, 0, 0);
                pushedLeft = true;
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.2f && alpha.rotationPoint.rotation.z > -.49f)
            {
                //spell is aimed down right
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, spell.knockbackValue, 0);
                pushedLeft = true;
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.5f && alpha.rotationPoint.rotation.z > -.79f)
            {
                //spell is aimed down
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(0, spell.knockbackValue * 1.5f, 0);
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.8f && alpha.rotationPoint.rotation.z > -.94f)
            {
                //spell is aimed down left
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, spell.knockbackValue, 0);
                pushedRight = true;
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < -.95f || alpha.rotationPoint.rotation.z > .95f)
            {
                //spell is aimed left
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, 0, 0);
                pushedRight = true;
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < .94f && alpha.rotationPoint.rotation.z > .81f)
            {
                //spell is aimed up left
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(spell.knockbackValue, spell.knockbackValue, 0);
                pushedRight = true;
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < .8f && alpha.rotationPoint.rotation.z > .5f)
            {
                //spell is aimed up
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(0, -spell.knockbackValue, 0);
                pushed = true;
            }
            else if (alpha.rotationPoint.rotation.z < .49f && alpha.rotationPoint.rotation.z > .21f)
            {
                //spell is aimed up right
                alpha.GetComponent<Rigidbody>().velocity = new Vector3(-spell.knockbackValue, -spell.knockbackValue, 0);
                pushedLeft = true;
                pushed = true;
            }

            if (coroutine != null)
            {
                alpha.StopCoroutine(coroutine);
            }

            coroutine = alpha.StartCoroutine(ResetMovement());
        }
    }

    private IEnumerator ResetMovement()
    {
        //Debug.Log("working");
        preventMoving = true;
        yield return new WaitForSeconds(0.3f);
        preventMoving = false;

        while(!((alpha.isMovingLeft && pushedRight) || (alpha.isMovingRight && pushedLeft)) && pushed)
        {
            //Debug.Log("yes?");
            yield return null;
        }

        if(pushed)
        {
            pushed = false;
            alpha.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
