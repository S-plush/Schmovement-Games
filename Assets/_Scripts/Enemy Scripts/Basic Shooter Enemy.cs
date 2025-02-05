using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicShooterEnemy : MonoBehaviour
{
    public GameObject player;
    private GameObject enemy;

    [SerializeField] private float atkFrequency;

    private Rigidbody enemyRB;
    private float timer;
    private float delay;

    private bool facingRight;
    private bool facingLeft;

    private bool inRange;

    void Start() {
        enemy = this.gameObject;
        enemyRB = GetComponent<Rigidbody>();
        timer = 0;
        inRange = false;
    }

    // Start is called before the first frame update
    void Update() {

        facePlayer();



        timer += Time.deltaTime;
        delay += Time.deltaTime;

        while (timer >= atkFrequency) {
            shootAttack();
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

    void moveToPlayer() {
        if(Vector3.Distance(enemy.transform.position, player.transform.position) > 20f) {
            inRange = false;
            Vector3 pos = Vector3.MoveTowards(transform.position, player.transform.position, 10f * Time.deltaTime);

            enemyRB.MovePosition(pos);
        }
        else {
            timer = 0;
            inRange = true;

            while (timer >= atkFrequency) {
                shootAttack();
                timer -= atkFrequency;
            }
        }
    }

    void shootAttack() {

        delay = 0;




    }
}
