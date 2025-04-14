using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fireArea;
    [SerializeField] private GameObject fireAreaL;
    [SerializeField] private GameObject fireAreaR;


    public GameObject bullet;
    public GameObject laser;

    private GameObject enemy;

    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;


    public int health;


    public float fireRange;

    public bool lockOn;

    public bool triShot;

    public bool rapidShot;

    public bool shootLaser;


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

            if (lockOn) {
                aimAtPlayer();
            }
            


            startAttack();
            timer -= shootFrequency;


        }

    }
    
    void aimAtPlayer() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
        fireAreaL.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
        fireAreaR.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));

    }

    void startAttack() {
        RaycastHit hit;

        //Debug.Log("Player be like " + player.transform.position + "\n Enemy Be like " + transform.position);

        Debug.Log(inRange + " for range");


        if (rapidShot == false) {

            if (inRange) {

                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, player.transform.position, out hit, 10f, terrainLayerMask)) {
                    Debug.DrawRay(transform.position, player.transform.position * hit.distance, Color.yellow);
                    Debug.Log("Hit Terrain");
                    Debug.Log("hit: " + hit.transform);

                } else {


                    Debug.DrawRay(transform.position, transform.TransformDirection(player.transform.position) * 10f, Color.white);
                    Debug.Log("Did not Hit");

                    if (triShot) {
                        if (shootLaser == false) {
                            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
                            Instantiate(bullet, fireAreaL.transform.position, fireAreaL.transform.rotation);
                            Instantiate(bullet, fireAreaR.transform.position, fireAreaR.transform.rotation);
                        } else if (shootLaser == true) {
                            Instantiate(laser, fireArea.transform.position, fireArea.transform.rotation);
                            Instantiate(laser, fireAreaL.transform.position, fireAreaL.transform.rotation);
                            Instantiate(laser, fireAreaR.transform.position, fireAreaR.transform.rotation);
                        }
                    } else {
                        if (shootLaser == false) {
                            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
                        } else if (shootLaser == true) {
                            Instantiate(laser, fireArea.transform.position, fireArea.transform.rotation);
                        }
                    }

                }
            }
        } else if (rapidShot) {
            StartCoroutine(burstFire());
        }
        

    }

    IEnumerator burstFire() {
        if (triShot) {
            for (int i = 0; i < 3; i++) {
                if(shootLaser == false) {
                    Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
                    Instantiate(bullet, fireAreaL.transform.position, fireAreaL.transform.rotation);
                    Instantiate(bullet, fireAreaR.transform.position, fireAreaR.transform.rotation);
                } else if (shootLaser) {
                    Instantiate(laser, fireArea.transform.position, fireArea.transform.rotation);
                    Instantiate(laser, fireAreaL.transform.position, fireAreaL.transform.rotation);
                    Instantiate(laser, fireAreaR.transform.position, fireAreaR.transform.rotation);
                }
                

                yield return new WaitForSeconds(.5f);
            }
            
        } else {
            for (int i = 0; i < 3; i++) {
                if (shootLaser == false) {
                    Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
                } else if (shootLaser == true) {
                    Instantiate(laser, fireArea.transform.position, fireArea.transform.rotation);
                }

                yield return new WaitForSeconds(.5f);
            }
        }

        timer = 0;


    }

    private void OnTriggerEnter(Collider other) {



        if (other.tag == "Player Spell") {            
            health -= 1;
            Debug.Log("Got Hit! Health is now: " + health);
        }

        if (other.tag == "Player") {
            other.GetComponent<Alpha>().TakeDamage(1);
        }
    }
}
