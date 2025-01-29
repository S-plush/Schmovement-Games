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
    public GameObject spellAttack2; //for the spell2 effect/attack prefab 
    public GameObject activeSpell; //for rn the spell's spawnpoint is what's used for this
    public Transform rotationPoint; 
    public float timer; //for spell

    private float lastShot; //cooldown for the spell 1
    private bool isGrounded;
    private Rigidbody alpha;
    private BoxCollider boxCollider;
    private ExplosionSpell explosion;
    //private Aiming aiming; //might not need this here

    public GameObject Inventory;

    public HealthBar healthBar;
    public int maxHealth;
    private int currentHealth;

    public ManaBar manaBar;
    public int maxMana;
    private int currentMana;

    void Start()
    {
        alpha = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        explosion = spellAttack.GetComponent<ExplosionSpell>();
        Inventory.SetActive(false);

        maxHealth = 5; /////////////////////////////////input from file later
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxMana = 5; ///////////////////////////////////input from file later
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    private void Update()
    {
        //Debug.Log("rotation is " + rotationPoint.rotation.z);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootSpell1();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("I'm being pressed");
            ShootSpell2();
            TakeDamage(1); //////////////////////////////////////////////////////remove this line
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
        if(activeSpell.activeInHierarchy)
        {
            if(Time.time - lastShot < timer)
            {
                return;
            }
            useMana(1);
            //rn this is for the explosion spell
            explosion.alpha = this; //for some reason I can't put the player onto the explosion object so this is a supplement for that
            explosion.Aiming();
            GameObject g = Instantiate(spellAttack, spellSpawn.position, spellSpawn.rotation);
            lastShot = Time.time;
            Destroy(g, 0.5f);
        }
    }

    void ShootSpell2()
    {
        if (activeSpell.activeInHierarchy)
        {
            if (Time.time - lastShot < timer)
            {
                return;
            }
            Debug.Log("I'm in...");
            GameObject g = Instantiate(spellAttack2, spellSpawn.position, spellSpawn.rotation);

            //g.GetComponent<Rigidbody>().velocity = new Vector3(rotationPoint.rotation.z, 0, 0) * 20f;

            //if (rotationPoint.rotation.z < .2f && rotationPoint.rotation.z > -.19f)
            //{
            //    //spell goes right
            //    g.GetComponent<Rigidbody>().velocity = Vector3.right * 20f;
            //}
            //else if (rotationPoint.rotation.z < -.2f && rotationPoint.rotation.z > -.49f)
            //{
            //    //spell goes down right
            //    g.GetComponent<Rigidbody>().velocity = new Vector3(1f, -1f, 0) * 20f;
            //}
            //else if (rotationPoint.rotation.z < -.5f && rotationPoint.rotation.z > -.79f)
            //{
            //    //spell goes down
            //    g.GetComponent<Rigidbody>().velocity = Vector3.down * 20f;
            //}
            //else if (rotationPoint.rotation.z < -.8f && rotationPoint.rotation.z > -.94f)
            //{
            //    //spell goes down left
            //    g.GetComponent<Rigidbody>().velocity = new Vector3(-1f, -1f, 0) * 20f;
            //}
            //else if (rotationPoint.rotation.z < -.95f || rotationPoint.rotation.z > .95f)
            //{
            //    //spell goes left
            //    g.GetComponent<Rigidbody>().velocity = Vector3.left * 20f;
            //}
            //else if (rotationPoint.rotation.z < .94f && rotationPoint.rotation.z > .81f)
            //{
            //    //spell goes up left
            //    g.GetComponent<Rigidbody>().velocity = new Vector3(-1f, 1f, 0) * 20f;
            //}
            //else if (rotationPoint.rotation.z < .8f && rotationPoint.rotation.z > .5f)
            //{
            //    //spell goes up
            //    g.GetComponent<Rigidbody>().velocity = Vector3.up * 20f;
            //}
            //else if (rotationPoint.rotation.z < .49f && rotationPoint.rotation.z > .21f)
            //{
            //    //spell goes up right
            //    g.GetComponent<Rigidbody>().velocity = new Vector3(1f, 1f, 0) * 20f;
            //}

            lastShot = Time.time;
            Destroy(g, 1f);

        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void useMana(int lostMana)
    {
        currentMana -= lostMana;

        manaBar.SetMana(currentMana);
    }
}
