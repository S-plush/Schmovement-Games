using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VZ17Code : MonoBehaviour {

    public GameObject player;
    public GameObject pack;
    public GameObject fireArea;
    public GameObject bullet;

    private GameObject enemy;

    [SerializeField] private float shootFrequency;
    [SerializeField] private float jumpAtkFrequency;

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
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;
        atkFrequency = shootFrequency;
        canFire = true;

    }

    // Update is called once per frame
    void Update() {

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
        fireArea.transform.LookAt(player.transform.position);

    }

    void startAttack() {


        if (isGrounded && inRange && canFire == true) {
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);

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
}