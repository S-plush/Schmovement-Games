using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefeatTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private int enemyCount;
    public bool openDoor;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = enemies.Count;
        openDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount == 0)
        {
            openDoor = true;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        enemyCount--;
    }

    public bool IsDoorOpen()
    {
        if (openDoor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
