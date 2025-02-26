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
    public float dashPush = 5;

    //set up for this test build, but will need to have an abstract class for all the spells
    public Transform spellSpawn; //spawn point for spell's attack
    public GameObject spellAttack; //for the explosion spell effect/attack prefab
    public GameObject spellAttack2; //for the lightning spell effect/attack prefab 
    public GameObject activeSpell; //for rn the spell's spawnpoint is what's used for this
    public Transform rotationPoint;
    public Vector3 aimingDirection;
    public float timer; //for spell

    private float lastShot; //cooldown for the spell 1
    private bool hasDashed = false;
    private bool canDoubleJump = false;
    private Rigidbody alpha;
    private BoxCollider boxCollider;

    private ExplosionSpell explosion;
    private LightningSpell lightning;
    public IcicleSpearSpell iciclePrefab;
    public SoundWaveSpell soundWavePrefab;
    
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

    public GameObject InventoryManager;

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy Attack")
        {
            TakeDamage(1);
        }
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

        if (Input.GetKeyDown(KeyCode.I)) //open inventory keybind (also saves spells that are in loadout slots when inventory is opened/closed)
        {
            OpenMenu();

            InventoryManager.GetComponent<LoadoutsToFile>().saveLoadoutsToFile();
        }

        if (Input.GetKeyDown(KeyCode.Q)) //use of stim keybind
        {
            UseStim();
        }

        //keybinds for switching to different loadout slots
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.GetComponent<LoadoutsToFile>().switchLoadouts(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InventoryManager.GetComponent<LoadoutsToFile>().switchLoadouts(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InventoryManager.GetComponent<LoadoutsToFile>().switchLoadouts(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InventoryManager.GetComponent<LoadoutsToFile>().switchLoadouts(4);
        }

        //this is for the FixedUpdate to help get rid of the jitteriness
        isMovingLeft = Input.GetKey(KeyCode.A);
        isMovingRight = Input.GetKey(KeyCode.D);

        //isGrounded makes it so the player isn't able to spam the jump button while in mid-air
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            alpha.AddForce(Vector3.up * jumpSpd);
            float tempFallSpd = fallSpd;
            fallSpd = 0.5f; //this is to sort of help reset the jump
            //Debug.Log("fall speed is now " + fallSpd);
            fallSpd = tempFallSpd;
            canDoubleJump = true;
        }
        else if(Input.GetButtonDown("Jump") && canDoubleJump)
        {
            alpha.velocity = Vector3.zero;
            alpha.AddForce(Vector3.up * jumpSpd);
            float tempFallSpd = fallSpd;
            fallSpd = 0.5f; //this is to sort of help reset the jump
            fallSpd = tempFallSpd;
            canDoubleJump = false;
        }

        //this is to use dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    //to help get rid of the jitteriness
    private void FixedUpdate()
    {
        if (isMovingLeft)
        {
            if(!explosion.preventMoving)
            {
                this.gameObject.transform.Translate(new Vector3(0, 0, -alphaMovementSpd * Time.deltaTime));
            }
        }
        else if (isMovingRight)
        {
            if(!explosion.preventMoving)
            {
                this.gameObject.transform.Translate(new Vector3(0, 0, alphaMovementSpd * Time.deltaTime));
            }
        }
    }

    //using raycast to detect if player is grounded
    private bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, -gameObject.transform.up, boxCollider.bounds.extents.y + 0.1f);
        hasDashed = false;
        return isGrounded;
    }

    private IEnumerator Dash()
    {
        if (this.gameObject.transform.rotation.y == 90 && !hasDashed || isMovingRight && !hasDashed)
        {
            alpha.useGravity = false;
            alpha.velocity = new Vector3(transform.localScale.x * dashPush, 0, 0);
            hasDashed = true;
        }
        else if(isMovingLeft && !hasDashed)
        {
            alpha.useGravity = false;
            alpha.velocity = new Vector3(-transform.localScale.x * dashPush, 0, 0);
            hasDashed = true;
        }

        yield return new WaitForSeconds(0.5f);
        alpha.velocity = Vector3.zero;
        alpha.useGravity = true;
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
            //explosion.pushed = false;
            //explosion.alpha = this; //for some reason I can't put the player onto the explosion object so this is a supplement for that
            //explosion.Aiming();
            //GameObject g = Instantiate(spellAttack, spellSpawn.position, spellSpawn.rotation);
            //lastShot = Time.time;
            //Destroy(g, 0.5f);

            //this is the icicle spear spell
            aimingDirection = FindObjectOfType<Aiming>().AimDirection();
            IcicleSpearSpell icicleSpear = Instantiate(iciclePrefab, spellSpawn.position, spellSpawn.rotation);
            icicleSpear.Aiming(aimingDirection);
            lastShot = Time.time;
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

            //this is the lightning spell
            //GameObject g = Instantiate(spellAttack2, spellSpawn.position, spellSpawn.rotation);
            //aimingDirection = FindObjectOfType<Aiming>().AimDirection();
            //Rigidbody rg = g.GetComponent<Rigidbody>();
            //rg.velocity = new Vector3(aimingDirection.x, aimingDirection.y, 0) * 20f;
            ////shootingSpell.Aiming();
            //lastShot = Time.time;
            //Destroy(g, 1f);

            //this is the sound wave spell
            aimingDirection = FindObjectOfType<Aiming>().AimDirection();
            SoundWaveSpell soundWave = Instantiate(soundWavePrefab, spellSpawn.position, spellSpawn.rotation);
            soundWave.Aiming(aimingDirection);
            lastShot = Time.time;
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
    public void TakeDamage(int damage)
    {
        Debug.Log("current health is: " + currentHealth);
        currentHealth = currentHealth - damage;
        Debug.Log(currentHealth);
        healthBar.SetHealth(currentHealth);
    }

    void useMana(int lostMana)
    {
        currentMana -= lostMana;

        manaBar.SetMana(currentMana);
    }
}
