using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;

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
    public Vector3 aimingDirection;
    public float timer; //for spell

    private float lastShot; //cooldown for the spell 1
    private bool isGrounded;
    private Rigidbody alpha;
    private BoxCollider boxCollider;
    private ExplosionSpell explosion;
    private ShootingSpell shootingSpell;
    
    [HideInInspector] public bool isMovingLeft = false;
    [HideInInspector] public bool isMovingRight = false;

    public GameObject Inventory;

    public GameObject HUD;

    public HealthBar healthBar;
    public int maxHealth;
    private int currentHealth;

    public ManaBar manaBar;
    public int maxMana;
    private int currentMana;

    public TMP_Text stimCountText;
    public int stimCount;
    public int manaFromStim;
    public int healthFromStim;

    void Start()
    {
        alpha = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        explosion = spellAttack.GetComponent<ExplosionSpell>();

        Inventory.SetActive(false);
        HUD.SetActive(true);
        stimCount = 30; /////////////////////////////////input from file later
        stimCountText.text = stimCount + "\n\nStims";
        healthFromStim = 3; ////////////////////////////input from file later
        manaFromStim = 1; //////////////////////////////input from file later

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
            if (currentMana > 0) //check if out of mana
            {
                if (Time.timeScale != 0.0f) //check if inventory is open
                {
                    ShootSpell1();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentMana > 0) //check if out of mana
            {
                if (Time.timeScale != 0.0f) //check if inventory is open
                {
                    //Debug.Log("I'm being pressed");
                    ShootSpell2();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) //open inventory keybind
        {
            OpenMenu();
        }

        if (Input.GetKeyDown(KeyCode.Q)) //use of stim keybind
        {
            UseStim();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //switchToLoadout1
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //switchToLoadout2
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //switchToLoadout3
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //switchToLoadout4
        }

        //this is for the FixedUpdate to help get rid of the jitteriness
        isMovingLeft = Input.GetKey(KeyCode.A);
        isMovingRight = Input.GetKey(KeyCode.D);

        //Debug.Log("fall speed is " + fallSpd);

        //isGrounded makes it so the player isn't able to spam the jump button while in mid-air
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            alpha.AddForce(Vector3.up * jumpSpd);
            isGrounded = false;
            float tempFallSpd = fallSpd;
            fallSpd = 0.5f; //this is to sort of help reset the jump
            //Debug.Log("fall speed is now " + fallSpd);
            fallSpd = tempFallSpd;
        }
    }

    //to help get rid of the jitteriness
    private void FixedUpdate()
    {
        if (isMovingLeft)
        {
            if (explosion.pushed && explosion.pushedRight)
            {
                alpha.velocity = Vector3.zero;
                explosion.pushed = false;
                explosion.pushedRight = false;
            }

            this.gameObject.transform.Translate(new Vector3(0, 0, -alphaMovementSpd * Time.deltaTime));
        }
        else if(isMovingRight)
        {
            if (explosion.pushed && explosion.pushedLeft)
            {
                alpha.velocity = Vector3.zero;
                explosion.pushed = false;
                explosion.pushedLeft = false;
            }

            this.gameObject.transform.Translate(new Vector3(0, 0, alphaMovementSpd * Time.deltaTime));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OpenMenu()
    {
        if (!Inventory.activeInHierarchy)
        {
            //HUD.SetActive(false);
            Inventory.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Inventory.activeInHierarchy)
        {
            //HUD.SetActive(true);
            Inventory.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    void ShootSpell1()
    {
        if (activeSpell.activeInHierarchy)
        {
            if (Time.time - lastShot < timer)
            {
                return;
            }

            useMana(1);

            //rn this is for the explosion spell
            explosion.pushed = false;
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
            aimingDirection = FindObjectOfType<Aiming>().AimDirection();
            Rigidbody rg = g.GetComponent<Rigidbody>();
            rg.velocity = new Vector3(aimingDirection.x, aimingDirection.y, 0) * 20f;
            //shootingSpell.Aiming();
            lastShot = Time.time;
            Destroy(g, 1f);

        }
    }

    void UseStim()
    {
        if (Time.timeScale != 0.0f)
        {
            if (stimCount > 0)
            {
                stimCount -= 1;
                stimCountText.text = stimCount + "\n\nStims";

                if (currentHealth + healthFromStim > maxHealth) //if health exceeds max health condition
                {
                    currentHealth = maxHealth;
                    healthBar.SetHealth(maxHealth);
                }
                else
                {
                    currentHealth += healthFromStim;
                    healthBar.SetHealth(currentHealth);
                }

                if (currentMana + manaFromStim > maxMana) //if mana exceeds max mana condition
                {
                    currentMana = maxMana;
                    manaBar.SetMana(maxMana);
                }
                else
                {
                    currentMana += manaFromStim;
                    manaBar.SetMana(currentMana);
                }
            }
            else
            {
                // play empty (out of stims) sound and flash red
            }
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
