using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour 
{
    public int health;
    public int damage;
    public int contactDamage;
    public float atkFrequency;
    public float moneyDropMin;
    public float moneyDropMax;



    public Animator animator;
    public GameObject fireArea;
    public GameObject bullet;
    public GameObject money;

    protected float timer;
    protected bool attacking;
    protected bool isFacingRight;
    protected bool isFacingLeft;



    protected GameObject player;
    protected GameObject thisEnemyObject;
    protected Rigidbody thisRigidBody;
    


    void Start() {

        player = Alpha.PlayerRef;

        thisEnemyObject = this.gameObject;
        thisRigidBody = thisEnemyObject.GetComponent<Rigidbody>();
        
        attacking = false;
        Debug.Log("This is not FB");


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

        triggerDeath();


        if (other.tag == "Player") {
            other.GetComponent<Alpha>().TakeDamage(contactDamage);
        }
    }

    public void triggerDeath() {
        if (health <= 0) {
            DropMoney();
            Destroy(this.gameObject);
        }
    }


    public void DropMoney() {
        int randMoney = (int)Random.Range(moneyDropMin, moneyDropMax + 1);
        int randDirectionX;

        for (int i = 0; i < randMoney; i++) {
            randDirectionX = (int)Random.Range(-1, 2);

            Instantiate(money, this.transform.position + new Vector3(randDirectionX, 1, 0) , this.transform.rotation);
        }


    }
}
