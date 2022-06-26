using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    int numPattern;
    public GameObject enemyPrefab;

    public GameObject[] spawn;
    public GameObject[] enemy;

    void Start()
    {
        spawn = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemy.Length <= 0)
        {
            numPattern++;
            //Debug.Log("random num " + numPattern);
        }

        if (numPattern == 1)
        {
            Instantiate(enemyPrefab, spawn[0].transform);
            Instantiate(enemyPrefab, spawn[1].transform);
            Instantiate(enemyPrefab, spawn[2].transform);
            Instantiate(enemyPrefab, spawn[3].transform);
            numPattern = 0;
        }
    }

}
