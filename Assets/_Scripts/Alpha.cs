using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using UnityEditor.Experimental.GraphView;

public class Alpha : MonoBehaviour
{
    [HideInInspector] public float alphaMovementSpd = 7f;
    [HideInInspector] public float jumpSpd = 7f;
    [HideInInspector] public float fallSpd = 2.5f;
    [HideInInspector] public float dashPush = 4;
    [HideInInspector] public float gravity = -9.8f;
    private float slopeDirection;
    private Vector3 velocity;
    private float ySpeed;
    private float originalStepOffset;
    private bool isDead = false;

    [SerializeField] private Transform alphaModel;

    //set up for this test build, but will need to have an abstract class for all the spells
    public Transform spellSpawn; //spawn point for spell's attack
    public Transform meleeSpawn; //spawn point for melee attack
    public GameObject spellAttack; //for the explosion spell effect/attack prefab
    public GameObject activeSpell; //for rn the spell's spawnpoint is what's used for this
    public Transform rotationPoint;
    [HideInInspector] public Vector3 aimingDirection;
    public float timer; //for spell
    public float dashTimer;
    public float stepTimer;

    private float lastStepTime;
    private float lastShot; //cooldown for the spell 1
    private float lastDash;
    private bool hasDashed = false;
    private bool canDoubleJump = false;
    private float lastDirectionFaced;

    private CharacterController alpha;
    private bool isGamePaused = false;

    [Header("Attack/Spells Prefabs")] 
    public ExplosionSpell explosionPrefab;
    public LightningSpell lightningPrefab;
    public IcicleSpearSpell iciclePrefab;
    public SoundWaveSpell soundWavePrefab;
    public WindSpell windPrefab;
    public EarthSpell earthPrefab;
    public BoulderSpell boulderPrefab;
    public MeleeAttack meleePrefab;

    private SFXManager sfxManager;

    [HideInInspector] public bool isMovingLeft = false;
    [HideInInspector] public bool isMovingRight = false;
    private float moveDirection;

    [Header("Respawn Stuff")]
    public RespawnPoint respawnPoint;
    public GameObject respawnPointObj;

    public GameObject deathScreen;

    [Header("UI Stuff")]
    public GameObject Inventory;

    public GameObject HUD;

    public HealthBar healthBar;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    public ManaBar manaBar;
    [HideInInspector] public int maxMana;
    [HideInInspector] public int currentMana;

    public TMP_Text stimCountText;
    public int maxStims;
    public int stimCount;
    public int manaFromStim;
    public int healthFromStim;

    public string currentCheckpointName = "default";
    public static string currentSceneName;

    public GameObject InventoryManager;
    private InvDataBetweenRuns invData;

    public GameObject Settings;

    [HideInInspector] public int[] indexs; //used to store the output of LoadoutsToFile.switchLoadouts(). the two values saved in this array are index references to which item in the keyArray are equipped

    public string leftSpell; //keeps track of the name of the spell that the UI loadout slot says should be being shot
    public string rightSpell; //keeps track of the name of the spell that the UI loadout slot says should be being shot

    LoadoutsToFile LoadoutsToFileScript;

    Checkpoints CheckpointsScript;

    public int currentlyEquippedLoadout;

    public Animator animator;

    public bool transitioned = false; //info comes from AreaChanger for use here in InitialLoadoutCall

    void Awake()
    {
        Time.timeScale = 1.0f;
        alpha = GetComponent<CharacterController>();
        originalStepOffset = alpha.stepOffset;
        invData = FindObjectOfType<InvDataBetweenRuns>();

        Inventory.SetActive(false);
        HUD.SetActive(true);
        stimCount = maxStims;
        stimCountText.text = stimCount + "\n\nStims";
        healthFromStim = 3; //////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        manaFromStim = 1; ////////////////////////////////////////////////////////////////////////////////////////////////////input from file later

        //currentHealth = 5; ///////////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        //currentMana = 5; ///////////////////////////////////////////////////////////////////////////////////////////////////////input from file later

        //currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);

        //currentMana = maxMana;
        //manaBar.SetMaxMana(maxMana);

        Settings.SetActive(false);

        CheckpointsScript = FindObjectOfType<Checkpoints>();
        LoadoutsToFileScript = FindObjectOfType<LoadoutsToFile>();
    }

