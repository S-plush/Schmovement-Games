using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VZ17Code : MonoBehaviour {

    public GameObject player;
    public GameObject pack;
    public GameObject fireArea;
    public GameObject bullet;

    public int health;

    private GameObject enemy;

    [SerializeField] private float shootFrequency;
    [SerializeField] private float jumpAtkFrequency;

    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;

    private float atkFrequency;

    [SerializeField] private float forwardVelocity;
    [SerializeField] private float upwardVelocity;

    private Rigidbody enemyRB;
    private float timer;



    private bool facingRight;
    private bool facingLeft;

    private bool isGrounded;
    private bool inRange;
    

    private bool canFire;

    // Start is called before the first frame update
    void Start() {

        terrainLayerMask = LayerMask.GetMask("Default");
        playerLayerMask = LayerMask.GetMask("Player");
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;
        atkFrequency = shootFrequency;
        canFire = true;

        if(health <= 0){
            health = 2;
        }

    }

    // Update is called once per frame
    void Update() {

        if(health == 0) {
            Destroy(this.gameObject);
        }

        if(pack == null) {
            canFire = false;
            atkFrequency = jumpAtkFrequency;

        }

        if (Vector3.Distance(enemy.transform.position, player.transform.position) > 12f) {
            inRange = false;
        } else {
            inRange = true;
        }

        timer += Time.deltaTime;

        while (timer >= atkFrequency) {
            facePlayer();
            aimAtPlayer();
            startAttack();
            timer -= atkFrequency;
        }
    }


    void facePlayer() {
        if (player.transform.position.x > enemy.transform.position.x) {
            enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            facingRight = true;
            facingLeft = false;

        } else if (player.transform.position.x < enemy.transform.position.x) {
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            facingRight = false;
            facingLeft = true;
        }
    }

    void aimAtPlayer() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));

    }

    void startAttack() {
        RaycastHit hit;

        if (isGrounded && inRange && canFire == true) {
            if (Physics.Raycast(transform.position, transform.TransformDirection(player.transform.position), out hit, 10f, terrainLayerMask)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");

            } else {


                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * 10f, Color.white);
                //Debug.Log("Did not Hit");

                Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);

            }
            Debug.Log("Fire!");            
        } else if (isGrounded && inRange) {
            if (facingRight) {
                //Debug.Log("jumped");

                enemyRB.velocity = new Vector3(forwardVelocity, upwardVelocity, 0);
                isGrounded = false;

            } else if (facingLeft) {
                //Debug.Log("jumped");

                enemyRB.velocity = new Vector3(-forwardVelocity, upwardVelocity, 0);
                isGrounded = false;

            }
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