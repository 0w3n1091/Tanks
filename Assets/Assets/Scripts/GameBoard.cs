using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameTile tilePrefab;
    public GameObject enemyPrefab1;
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    public List<GameTile> tiles = new List<GameTile>();
    public Transform ground = default;
    public int leftBottomIndex, leftTopIndex, rightBottomIndex, rightTopIndex;
    
    
    /// <summary>
    /// Inicjalizuje pole do gry o podanej powierzchni
    /// </summary>
    /// <param name="aSize"> Podany rozmiar pola gry</param>
    public void Initialize(Vector2 aSize)
    {
        FillEnemiesList();

        aSize = ValidateSize(aSize);
        ground.localScale = new Vector3(aSize.x, aSize.y, 1f);
        
        InstantiateTiles(aSize);
        SetNeighbours(aSize);

        SpawnEnemies(aSize, 1);
    }

    /// <summary>
    /// Fills List of enemy prefabs
    /// </summary>
    private void FillEnemiesList()
    {
        enemiesPrefabs.Add(enemyPrefab1);
    }

    /// <summary>
    /// Sprawdza czy pole gry ma wymagane minimalne rozmiary 
    /// </summary>
    /// <param name="aSize"></param>
    /// <returns>Sprawdzony rozmiar pola gry</returns>
    Vector2 ValidateSize(Vector2 aSize) 
    {
		if (aSize.x < 2) 
			aSize.x = 2;
		
		if (aSize.y < 2) 
			aSize.y = 2;

        return aSize;
	}

    /// <summary>
    /// Instancjonuje teksturę podłogi planszy
    /// </summary>
    void InstantiateTiles(Vector2 aSize)
    {
        for (float i = ((-aSize.x) / 2 + 0.5f); i <= aSize.x / 2 - 0.5f; i++)
        {
            for (float j = ((-aSize.y) / 2 + 0.5f); j <= aSize.y / 2 - 0.5f; j++)
            {
                GameTile tile = Instantiate(tilePrefab);
                tile.transform.SetParent(ground.transform);
                tile.transform.position = new Vector3(i, 0, j);
                tiles.Add(tile);
                tile.index = tiles.Count - 1;
                SetCornersIndex(tile, aSize, tiles.Count - 1, i, j);
            }
        }
    }

    /// <summary>
    /// Ustawia Indexy skrajnych pozycji na planszy
    /// </summary>
    void SetCornersIndex(GameTile aTile, Vector2 aSize, int aIndex, float aX, float aY)
    {
        //lewy dolny
        if (aX == ((-aSize.x) / 2 + 0.5f) && aY == ((-aSize.y) / 2 + 0.5f))
            leftBottomIndex = aIndex;

        //lewy górny
        if (aX == ((-aSize.x) / 2 + 0.5f) && aY == aSize.y / 2 - 0.5f)
            leftTopIndex = aIndex;   

        //prawy dolny
        if (aX == aSize.x / 2 - 0.5f && aY == ((-aSize.y) / 2 + 0.5f))
            rightBottomIndex = aIndex;

        //prawy górny
        if (aX == aSize.x / 2 - 0.5f && aY == aSize.y / 2 - 0.5f)
            rightTopIndex = aIndex;
    }

    /// <summary>
    /// Ustawia referencje sąsiadów dla każdego Tile'a
    /// </summary>
    /// <param name="aSize"></param>
    void SetNeighbours(Vector2 aSize)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (i == leftBottomIndex)
            {
                tiles[i].east = tiles[i + (int)aSize.x];
                tiles[i].north = tiles[i + 1];
            }
            if (i == leftTopIndex)
            {
                tiles[i].east = tiles[i + (int)aSize.x];
                tiles[i].south = tiles[i - 1];
            }
            if (i == rightBottomIndex)
            {
                tiles[i].west = tiles[i - (int)aSize.x];
                tiles[i].north = tiles[i + 1];
            }
            if (i == rightTopIndex)
            {
                tiles[i].west = tiles[i - (int)aSize.x];
                tiles[i].south = tiles[i - 1];
            }

            //lewa krawędź
            if (i < aSize.x && (i != leftTopIndex && i != leftBottomIndex))
            {
                tiles[i].east = tiles[i + (int)aSize.x];
                tiles[i].north = tiles[i + 1];
                tiles[i].south = tiles[i - 1]; 
            }
            //prawa krawędź
            if (i > (tiles.Count - aSize.x) && (i != rightTopIndex && i != rightBottomIndex))
            {
                tiles[i].west = tiles[i - (int)aSize.x];
                tiles[i].north = tiles[i + 1];
                tiles[i].south = tiles[i - 1]; 
            }
            //górna krawędź
            if ((i + 1) % aSize.x == 0 && (i != leftTopIndex && i != rightTopIndex) && i != 0)
            {
                tiles[i].west = tiles[i - (int)aSize.x];
                tiles[i].east = tiles[i + (int)aSize.x];
                tiles[i].south = tiles[i - 1];
            }
            //dolna krawędź
            if (i % aSize.x == 0 && (i != leftBottomIndex && i != rightBottomIndex) && i != 0)
            {
                tiles[i].west = tiles[i - (int)aSize.x];
                tiles[i].east = tiles[i + (int)aSize.x];
                tiles[i].north = tiles[i + 1];
            }
        }

        //cała reszta
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].west == null && tiles[i].east == null && tiles[i].north == null && tiles[i].south == null)
            {
                tiles[i].west = tiles[i - (int)aSize.x];
                tiles[i].east = tiles[i + (int)aSize.x];
                tiles[i].north = tiles[i + 1];
                tiles[i].south = tiles[i - 1];
            }
        }
    }
    
    /// <summary>   
    /// Spawns enemies at random position on the map
    /// </summary>
    private void SpawnEnemies(Vector2 aBoardSize, int aEnemiesCount)
    {
        List<Transform> spawnList = new List<Transform>();

        spawnList.Add(tiles[(int)aBoardSize.y - 1].transform);
        spawnList.Add(tiles[tiles.Count - 1].transform);
        spawnList.Add(tiles[((int)aBoardSize.y - 1) + ((int)aBoardSize.y * ((int)aBoardSize.x - 1) / 2)].transform);

        for (int i = 0; i < aEnemiesCount; i++)
        {
            System.Random randomEnemy = new System.Random(Guid.NewGuid().GetHashCode());
            System.Random randomPosition = new System.Random(Guid.NewGuid().GetHashCode());

            Vector3 spawnPosition = new Vector3(spawnList[randomPosition.Next(spawnList.Count)].transform.position.x,
                                                spawnList[randomPosition.Next(spawnList.Count)].transform.position.y + 0.21f,
                                                spawnList[randomPosition.Next(spawnList.Count)].transform.position.z);

            Instantiate(enemiesPrefabs[randomEnemy.Next(enemiesPrefabs.Count)], spawnPosition, Quaternion.identity);
        }
    }
}