    void Start()
    {
        StartCoroutine(InitialLoadoutCall(currentlyEquippedLoadout));
        sfxManager = FindAnyObjectByType<SFXManager>();
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDead)
        {
            if (currentMana > 0) //check if out of mana
            {
                if (Time.timeScale != 0.0f) //check if inventory is open
                {
                    ShootSpell1();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !isDead)
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
        else if (Input.GetKeyDown(KeyCode.E))
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

        #region movement related stuff

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0);
        float magnitude = Mathf.Clamp01(moveDirection.magnitude) * alphaMovementSpd;
        moveDirection.Normalize();
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (alpha.isGrounded)
        {
            if (horizontalInput > 0 || horizontalInput < 0)
            {
                if (Time.time - lastStepTime >= stepTimer)
                {
                    sfxManager.WalkingSFX();
                    lastStepTime = Time.time;
                }
            }

            alpha.stepOffset = originalStepOffset;
            ySpeed = -1f;
            hasDashed = false;
            canDoubleJump = true;

            if (Input.GetButtonDown("Jump") && !isDead)
            {
                ySpeed = jumpSpd;
                //canDoubleJump = true;
            }
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            ySpeed = jumpSpd;
            canDoubleJump = false;
        }
        else
        {
            alpha.stepOffset = 0;
        }

        Vector3 velocity = moveDirection * magnitude;
        velocity = OnSlope(velocity);
        velocity.y += ySpeed;

        //when hitting the ceiling, this will stop the jumping push
        if ((alpha.collisionFlags & CollisionFlags.Above) != 0)
        {
            Debug.Log("am i entering here?");
            if (velocity.y > 0)
            {
                Debug.Log("now am i in here?");
                ySpeed = -0.5f;
            }
        }

        if (!isDead)
        {
            alpha.Move(velocity * Time.deltaTime);
        }

        #endregion

        //this is to use dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed && !isDead)
        {
            if (Time.time - lastDash < dashTimer)
            {
                return;
            }

            animator.SetTrigger("Dash");
            StartCoroutine(Dash(horizontalInput));

            lastDash = Time.time;
        }

        //this is to open and close the settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Settings.activeSelf)
            {
                //HUD.SetActive(false);
                Settings.SetActive(false);
                Time.timeScale = 1.0f;
                isGamePaused = false;
            }
            else
            {
                //HUD.SetActive(true);
                Settings.SetActive(true);
                Time.timeScale = 0.0f;
                isGamePaused = true;
            }
        }

        //ANIMATOR UPDATE PARAMETERS
        //Debug.Log(alpha.isGrounded);
        animator.SetBool("Grounded", alpha.isGrounded);
        animator.SetBool("CanDoubleJump", canDoubleJump);

        if (!isDead && !isGamePaused)
        {
            animator.SetBool("isMirrored", (Input.mousePosition.x / Screen.width) - 0.5f <= 0);
        }

        animator.SetFloat("VelocityX", velocity.x);
        if (!isGamePaused)
        {
            animator.SetFloat("AimH", (Input.mousePosition.x / Screen.width) - 0.5f);
            animator.SetFloat("AimV", (Input.mousePosition.y / Screen.height) - 0.5f);
        }
        animator.SetBool("Dead", currentHealth <= 0);

