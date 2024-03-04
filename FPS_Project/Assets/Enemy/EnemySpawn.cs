using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject Enemy;

    public int xPos;
    public int zPos;


    [SerializeField]
    private float intervalTime = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(intervalTime, Enemy));

    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        xPos = Random.Range(18, -40);
        zPos = Random.Range(40, -3);
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(Enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));

    }

}