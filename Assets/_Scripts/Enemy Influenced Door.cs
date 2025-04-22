using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyInfluencedDoor : Doors
{
    [SerializeField] private GameObject enemyWaveManager;

    //Enemy's scripts will need these lines of code:
    //[SerializeField] private GameObject enemyWaveManager;

    //private void OnDestroy()
    //{
    //    if (enemyWaveManager != null)
    //    {
    //        enemyWaveManager.GetComponent<EnemyDefeatTrigger>().RemoveEnemy(this.gameObject);
    //    }
    //}


    // Start is called before the first frame update
    void Start()
    {
        enemyWaveManager.GetComponent<EnemyDefeatTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyWaveManager.GetComponent<EnemyDefeatTrigger>().IsDoorOpen())
        {
            if (other.gameObject.tag == "Player")
            {
                door.transform.Translate(new Vector3(0, 0, 5));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyWaveManager.GetComponent<EnemyDefeatTrigger>().IsDoorOpen())
        {
            if (other.gameObject.tag == "Player")
            {
                door.transform.Translate(new Vector3(0, 0, -5));
            }
        }
    }
}
