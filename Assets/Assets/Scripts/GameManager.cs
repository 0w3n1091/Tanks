using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector2 boardSize;
    public Transform enemiesContainer;
    public GameBoard gameBoard;
    public TankControl playerTank;
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    public List<GameObject> powerUpsPrefabsList = new List<GameObject>();
    public List<Transform> powerUpsSpawnList = new List<Transform>();
    public List<GameObject> enemiesList = new List<GameObject>();
    public List<GameObject> currentEnemiesList = new List<GameObject>();
    public int powerUpDuration;

    [HideInInspector]
    public bool isPaused;

    [SerializeField]
    private int minEnemies;
    [SerializeField]
    private int maxEnemies;
    [SerializeField]
    private int enemySpawnDelay;
    [SerializeField]
    private int minPowerUps;
    [SerializeField]
    private int maxPowerUps;
    [SerializeField]
    private int powerUpsSpawnDelay;

    void Awake()
    {
        isPaused = false;
        Instance = this;
    }

    void Start()
    {
        playerTank.transform.position = new Vector3(gameBoard.tiles[0].transform.position.x,
                                                    gameBoard.tiles[0].transform.position.y + 0.21f,
                                                    gameBoard.tiles[0].transform.position.z);

        SpawnEnemies(boardSize);
        StartCoroutine(SpawnPowerUpAsync());
    }

    /// <summary>   
    /// Spawns enemies on the board
    /// </summary>
    private void SpawnEnemies(Vector2 aBoardSize)
    {
        FillEnemiesList();

        List<Transform> spawnPoints = GetEnemySpawnPoints(aBoardSize);

        StartCoroutine(SpawnEnemiesAsync(spawnPoints));
    }

    /// <summary>
    /// Spawns enemies asynchronically at given spawnPoints list
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemiesAsync(List<Transform> aSpawnPoints)
    {
        int spawnPointCounter = 0;
        for (int i = 0; i < enemiesList.Count; i++)
        {   
            
            currentEnemiesList.Add(Instantiate(enemiesList[i], new Vector3(aSpawnPoints[spawnPointCounter].transform.position.x,
                                                    aSpawnPoints[spawnPointCounter].transform.position.y + 0.21f,
                                                    aSpawnPoints[spawnPointCounter].transform.position.z), Quaternion.identity));

            yield return new WaitForSeconds(enemySpawnDelay);

            spawnPointCounter++;

            if (spawnPointCounter == 3)
                spawnPointCounter = 0;
            
            if (isPaused)
                yield return new WaitForSeconds(powerUpDuration);
        }
    }

    /// <summary>
    /// Fills spawnList based on gameBoard Size
    /// </summary>
    private List<Transform> GetEnemySpawnPoints(Vector2 aBoardSize)
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
        int enemiesCount = UnityEngine.Random.Range(minEnemies, maxEnemies);

        for (int i = 0; i < enemiesCount; i++)
        {
            System.Random randomEnemy = new System.Random(Guid.NewGuid().GetHashCode());
            enemiesList.Add(enemiesPrefabs[randomEnemy.Next(enemiesPrefabs.Count)]);
        }
    }

    /// <summary>
    /// Asynchronically spawns random PowerUp at random position on the board
    /// </summary>
    private IEnumerator SpawnPowerUpAsync()
    {
        int powerUpsCount = UnityEngine.Random.Range(minPowerUps, maxPowerUps);
        for (int i = 0; i < powerUpsCount; i++)
        {
            GameObject powerUp = Instantiate(powerUpsPrefabsList[UnityEngine.Random.Range(0, powerUpsPrefabsList.Count)],
                        gameBoard.tiles[UnityEngine.Random.Range(0, gameBoard.tiles.Count)].transform);

            yield return new WaitForSeconds(powerUpsSpawnDelay);
            Destroy(powerUp);
        }
    }

    /// <summary>
    /// Coroutine which pauses enemies for powerUpDuration
    /// </summary>
    private IEnumerator TimePowerUpCoroutine()
    {
        isPaused = true;
        yield return new WaitForSeconds(powerUpDuration);
        isPaused = false;
    }

    /// <summary>
    /// Coroutine which gives player immunity for powerUpDuration
    /// </summary>
    private IEnumerator ImmunePowerUpCoroutine()
    {
        playerTank.isImmune = true;
        yield return new WaitForSeconds(powerUpDuration);
        playerTank.isImmune = false;
    }

    public void OnTimePowerUpCatch(GameObject aPowerUpObject)
    {
        Destroy (aPowerUpObject);

        StartCoroutine(TimePowerUpCoroutine());
    }

    public void OnWeaponPowerUpCatch(GameObject aPowerUpObject)
    {
        Destroy (aPowerUpObject);

        playerTank.weaponDamage++;
        playerTank.playerHealth++;
    }
    
    public void OnClearPowerUpCatch(GameObject aPowerUpObject)
    {
        Destroy (aPowerUpObject);

        foreach(GameObject enemy in currentEnemiesList)
            Destroy(enemy);
            
        currentEnemiesList.Clear();
    }

    public void OnImmunePowerUpCatch(GameObject aPowerUpObject)
    {
        Destroy(aPowerUpObject);

        StartCoroutine(ImmunePowerUpCoroutine());
    }
}
