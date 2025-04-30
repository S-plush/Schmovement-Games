using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BasicJumpEnemy : MonoBehaviour {

    private GameObject player;

    public int health;

    private GameObject enemy;

    [SerializeField] private float atkFrequency;

    [SerializeField] private float forwardVelocity;
    [SerializeField] private float upwardVelocity;

    private Rigidbody enemyRB;
    private float timer;

    private bool facingRight;
    private bool facingLeft;

    private bool isGrounded;
    private bool inRange;

    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        enemy = this.gameObject;
        player = GameObject.Find("Player_Alpha");

        enemyRB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        timer = 0;

        if(health == 0) {
            health = 1;
        }

    }

    // Update is called once per frame
    void Update() {

        if(health <= 0) {
            Destroy(this.gameObject); 
        }

        facePlayer();

        if(Vector3.Distance(enemy.transform.position, player.transform.position) > 8f) {
            inRange = false;
        } else {
            inRange = true;
        }

        timer += Time.deltaTime;

        while (timer >= atkFrequency) {
            jumpAttack();
            timer -= atkFrequency;
        }
        animator.SetBool("Grounded", isGrounded);
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

    void jumpAttack() {


        if(isGrounded && inRange) {
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
        if(other.tag == "Player Spell") {
            health -= 1; 
        }

        if (other.tag == "Player") {
            other.GetComponent<Alpha>().TakeDamage(1);
        }
    }
}