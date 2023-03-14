using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Spawner : MonoBehaviour
{
    private GameObject enemySpawned;
    public GameObject enemy;
    private bool gonnaSpawnEnemy = true, waitingForEnemy;

    // Update is called once per frame
    void Update()
    {
        if (gonnaSpawnEnemy)
        {
            StartCoroutine(SpawnEnemy());
        }
        if (enemySpawned == null && !waitingForEnemy)
        {
            gonnaSpawnEnemy = true;
        }
    }

    IEnumerator SpawnEnemy()
    {
        gonnaSpawnEnemy = false;
        waitingForEnemy = true;
        yield return new WaitForSeconds(2f);
        enemySpawned = Instantiate(enemy, transform.position, Quaternion.identity);
        enemySpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 7f);
        waitingForEnemy = false;
    }
}
