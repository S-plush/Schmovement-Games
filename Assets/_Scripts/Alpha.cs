using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor.SceneManagement;
using System.Runtime.CompilerServices;
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

    List<InventorySpell> allInventorySpells = new List<InventorySpell>();

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
    public float iFrameTimer;

    private float lastStepTime;
    private float lastShot1; //cooldown for the spell 1
    private float lastShot2;
    private float lastDash;
    private float lastDamageTaken;
    private bool hasDashed = false;
    private bool canDoubleJump = false;
    private float lastDirectionFaced;

    private CharacterController alpha;
    public static bool isGamePaused = false;

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
    private DBHolder dialogueCutscene;

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

    public GameObject InventoryManager;
    private InvDataBetweenRuns invData;

    public GameObject Settings;

    [HideInInspector] public int[] indexs; //used to store the output of LoadoutsToFile.switchLoadouts(). the two values saved in this array are index references to which item in the keyArray are equipped

    public string leftSpell; //keeps track of the name of the spell that the UI loadout slot says should be being shot
    public string rightSpell; //keeps track of the name of the spell that the UI loadout slot says should be being shot

    LoadoutsToFile LoadoutsToFileScript;

    Checkpoints CheckpointsScript;

    public int currentlyEquippedLoadout = 1;

    public Animator animator;

    public static GameObject PlayerRef; //reference to current instantiated player

    void Awake()
    {
        InitializeVariables();

        PlayerRef = this.gameObject;

        Time.timeScale = 1.0f;
        alpha = GetComponent<CharacterController>();
        originalStepOffset = alpha.stepOffset;
        invData = FindObjectOfType<InvDataBetweenRuns>();

        

        //currentHealth = 5; ///////
        //currentMana = 5; ////

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);

        

        CheckpointsScript = FindObjectOfType<Checkpoints>();
        LoadoutsToFileScript = FindObjectOfType<LoadoutsToFile>();
    }

    void Start()
    {
        Inventory.SetActive(false);
        HUD.SetActive(true);
        stimCount = maxStims;
        stimCountText.text = stimCount + "\n\nStims";
        healthFromStim = 3; //////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        manaFromStim = 1; ////////////////////////////////////////////////////////////////////////////////////////////////////input from file later
        Settings.SetActive(false);

        StartCoroutine(InitialLoadoutCall(currentlyEquippedLoadout));
        sfxManager = FindAnyObjectByType<SFXManager>();
        dialogueCutscene = FindAnyObjectByType<DBHolder>();
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
        else if (Input.GetKeyDown(KeyCode.E) && !isGamePaused)
        {
            MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.I)) //open inventory keybind (also saves spells that are in loadout slots when inventory is opened/closed)
        {
            if(dialogueCutscene == null)
            {
                OpenMenu();
            }
            else if(!dialogueCutscene.InDialogueCheck())
            {
                OpenMenu();
            }

                //LoadoutsToFileScript.saveLoadoutsToFile(); this happens in OpenMenu() now
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isGamePaused) //use of stim keybind
        {
            UseStim();
        }


        //keybinds for switching to different loadout slots
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Loadout.currentLoadoutSelected = 1;
            LoadoutsToFileScript.switchLoadouts(1);
            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Loadout.currentLoadoutSelected = 2;
            LoadoutsToFileScript.switchLoadouts(2);
            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Loadout.currentLoadoutSelected = 3;
            LoadoutsToFileScript.switchLoadouts(3);
            leftSpell = LoadoutsToFileScript.equippedSpells[0];
            rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Loadout.currentLoadoutSelected = 4;
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

            if (Input.GetButtonDown("Jump") && !isDead && !isGamePaused)
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

        if (!isDead && !isGamePaused)
        {
            alpha.Move(velocity * Time.deltaTime);
        }

        #endregion

        //this is to use dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed && !isDead && !isGamePaused)
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
        if (Input.GetKeyDown(KeyCode.Escape) && VentScript.playerInside == false) //updated to prevent menu storage bug with vent menu
        {

            if (Settings.activeSelf)
            {
                HUD.SetActive(true);
                Settings.SetActive(false);
                Time.timeScale = 1.0f;
                isGamePaused = false;
            }
            else
            {
                HUD.SetActive(false);
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
            animator.SetFloat("VelocityX", velocity.x);
        }

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
        //opening the inventory
        if (!Inventory.activeInHierarchy)
        {
            Inventory.SetActive(true);
            isGamePaused = true;
            invData.LoadInventory();
            Time.timeScale = 0.0f;

            allInventorySpells = new List<InventorySpell>(FindObjectsOfType<InventorySpell>());

            //reset all spells when opening the inventory
            foreach (var spell in allInventorySpells)
            {
                if (spell != null)
                {
                    if (spell.parentAfterDrag == null)
                        spell.parentAfterDrag = spell.transform.parent;

                    spell.ResetSpell();
                    spell.image.raycastTarget = true;
                    spell.transform.SetParent(spell.parentAfterDrag);
                    spell.transform.localPosition = Vector3.zero;
                }
            }
        }
        //closing the inventory
        else if (Inventory.activeInHierarchy)
        {
            //remove null spells
            List<InventorySpell> validSpells = new List<InventorySpell>();
            foreach (var spell in allInventorySpells)
            {
                if (spell != null)
                    validSpells.Add(spell);
            }
            allInventorySpells = validSpells;

            //reset all spells when closing the inventory
            foreach (var spell in allInventorySpells)
            {
                if (spell != null)
                {
                    if (spell.transform.parent != spell.parentAfterDrag)
                    {
                        spell.transform.SetParent(spell.parentAfterDrag);
                        spell.transform.localPosition = Vector3.zero;

                        PointerEventData eventData = new PointerEventData(EventSystem.current);
                        spell.OnEndDrag(eventData);
                    }

                    spell.ResetSpell();
                    spell.image.raycastTarget = true;
                }
            }

            //save inventory and close
            invData.SaveInventory();
            LoadoutsToFileScript.saveLoadoutsToFile();
            Inventory.SetActive(false);
            isGamePaused = false;
            Time.timeScale = 1.0f;
            LoadoutsToFileScript.switchLoadouts(currentlyEquippedLoadout);
            //leftSpell = LoadoutsToFileScript.equippedSpells[0];
            //rightSpell = LoadoutsToFileScript.equippedSpells[1];
        }
    }













    void ShootSpell1()
    {
        //Debug.Log(leftSpell);////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (activeSpell.activeInHierarchy && !isGamePaused)
        {
            if (Time.time - lastShot1 < timer)
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

            lastShot1 = Time.time;
        }
    }

    void ShootSpell2()
    {
        if (activeSpell.activeInHierarchy && !isGamePaused)
        {
            if (Time.time - lastShot2 < timer)
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

            lastShot2 = Time.time;
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

    public void PauseGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
        }
        else if (!isGamePaused)
        {
            isGamePaused = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if(Time.time - lastDamageTaken < iFrameTimer)
        {
            return;
        }

        Debug.Log("current health is: " + currentHealth);
        currentHealth = currentHealth - damage;
        Debug.Log(currentHealth);
        healthBar.SetHealth(currentHealth);
        lastDamageTaken = Time.time;
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

        if (loadoutNum != 1 && loadoutNum != 2 && loadoutNum != 3 && loadoutNum != 4)
        {
            LoadoutsToFileScript.switchLoadouts(1);
        }
        else
        {
            Loadout.currentLoadoutSelected = loadoutNum;
            LoadoutsToFileScript.switchLoadouts(loadoutNum);
        }

        OpenMenu();
        LoadoutsToFileScript.saveLoadoutsToFile();
        OpenMenu();

        //I am pretty sure these two lines are reduntant now since this happens in both OpenMenu() and things that OpenMenu() calls in LoadoutsToFile script
        leftSpell = LoadoutsToFileScript.equippedSpells[0];
        rightSpell = LoadoutsToFileScript.equippedSpells[1];

        this.GetComponent<CharacterController>().enabled = true; //obviously very important!

        FindObjectOfType<MiscDataToFile>().saveAllMiscData();
    }

    public void InitializeVariables()
    {
        respawnPoint = FindObjectOfType<RespawnPoint>(true);
        respawnPointObj = respawnPoint.gameObject;

        deathScreen = FindInScene("ded Screen");

        Inventory = FindInScene("Main Inventory Group");

        HUD = FindInScene("Main HUD Group");

        healthBar = FindObjectOfType<HealthBar>(true);

        manaBar = FindObjectOfType<ManaBar>(true);

        stimCountText = FindInScene("Stim Counter").ConvertTo<TMP_Text>();

        InventoryManager = FindInScene("InventoryManager");
    
        Settings = FindInScene("SettingsMenuStuff");

        //GameObject.Find("Canvas").GetComponentsInChildren<GameObject>(true).FirstOrDefault(t => t.name == "Main Inventory Group")?.gameObject;
        //FindInScene("Main Inventory Group");
    }

    public static GameObject FindInScene(string name)
    {
        return SceneManager.GetActiveScene()
                           .GetRootGameObjects()
                           .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                           .FirstOrDefault(t => t.name == name)?.gameObject;
    }

    #region Spells
    public void UseExplosionSpell()
    {
        sfxManager.ExplosionSpellSFX();
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
        sfxManager.IceicleSpearSpellSFX();
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