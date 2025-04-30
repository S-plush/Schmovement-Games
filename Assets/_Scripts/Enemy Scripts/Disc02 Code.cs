using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc02Code : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject fireArea;
    public GameObject bullet;


    private GameObject enemy;

    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;


    private bool forward, up, down;

    public int health;

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
    }

    // Update is called once per frame
    void FixedUpdate() {
        //RaycastHit hit;


        if (health <= 0) {
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

        timer += Time.deltaTime;

        if (detected && inRange) {
            checkDirections();

            if (forward && facingLeft) {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-1, 0, 0), 0.1f);
            }
            if (forward && facingRight) {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(1, 0, 0), 0.1f);
            }

            if (down) {
                if (enemy.transform.position.y > player.transform.position.y) {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, -1, 0), 0.1f);
                }
            }

            if (up) {
                if (enemy.transform.position.y < player.transform.position.y) {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), 0.1f);
                }
            }
        }

        if (!inRange) {
            enemyRB.velocity = Vector3.zero;
        }


        while (timer >= moveFrequency) {
            facePlayer();
            aimAtPlayer();
            startAttack();
            timer -= moveFrequency;


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
        //back = true;
        up = true;
        down = true;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, terrainLayerMask)) {
            forward = false;
            //Debug.Log("Hit Forward");
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out hit, 10f, terrainLayerMask)) {
            //back = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Hit back");

        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(transform.up), out hit, 5f, terrainLayerMask)) {
            up = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(transform.up) * hit.distance, Color.yellow);
            //Debug.Log("Hit up");

        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(-transform.up), out hit, 5f, terrainLayerMask)) {
            down = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(-transform.up) * hit.distance, Color.yellow);
            //Debug.Log("Hit down");

        }


    }

    void aimAtPlayer() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));

    }

    void startAttack() {
        RaycastHit hit;
        if (inRange)
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(player.transform.position), out hit, 10f, terrainLayerMask)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * hit.distance, Color.yellow);
                Debug.Log("Hit Terrain");

            } else {


                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * 10f, Color.white);
                //Debug.Log("Did not Hit");

                Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);

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
