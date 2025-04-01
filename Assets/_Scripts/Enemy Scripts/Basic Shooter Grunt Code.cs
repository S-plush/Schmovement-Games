using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicShooterGruntCode : MonoBehaviour
{
    public GameObject player;
    public GameObject fireArea;
    public GameObject bullet;
    public LedgeChecker ledgeChecker;


    public int health;
    public int speed;

    public float detectionRange;
    public float fireRange;


    private GameObject enemy;

    [SerializeField] private float shootFrequency;

    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;

    private float atkFrequency;


    private Rigidbody enemyRB;
    private float timer;



    private bool facingRight;
    private bool facingLeft;

    private bool isGrounded;
    private bool inRange;
    private bool detected;

    private bool canFire;

    private bool canMove;

    // Start is called before the first frame update
    void Start() {

        terrainLayerMask = LayerMask.GetMask("Default");
        playerLayerMask = LayerMask.GetMask("Player");
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;
        atkFrequency = shootFrequency;
        canFire = true;
        canMove = true;

        if (health <= 0) {
            health = 2;
        }

    }

    // Update is called once per frame
    void FixedUpdate() {

        if (health == 0) {
            Destroy(this.gameObject);
        }


        if (Vector3.Distance(enemy.transform.position, player.transform.position) > fireRange) {
            inRange = false;
        } else {
            inRange = true;
        }

        timer += Time.deltaTime;

        if (Vector3.Distance(enemy.transform.position, player.transform.position) > detectionRange) {
            detected = false;
        } else {
            detected = true;
        }


        facePlayer();

        if (ledgeChecker.isGroundDetected()) {
            if (detected && !inRange) {
                if (facingLeft) {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x - 5, this.transform.position.y, this.transform.position.z), 0.05f);
                } else if (facingRight) {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x + 5, this.transform.position.y, this.transform.position.z), 0.05f);
                }


            } else {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position, 0.1f);
            }
        }
        
        


        while (timer >= atkFrequency) {

            facePlayer();
            aimAtPlayer();
            startAttack();
            timer -= atkFrequency;
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

    void aimAtPlayer() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));

    }

    void startAttack() {
        RaycastHit hit;

        if (isGrounded && inRange) {
            if (Physics.Raycast(transform.position, transform.TransformDirection(player.transform.position), out hit, 10f, terrainLayerMask)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * hit.distance, Color.yellow);
                Debug.Log("Did Not Hit Player");

            } else {


                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * 10f, Color.white);
                Debug.Log("Hit Player");

                Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);

            }
            Debug.Log("Fire!");
        } 
    
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
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