        DeathCheck();
    }

    private Vector3 OnSlope(Vector3 velocity)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit slopeHit, 0.4f))
        {
            //Debug.Log("this is working");
            //Debug.DrawRay(slopeHit.point, slopeHit.normal, Color.red, 0.4f);
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, slopeHit.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    private IEnumerator Dash(float direction)
    {
        if (!hasDashed)
        {
            hasDashed = true;
            float originalYSpeed = velocity.y;
            Vector3 dashDirection = Vector3.zero;

            if (direction > 0)
            {
                dashDirection = Vector3.right;
            }
            else if (direction < 0)
            {
                dashDirection = Vector3.left;
            }
            else if (rotationPoint.rotation.z < .69f && rotationPoint.rotation.z > -.69)
            {
                dashDirection = Vector3.right;
            }
            else if(rotationPoint.rotation.z > .71f || rotationPoint.rotation.z < -.71)
            {
                dashDirection = Vector3.left;
            }

            Vector3 targetPosition = transform.position + dashDirection * dashPush;
            float dashTime = 0.2f;
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            Vector3 dashMove = Vector3.zero;

            while (elapsedTime <= dashTime)
            {
                float dashProgress = elapsedTime / dashTime;
                dashMove = Vector3.Lerp(startPosition, targetPosition, dashProgress);
                velocity.y = originalYSpeed;
                alpha.Move(dashMove - transform.position + new Vector3(0, velocity.y, 0));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);
            alphaModel.localPosition = Vector3.zero;
            velocity = Vector3.zero;
        }
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
            else if (leftSpell == "Wind")
            {
                useMana(1);
                UseWindSpell();
            }
            else if (leftSpell == "Boulder")
            {
                useMana(1);
                UseBoulderSpell();
            }
            else if (leftSpell == "Earth" && alpha.isGrounded)
            {
                useMana(1);
                UseEarthSpell();
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
            else if (rightSpell == "Wind")
            {
                useMana(1);
                UseWindSpell();
            }
            else if(rightSpell == "Boulder")
            {
                useMana(1);
                UseBoulderSpell();
            }
            else if(rightSpell == "Earth" && alpha.isGrounded)
            {
                useMana(1);
                UseEarthSpell();
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
            if(!isDead)
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
        if (currentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(Respawn());
        }
        //else if (Input.GetKeyDown(KeyCode.K))
        //{
        //StartCoroutine(Respawn());
        //}
    }

    IEnumerator Respawn()
    {
        //deathScreen.SetActive(true);
        yield return new WaitForSeconds(4.3f);
        deathScreen.SetActive(false);
        respawnPoint.RespawnPlayer();

        isDead = false;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        currentMana = maxMana;
        manaBar.SetMana(currentMana);
        stimCount = maxStims;
        stimCountText.text = stimCount + "\n\nStims";
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

    IEnumerator InitialLoadoutCall(int loadoutNum)
    {
        yield return new WaitForSeconds(.1f);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);

        //helps update the state of the inventory correctly on making a new game (so you dont get ghost spells that persist until you open your inventory)
        //BIG ISSUE IF PLAYER OPENS INVENTORY ON FIRST FRAME!?!?
        OpenMenu();
        LoadoutsToFileScript.saveLoadoutsToFile();
        OpenMenu();

        if (loadoutNum != 1 && loadoutNum != 2 && loadoutNum != 3 && loadoutNum != 4)
        {
            LoadoutsToFileScript.switchLoadouts(1);
        }
        else
        {
            LoadoutsToFileScript.switchLoadouts(loadoutNum);
        }

        leftSpell = LoadoutsToFileScript.equippedSpells[0];
        rightSpell = LoadoutsToFileScript.equippedSpells[1];

        //also checkpoint loading stuff below this
        //Debug.Log(currentCheckpointName);

        //CapsuleCollider capsule = this.GetComponentInChildren<CapsuleCollider>();

        // Temporarily disable physics
        //capsule.enabled = false;
        
         
        //this.GetComponent<CharacterController>().enabled = false;

        if (transitioned == false)
        {
            if (currentCheckpointName != "default" && currentCheckpointName != null) //COULD CHECK IF MATCHES SCENE BUT DOESN'T (unique checkpoint names are probably better (could checkpoints have a variable for what scene they are in!?!))
            {
                respawnPointObj.transform.position = GameObject.Find(currentCheckpointName).transform.position;
                //respawnPoint.respawnPoint.transform.position = GameObject.Find(currentCheckpointName).transform.position;
                this.gameObject.transform.position = GameObject.Find(currentCheckpointName).transform.position;
                //this.gameObject.transform.position = respawnPointObj.transform.position;
            }
        }
        else
        {
            transitioned = false;
        }

            this.GetComponent<CharacterController>().enabled = true;

        // Re-enable physics
        //capsule.enabled = true;


        // = respawnPoint.respawnPoint.transform.position;
        //respawnPoint.RespawnPlayer();
        //this.gameObject.transform.position = respawnPointObj.transform.position;

        Alpha.currentSceneName = SceneManager.GetActiveScene().name;
        
        FindObjectOfType<MiscDataToFile>().saveAllMiscData();
    }

    #region Spells
    public void UseExplosionSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        ExplosionSpell explosion = Instantiate(explosionPrefab, spellSpawn.position, spellSpawn.rotation);
        explosion.Aiming(aimingDirection);
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

    public void UseWindSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        WindSpell wind = Instantiate(windPrefab, spellSpawn.position, spellSpawn.rotation);
        wind.Aiming(aimingDirection);
    }

    public void UseEarthSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();

        if (aimingDirection.x > 0)
        {
            EarthSpell earthSpike = Instantiate(earthPrefab, new Vector3(transform.position.x + 5, (transform.position.y - 1.08f) + 2, 10), Quaternion.Euler(0, 0, 25));
            earthSpike.DestroyEarthSpike();
        }
        else if(aimingDirection.x < 0)
        {
            EarthSpell earthSpike = Instantiate(earthPrefab, new Vector3(transform.position.x - 5, (transform.position.y - 1.08f) + 2, 10), Quaternion.Euler(0, 0, -25));
            earthSpike.DestroyEarthSpike();
        }

    }

    public void UseBoulderSpell()
    {
        aimingDirection = FindObjectOfType<Aiming>().AimDirection();
        BoulderSpell boulder = Instantiate(boulderPrefab, spellSpawn.position, spellSpawn.rotation);
        boulder.Aiming(aimingDirection);
    }
    #endregion
}