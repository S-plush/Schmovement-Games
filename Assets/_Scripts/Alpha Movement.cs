using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlphaMovement : MonoBehaviour
{
    public float alphaMovementSpd = 3.5f;
    public float jumpSpd = 5f;
    public float fallSpd = 2.5f;
    public float lowJumpSpd = 2f;

    //set up for this test build, but will need to have an abstract class for all the spells
    public Transform spellSpawn; //spawn point for spell's attack
    public GameObject spellAttack; //for the spell effect/attack prefab
    public GameObject activeSpell; //for rn the spell's spawnpoint is what's used for this
    public Transform rotationPoint;
    public float timer;

    private float lastShot; //cooldown for the spell
    private bool isGrounded;
    private Rigidbody alpha;
    private BoxCollider boxCollider;
    private Aiming aiming;
    //private Vector3 horizontalMovement;

    void Start()
    {
        alpha = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Debug.Log("rotation is " + rotationPoint.rotation.z);

        //probably a better way to have the player shoot while moving than what I have it set up rn

        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(alphaMovementSpd * Time.deltaTime, 0, 0));

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-alphaMovementSpd * Time.deltaTime, 0, 0));

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        Debug.Log("fall speed is " + fallSpd);//this is to sort of help reset the jump

        //isGrounded makes it so the player isn't able to spam the jump button while in mid-air
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //by changing jumpSpd and fallSpd, we should be able to change jump physics
            //to make it feel right

            alpha.AddForce(Vector3.up * jumpSpd);

            if (alpha.velocity.y > 0)
            {
                Debug.Log("i'm working!");
                alpha.velocity += Vector3.up * Physics.gravity.y * (fallSpd - 1) * Time.deltaTime;
            }

            isGrounded = false;
            float tempFallSpd = fallSpd;
            fallSpd = 0.5f; //this is to sort of help reset the jump
            Debug.Log("fall speed is now " + fallSpd);
            fallSpd = tempFallSpd;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void Shoot()
    {
        if(activeSpell.activeInHierarchy)
        {
            if(Time.time - lastShot < timer)
            {
                return;
            }

            if(rotationPoint.rotation.z < .2f && rotationPoint.rotation.z > -.19f)
            {
                alpha.velocity = new Vector3(-3f, 0, 0);
            }
            else if(rotationPoint.rotation.z < -.2f && rotationPoint.rotation.z > -.49f)
            {
                alpha.velocity = new Vector3(-3f, 3f, 0);
            }
            else if (rotationPoint.rotation.z < -.5f && rotationPoint.rotation.z > -.79f)
            {
                alpha.velocity = new Vector3(0, 3f, 0);
            }
            else if (rotationPoint.rotation.z < -.8f && rotationPoint.rotation.z > -.94f)
            {
                alpha.velocity = new Vector3(3f, 3f, 0);
            }
            else if (rotationPoint.rotation.z < -.95f || rotationPoint.rotation.z > .95f)
            {
                alpha.velocity = new Vector3(3f, 0, 0);
            }
            else if (rotationPoint.rotation.z < .94f && rotationPoint.rotation.z > .81f)
            {
                alpha.velocity = new Vector3(3f, -3f, 0);
            }
            else if (rotationPoint.rotation.z < .8f && rotationPoint.rotation.z > .5f)
            {
                alpha.velocity = new Vector3(0, -3f, 0);
            }
            else if (rotationPoint.rotation.z < .49f && rotationPoint.rotation.z > .21f)
            {
                alpha.velocity = new Vector3(-3f, -3f, 0);
            }

            lastShot = Time.time;
            GameObject g = Instantiate(spellAttack, spellSpawn.position, spellSpawn.rotation);
            Destroy(g, 0.5f);
        }
    }
}
