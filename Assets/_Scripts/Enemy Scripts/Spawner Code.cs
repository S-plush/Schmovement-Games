using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCode : MonoBehaviour
{


    public List<GameObject> enemyList;

    public List<int> waveList;

    public GameObject rightMarker;
    public GameObject leftMarker;

    public bool cleared;

    public bool activated;

    private List<GameObject> spawnedEnemies;

    private int waveNum;
    private int currWave;

    private float randomX;
    private float randomY;

    private System.Random random = new System.Random();



    int tempNum = 0;

    int tempWave = 0;

    int stuckBreaker = 0;

    List<int> tempWaveList = new List<int>();

    bool enemiesKilled = true;

    void Start() {
        waveNum = waveList.Count;
        currWave = 0;
        spawnedEnemies = new List<GameObject>();
    
    }



    void FixedUpdate() {
        if (activated) {
            cleared = false;
            if (cleared == false) {
                stuckBreaker++;
                if (enemiesKilled == true && currWave < waveNum) {


                    enemiesKilled = false;
                    tempNum = waveList[currWave].ToString().Length;


                    tempWave = waveList[currWave];

                    tempWaveList.Clear();


                    while (tempWave > 0) {

                        tempWaveList.Add(tempWave % 10);

                        tempWave = tempWave / 10;
                    }


                    foreach (int i in tempWaveList) {

                        randomizeSpawnLocation();
                        GameObject tempEnemy = Instantiate(enemyList[i - 1], new Vector3(randomX, randomY, this.transform.position.z), transform.localRotation);
                        spawnedEnemies.Add(tempEnemy);
                    }
                }

                for (int i = 0; i < spawnedEnemies.Count; i++) {
                    if (spawnedEnemies[i] == null) {
                        spawnedEnemies.RemoveAt(i);
                        i--;
                    }
                }

                if (spawnedEnemies.Count == 0) {
                    enemiesKilled = true;
                    currWave++;
                }

                if (currWave > waveNum) {
                    cleared = true;
                    activated = false;
                    currWave = 0;

                }


            }
        }
    }

    public void randomizeSpawnLocation() {
        randomX = random.Next((int)leftMarker.transform.position.x, (int)rightMarker.transform.position.x);

        randomY = random.Next((int)leftMarker.transform.position.y, (int)rightMarker.transform.position.y);

    }

    public void restartSpawner() {
        cleared = false;
        activated = true;
        currWave = 0;

        foreach (GameObject x in spawnedEnemies) {
            Destroy(x);
        }

        spawnedEnemies.Clear();
    }


}
