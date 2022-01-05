using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Rigidbody enemyRB;
    public Vector3 turnDirection = Vector3.forward;
    public List<Vector3> availableDirections = new List<Vector3>();
    private GameBoard board;
    private GameTile currentTile;
    
    void Start()
    {
        board = GameObject.Find("GameBoard").GetComponent<GameBoard>();
        SetCurrentTile();
        GetDirectionAsync();
    }

    void Update()
    {
        SetCurrentTile();
        EnemyMove();
    }

    /// <summary>
    /// Moves enemy on the board
    /// </summary>
    public void EnemyMove()
    {
        transform.position += turnDirection * 0.003f * GameManager.instance.gameSpeed;
        //enemyRB.AddForce(turnDirection * GameManager.instance.gameSpeed);
    }

    /// <summary>
    /// Sets current GameTile based on Enemy position
    /// </summary>
    private void SetCurrentTile()
    {
        foreach (GameTile tile in board.tiles)
        {
            if((Math.Round(transform.position.x, 1) <= Math.Round(tile.transform.position.x, 1) + GameTile.tileWidth && 
                Math.Round(transform.position.x, 1) >= Math.Round(tile.transform.position.x, 1) - GameTile.tileWidth) && 
                (Math.Round(transform.position.z, 1) <= Math.Round(tile.transform.position.z, 1) + GameTile.tileWidth && 
                Math.Round(transform.position.z, 1) >= Math.Round(tile.transform.position.z, 1) - GameTile.tileWidth))
            {
                currentTile = tile;
                SetAvailableDirections();
                break;
            }
        }
    }

    /// <summary>
    /// Fills List with available directions for currentTile
    /// </summary>
    private void SetAvailableDirections()
    {
        availableDirections.Clear();
        
        if (currentTile.east != null)
            availableDirections.Add(Vector3.right);
        if (currentTile.west != null)
            availableDirections.Add(Vector3.left);
        if (currentTile.north != null)
            availableDirections.Add(Vector3.forward);
        if (currentTile.north != null)
            availableDirections.Add(Vector3.back);
    }

    /// <summary>
    /// Asynchronically gets new direction every 5 seconds
    /// </summary>
    /// <returns></returns>
    private async Task GetDirectionAsync()
    {
        GetDirection();
        await Task.Delay(5000);
        GetDirectionAsync();
    }

    /// <summary>
    /// Randomly drawns new direction from availableDirections List of currentTile
    /// </summary>
    private void GetDirection()
    {
        System.Random random = new System.Random();
        turnDirection = availableDirections[random.Next(availableDirections.Count)];

        RotateEnemy();
    }

    /// <summary>
    /// Rotates enemy based on turnDirection
    /// </summary>
    private void RotateEnemy()
    {
        if (turnDirection == Vector3.forward)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); 
        if (turnDirection == Vector3.back)
            transform.rotation = Quaternion.Euler(180f, 0f, 180f);
        if (turnDirection == Vector3.left)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        if (turnDirection == Vector3.right)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
