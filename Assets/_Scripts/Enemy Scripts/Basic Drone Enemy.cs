using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicDroneEnemy : MonoBehaviour
{
    public GameObject player;
    private GameObject enemy;

    [SerializeField] private float atkFrequency; //Also using this for how often it moves


    private Rigidbody enemyRB;
    private float timer;
    private float delay;


    //private bool inRange;

    //Pathing Helpers
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject topLeft;
    [SerializeField] private GameObject topMiddle;
    [SerializeField] private GameObject topRight;
    [SerializeField] private GameObject middleLeft;
    [SerializeField] private GameObject middleRight;
    [SerializeField] private GameObject bottomLeft;
    [SerializeField] private GameObject bottomMiddle;
    [SerializeField] private GameObject bottomRight;

    [SerializeField] private GameObject fireArea;
    [SerializeField] private GameObject bullet;

    void Start() {
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;
        //inRange = false;
    }

    // Start is called before the first frame update
    void Update() {



        facePlayer();


        timer += Time.deltaTime;
        delay += Time.deltaTime;

        while (timer >= atkFrequency) {
            moveToPlayer();
            Debug.Log("Look at me!");
            timer -= atkFrequency;
        }
    }


    void facePlayer() {
        if (player.transform.position.x > body.transform.position.x) {
            body.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (player.transform.position.x < body.transform.position.x) {
            body.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void moveToPlayer() {
        if(Vector3.Distance(enemy.transform.position, player.transform.position) > 10f) {
            //inRange = false;
            Vector3 pos = Vector3.MoveTowards(enemy.transform.position, player.transform.position, 500f * Time.deltaTime);

            enemyRB.MovePosition(pos);
            Debug.Log("working");
        }
        else{
            Debug.Log("Fire!");
            shootAttack();
            
        }
    }

    void shootAttack() {
        Debug.Log("Fired");
        Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = (player.transform.position - bullet.transform.position).normalized * 100f;

    }
}
