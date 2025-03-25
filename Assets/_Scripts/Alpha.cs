using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Alpha : MonoBehaviour
{
    public float alphaMovementSpd = 3.5f;
    public float jumpSpd = 5f;
    public float fallSpd = 2.5f;
    public float dashPush = 5;

    //set up for this test build, but will need to have an abstract class for all the spells
    public Transform spellSpawn; //spawn point for spell's attack
    public Transform meleeSpawn; //spawn point for melee attack
    public GameObject spellAttack; //for the explosion spell effect/attack prefab
    public GameObject activeSpell; //for rn the spell's spawnpoint is what's used for this
    public Transform rotationPoint;
    public Vector3 aimingDirection;
    public float timer; //for spell

    private float lastShot; //cooldown for the spell 1
    private bool isGrounded; //for jumping
    private bool hasDashed = false;
    private bool canDoubleJump = false;
    private Rigidbody alpha;
    private BoxCollider boxCollider;
    private bool isGamePaused = false;

    private ExplosionSpell explosion;
    public LightningSpell lightningPrefab;
    public IcicleSpearSpell iciclePrefab;
    public SoundWaveSpell soundWavePrefab;
    public MeleeAttack meleePrefab;
    
    [HideInInspector] public bool isMovingLeft = false;
    [HideInInspector] public bool isMovingRight = false;

    public RespawnPoint respawnPoint;

    public GameObject deathScreen;

    public GameObject Inventory;

    public GameObject HUD;

    public HealthBar healthBar;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    public ManaBar manaBar;
    [HideInInspector] public int maxMana;
    [HideInInspector] public int currentMana;

    public TMP_Text stimCountText;
    public int stimCount;
    public int manaFromStim;
    public int healthFromStim;

    public GameObject InventoryManager;
    private InvDataBetweenRuns invData;

    public GameObject Settings;

    [HideInInspector] public int[] indexs; //used to store the output of LoadoutsToFile.switchLoadouts(). the two values saved in this array are index references to which item in the keyArray are equipped

    public string leftSpell; //keeps track of the name of the spell that the UI loadout slot says should be being shot
    public string rightSpell; //keeps track of the name of the spell that the UI loadout slot says should be being shot

    LoadoutsToFile LoadoutsToFileScript;

    public int currentlyEquippedLoadout; //does nothing yet

    public Animator animator;

    void Start()
    {
        alpha = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        explosion = spellAttack.GetComponent<ExplosionSpell>();
        invData = FindObjectOfType<InvDataBetweenRuns>();

        Inventory.SetActive(false);
        HUD.SetActive(true);
        stimCount = 30; //////////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        stimCountText.text = stimCount + "\n\nStims";
        healthFromStim = 3; //////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        manaFromStim = 1; ////////////////////////////////////////////////////////////////////////////////////////////////////input from file later

        maxHealth = 5; ///////////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxMana = 5; /////////////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);

        Settings.SetActive(false);

        //makes sure the player's castable spells are set to loadout1 (because there needs to be some reference) on start
        //could be ///////////////////////////////////////////////////////////////////////////////////////////////////////////input from file later

        LoadoutsToFileScript = FindObjectOfType<LoadoutsToFile>();

        
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
        else if(Input.GetKeyDown(KeyCode.E))
        {
            MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.I)) //open inventory keybind (also saves spells that are in loadout slots when inventory is opened/closed)
        {
            OpenMenu();

            LoadoutsToFileScript.saveLoadoutsToFile();
        }

        if (Input.GetKeyDown(KeyCode.Q)) //use of stim keybind
        {
            UseStim();
        }

        
        //keybinds for switching to different loadout slots
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadoutsToFileScript.switchLoadouts(1);
            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadoutsToFileScript.switchLoadouts(2);
            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadoutsToFileScript.switchLoadouts(3);
            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadoutsToFileScript.switchLoadouts(4);

            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }

        //this is for the FixedUpdate to help get rid of the jitteriness
        isMovingLeft = Input.GetKey(KeyCode.A);
        isMovingRight = Input.GetKey(KeyCode.D);

        //isGrounded makes it so the player isn't able to spam the jump button while in mid-air
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            alpha.AddForce(Vector3.up * jumpSpd);
            isGrounded = false;
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed)
        {
            StartCoroutine(Dash());
        }

        //this is to open and close the settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Settings.activeSelf)
            {
                //HUD.SetActive(false);
                Settings.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                //HUD.SetActive(true);
                Settings.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }

        animator.SetBool("Grounded", isGrounded);
        DeathCheck();
    }

    //to help get rid of the jitteriness
    private void FixedUpdate()
    {
        if (isMovingLeft)
        {
            if(!explosion.preventMoving)
            {
                this.gameObject.transform.Translate(new Vector3(0, 0, -alphaMovementSpd * Time.deltaTime));
                animator.SetBool("isMoving", true);
                animator.SetBool("isMirrored", true);
            }
        }
        else if (isMovingRight)
        {
            if(!explosion.preventMoving)
            {
                this.gameObject.transform.Translate(new Vector3(0, 0, alphaMovementSpd * Time.deltaTime));
                animator.SetBool("isMoving", true);
                animator.SetBool("isMirrored", false);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            hasDashed = false;
        }
    }

    //using raycast to detect if player is grounded
    //private bool IsGrounded()
    //{
    //    float rayLength = 0.1f;
    //    bool isGrounded = Physics.Raycast(transform.position, -gameObject.transform.up, rayLength);

    //    if (isGrounded)
    //    {
    //        hasDashed = false;
    //    }

    //    return isGrounded;
    //}

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
            isGamePaused = true;
            invData.LoadInventory();
            Time.timeScale = 0.0f;
        }
        else if (Inventory.activeInHierarchy)
        {
            //HUD.SetActive(true);
            Inventory.SetActive(false);
            isGamePaused = false;
            invData.SaveInventory();
            Time.timeScale = 1.0f;
        }
    }

    void ShootSpell1()
    {
        Debug.Log(leftSpell);////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (activeSpell.activeInHierarchy && !isGamePaused)
        {
            if (Time.time - lastShot < timer)
            {
                return;
            }

            if (leftSpell == "empty")
            {

            }
            else if (leftSpell == "Explosion")
            {
                useMana(1);
                UseExplosionSpell();
            }
            else if (leftSpell == "Lightning")
            {
                useMana(1);
                UseLightningSpell();
            }
            else if (leftSpell == "Icicle Spear")
            {
                useMana(1);
                UseIcicleSpearSpell();
            }
            else if (leftSpell == "Sound Wave")
            {
                useMana(1);
                UseSoundWaveSpell();
            }

            lastShot = Time.time;
        }
    }

    void ShootSpell2()
    {
        if (activeSpell.activeInHierarchy && !isGamePaused)
        {
            if (Time.time - lastShot < timer)
            {
                return;
            }

            if (rightSpell == "empty")
            {

            }
            else if (rightSpell == "Explosion")
            {
                useMana(1);
                UseExplosionSpell();
            }
            else if (rightSpell == "Lightning")
            {
                useMana(1);
                UseLightningSpell();
            }
            else if (rightSpell == "Icicle Spear")
            {
                useMana(1);
                UseIcicleSpearSpell();
            }
            else if (rightSpell == "Sound Wave")
            {
                useMana(1);
                UseSoundWaveSpell();
            }

            lastShot = Time.time;
        }
    }

    void MeleeAttack()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        MeleeAttack meleeAttack;
        meleeAttack = Instantiate(meleePrefab, meleeSpawn.position, meleeSpawn.rotation);
        meleeAttack.gameObject.transform.parent = alpha.transform;
        meleeAttack.Aiming(aimingDirection);
        Destroy(meleeAttack.gameObject, 0.5f);
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

    public void DeathCheck()
    {
        if(currentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
        //else if (Input.GetKeyDown(KeyCode.K))
        //{
           //StartCoroutine(Respawn());
        //}
    }

    IEnumerator Respawn()
    {
        deathScreen.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        deathScreen.SetActive(false);
        respawnPoint.RespawnPlayer();
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        currentMana = maxMana;
        manaBar.SetMana(currentMana);
    }

    void useMana(int lostMana)
    {
        currentMana -= lostMana;

        manaBar.SetMana(currentMana);
    }

    //for testing can delete later
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Vector3 rayOrigin = transform.position;
    //    Vector3 rayDirection = -transform.up;
    //    float rayLength = 0.1f;

    //    Gizmos.DrawRay(rayOrigin, rayDirection * rayLength);
    //}

    #region Spells
    public void UseExplosionSpell()
    {
        explosion.pushed = false;
        explosion.alpha = this; //for some reason I can't put the player onto the explosion object so this is a supplement for that
        explosion.Aiming();
        GameObject g = Instantiate(spellAttack, spellSpawn.position, spellSpawn.rotation);
        lastShot = Time.time;
        Destroy(g, 0.5f);
    }

    public void UseLightningSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        LightningSpell lightning = Instantiate(lightningPrefab, spellSpawn.position, spellSpawn.rotation);
        lightning.Aiming(aimingDirection);
    }

    public void UseIcicleSpearSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        IcicleSpearSpell icicleSpear = Instantiate(iciclePrefab, spellSpawn.position, spellSpawn.rotation);
        icicleSpear.Aiming(aimingDirection);
    }

    public void UseSoundWaveSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        SoundWaveSpell soundWave = Instantiate(soundWavePrefab, spellSpawn.position, spellSpawn.rotation);
        soundWave.Aiming(aimingDirection);
    }
    #endregion
}