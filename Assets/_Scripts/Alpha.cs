using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Alpha : MonoBehaviour
{
    public float alphaMovementSpd = 3.5f;
    public float jumpSpd = 5f;
    public float fallSpd = 2.5f;

    //set up for this test build, but will need to have an abstract class for all the spells
    public Transform spellSpawn; //spawn point for spell's attack
    public GameObject spellAttack; //for the spell effect/attack prefab
    public GameObject activeSpell1; //for rn the spell's spawnpoint is what's used for this
    public GameObject activeSpell2; //for rn the spell's spawnpoint is what's used for this
    public Transform rotationPoint; 
    public float timer;

    private float lastShot; //cooldown for the spell 1
    private bool isGrounded;
    private Rigidbody alpha;
    private BoxCollider boxCollider;
    private Aiming aiming;

    public GameObject Inventory;

    void Start()
    {
        alpha = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        Inventory.SetActive(false);
    }

    private void FixedUpdate()
    {
        //Debug.Log("rotation is " + rotationPoint.rotation.z);

        //fixed by removing unnessisary lines of code

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootSpell1();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ShootSpell2();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenMenu();
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(alphaMovementSpd * Time.deltaTime, 0, 0));

        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-alphaMovementSpd * Time.deltaTime, 0, 0));

        }

        //Debug.Log("fall speed is " + fallSpd);

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

    void OpenMenu()
    {
        if (!Inventory.activeInHierarchy)
        {
            Inventory.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Inventory.activeInHierarchy)
        {
            Inventory.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    void ShootSpell1()
    {
        if(activeSpell1.activeInHierarchy)
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

    void ShootSpell2()
    {

    }
}
