using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BasicJumpEnemy : MonoBehaviour {

    public GameObject player;
    private GameObject enemy;

    [SerializeField] private float atkFrequency;

    [SerializeField] private float forwardVelocity;
    [SerializeField] private float upwardVelocity;

    private Rigidbody enemyRB;
    private float timer;
    private float delay;

    private bool facingRight;
    private bool facingLeft;

    // Start is called before the first frame update
    void Start() {
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;

        //InvokeRepeating("jumpAttack", 3f, 5f);
    }

    // Update is called once per frame
    void Update() {

        facePlayer();


        //jumpAttack();


        timer += Time.deltaTime;
        delay += Time.deltaTime;

        while (timer >= atkFrequency) {
            jumpAttack();
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

    void jumpAttack() {

        delay = 0;
        enemyRB = GetComponent<Rigidbody>();

        if (facingRight) {
            Debug.Log("jumped");

            enemyRB.velocity = new Vector3(forwardVelocity, upwardVelocity, 0);
        } else if (facingLeft) {
            Debug.Log("jumped");

            enemyRB.velocity = new Vector3(-forwardVelocity, upwardVelocity, 0);

        }

    }

}