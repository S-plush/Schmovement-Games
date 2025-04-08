using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fireArea;
    public GameObject bullet;


    private GameObject enemy;

    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;


    public int health;


    public float fireRange;

    [SerializeField] private float shootFrequency;


    private Rigidbody enemyRB;
    private float timer;

    private bool inRange;



    // Start is called before the first frame update
    void Start() {
        terrainLayerMask = LayerMask.GetMask("Default");
        playerLayerMask = LayerMask.GetMask("Player");
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;

        enemyRB = GetComponent<Rigidbody>();

        if (health == 0) {
            health = 3;
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


        while (timer >= shootFrequency) {
            Debug.Log("Fire!");
            aimAtPlayer();
            startAttack();
            timer -= shootFrequency;


        }

    }
    
    void aimAtPlayer() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
    }

    void startAttack() {
        RaycastHit hit;

        Debug.Log("Player be like " + player.transform.position + "\n Enemy Be like " + transform.position);

        Debug.Log(inRange + " for range");
        if (inRange) {

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, player.transform.position, out hit, 10f, terrainLayerMask)) {
                Debug.DrawRay(transform.position, player.transform.position * hit.distance, Color.yellow);
                Debug.Log("Hit Terrain");
                Debug.Log("hit: " + hit.transform);

            } else {


                Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * 10f, Color.white);
                Debug.Log("Did not Hit");

                Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);

            }
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
