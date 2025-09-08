using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public int health;
    public int damage;
    public int contactDamage;
    public float atkFrequency;

    public Animator animator;
    public GameObject fireArea;


    protected float timer;
    protected bool attacking;
    protected bool isFacingRight;
    protected bool isFacingLeft;



    protected GameObject player;
    protected GameObject thisEnemyObject;
    protected Rigidbody thisRigidBody;
    


    void Awake() {

        player = GameObject.Find("Player_Alpha");
        thisEnemyObject = this.gameObject;
        thisRigidBody = player.GetComponent<Rigidbody>();
        
        attacking = false;
    }

    public void facePlayer() {
        if (player.transform.position.x > thisEnemyObject.transform.position.x) {
            thisEnemyObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            isFacingRight = true;
            isFacingLeft = false;

        } else if (player.transform.position.x < thisEnemyObject.transform.position.x) {
            thisEnemyObject.transform.rotation = Quaternion.Euler(0, 270, 0);
            isFacingRight = false;
            isFacingLeft = true;
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player Spell") {
            health -= 1;
        }

        if (other.tag == "Player") {
            other.GetComponent<Alpha>().TakeDamage(contactDamage);
        }
    }

}
