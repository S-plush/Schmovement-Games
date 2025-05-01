using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrownedDroneCode : MonoBehaviour {

    private GameObject player;
    [SerializeField] private GameObject fireArea;
    public GameObject bullet;
    public GameObject droneSummon;

    private GameObject enemy;

    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;


    private bool forward, up, down;

    public int health;
    private int maxHealth;

    public float detectionRange;
    public float fireRange;

    [SerializeField] private float shootFrequency;
    [SerializeField] private float moveFrequency;

    [SerializeField] private float moveSpeed;

    private Rigidbody enemyRB;
    private float timer;

    private bool facingRight;
    private bool facingLeft;

    private bool inRange;

    private bool detected;


    public bool activated;


    private bool moving;

    private bool attacking;


    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player_Alpha");

        terrainLayerMask = LayerMask.GetMask("Default");
        playerLayerMask = LayerMask.GetMask("Player");
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;

        enemyRB = GetComponent<Rigidbody>();


        if (health == 0) {
            health = 1;
        }

        maxHealth = health;

    }

    public void resetHealth() {
        health = maxHealth;
    }

    
    // Update is called once per frame
    void FixedUpdate() {
        //RaycastHit hit;

        if (activated) {



            if (health <= 0) {
                Destroy(this.gameObject);
            }

            if (Vector3.Distance(enemy.transform.position, player.transform.position) > fireRange) {
                inRange = false;
            } else {
                inRange = true;
            }


            if (Vector3.Distance(enemy.transform.position, player.transform.position) > detectionRange) {
                detected = false;
            } else {
                detected = true;
            }


            if (detected && inRange && moving) {
                checkDirections();

                if (forward && facingLeft) {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-1, 0, 0), 0.1f);
                }
                if (forward && facingRight) {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(1, 0, 0), 0.1f);
                }



            }

            if (!inRange) {
                enemyRB.velocity = Vector3.zero;
            }


      
           timer += Time.deltaTime;

            

                

            while (timer >= moveFrequency) {

                attacking = true;
                facePlayer();
                aimAtPlayer();
                startAttack();

                timer = 0;

                moving = true;
            }
        }

    }
    void facePlayer() {
        if (player.transform.position.x > enemy.transform.position.x) {
            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
            facingRight = true;
            facingLeft = false;

        } else if (player.transform.position.x < enemy.transform.position.x) {
            enemy.transform.rotation = Quaternion.Euler(0, 270, 0);
            facingRight = false;
            facingLeft = true;
        }
    }
    void checkDirections() {
        RaycastHit hit;

        forward = true;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, terrainLayerMask)) {
            if (facingLeft) {
                enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                facingRight = true;
                facingLeft = false;
            }

            if (facingRight) {
                enemy.transform.rotation = Quaternion.Euler(0, 270, 0);
                facingRight = false;
                facingLeft = true;
            }

        }


    }

    void aimAtPlayer() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));

    }

    void startAttack() {
        RaycastHit hit;

        System.Random rand = new System.Random();


        moving = false;

        if (inRange) {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(player.transform.position + new Vector3(0, -1, 0)), out hit, fireRange, terrainLayerMask)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * hit.distance, Color.yellow);


            } else {
                int randInt = rand.Next(1, 10);

                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * 10f, Color.white);
                Debug.Log("Fire at Player");

                if (randInt < 9) {
                    StartCoroutine(burstFire());
                }
                else if(randInt == 9) {
                    Instantiate(droneSummon, new Vector3(transform.position.x + 5, transform.position.y, transform.position.z), transform.rotation);
                    Instantiate(droneSummon, new Vector3(transform.position.x - 5, transform.position.y, transform.position.z), transform.rotation);

                }




            }
                
        }
    }


    IEnumerator burstFire() {

        for (int i = 0; i < 6; i++) {
            fireArea.transform.LookAt(player.transform.position + new Vector3(0, 2, 0));
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
            fireArea.transform.LookAt(player.transform.position);
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
            fireArea.transform.LookAt(player.transform.position + new Vector3(0, -2, 0));
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);

            Debug.Log("Fired " + i + " times");

            yield return new WaitForSeconds(.2f);
  
        }


    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player Spell") {
            health -= 1;
        }

        if (other.tag == "Player") {
            other.GetComponent<Alpha>().TakeDamage(1);
        }
    }
}
