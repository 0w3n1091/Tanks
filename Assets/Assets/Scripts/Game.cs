using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform enemiesContainer;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;
    public GameBoard gameBoard;
    public TankControl playerTank;
    public Vector2 boardSize;
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    public List<GameObject> enemiesList = new List<GameObject>();
    
    void Awake()
    {
        gameBoard.Initialize(boardSize);
        playerTank.transform.position = new Vector3(gameBoard.tiles[0].transform.position.x,
                                                    gameBoard.tiles[0].transform.position.y + 0.21f, 
                                                    gameBoard.tiles[0].transform.position.z);

        SpawnEnemies(boardSize);
    }

    /// <summary>   
    /// Spawns enemies on the board
    /// </summary>
    private void SpawnEnemies(Vector2 aBoardSize)
    {
        FillEnemiesList();

        List<Transform> spawnPoints = GetSpawnPoints(aBoardSize);
        _ = SpawnEnemiesAsync(spawnPoints); //_ = discard
    }

    /// <summary>
    /// Spawns enemies asynchronically
    /// </summary>
    /// <param name="aSpawnPoints"></param>
    /// <returns></returns>
    private async Task SpawnEnemiesAsync(List<Transform> aSpawnPoints)
    {
        int spawnPointCounter = 0;
        for (int i = 0; i < enemiesList.Count; i++)
        {
            Instantiate(enemiesList[i], new Vector3(aSpawnPoints[spawnPointCounter].transform.position.x,
                                                    aSpawnPoints[spawnPointCounter].transform.position.y + 0.21f,
                                                    aSpawnPoints[spawnPointCounter].transform.position.z), Quaternion.identity);
            
            await Task.Delay(5000);

            spawnPointCounter++;
            
            if (spawnPointCounter == 3)
                spawnPointCounter = 0;
        }
    }

    private List<Transform> GetSpawnPoints(Vector2 aBoardSize)
    {
        List<Transform> spawnList = new List<Transform>();

        spawnList.Add(gameBoard.tiles[((int)aBoardSize.y - 1) + ((int)aBoardSize.y * ((int)aBoardSize.x - 1) / 2)].transform);
        spawnList.Add(gameBoard.tiles[gameBoard.tiles.Count - 1].transform);
        spawnList.Add(gameBoard.tiles[(int)aBoardSize.y - 1].transform);

        return spawnList;
    }

    /// <summary>
    /// Fills List of enemy prefabs
    /// </summary>
    private void FillEnemiesList()
    {
        if (enemyPrefab1 != null)
            enemiesPrefabs.Add(enemyPrefab1);
        if (enemyPrefab2 != null)
            enemiesPrefabs.Add(enemyPrefab2);
        if (enemyPrefab3 != null)
            enemiesPrefabs.Add(enemyPrefab3);
        if (enemyPrefab4 != null)
            enemiesPrefabs.Add(enemyPrefab4);

        int enemiesCount = UnityEngine.Random.Range(15,20);

        for (int i = 0; i < enemiesCount; i++)
        {
            System.Random randomEnemy = new System.Random(Guid.NewGuid().GetHashCode());
            enemiesList.Add(enemiesPrefabs[randomEnemy.Next(enemiesPrefabs.Count)]);
        }
    }
}
