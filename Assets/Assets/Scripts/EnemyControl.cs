using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Rigidbody enemyRB;
    public GameTile currentTile;
    private GameBoard board;
    public Vector3 turnDirection = Vector3.forward;
    public List<Vector3> availableDirections = new List<Vector3>();

    public int health;
    public int minDirectionChangeDelay;
    public int maxDirectionChangeDelay;

    public float moveSpeed;
    
    
    void Start()
    {
        board = GameObject.Find("GameBoard").GetComponent<GameBoard>();

        SetCurrentTile();
        
        StartCoroutine(SetDirectionAsync());
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
        if (GameManager.Instance.isPaused)
            enemyRB.velocity = Vector3.zero;
        else
            enemyRB.velocity = turnDirection * moveSpeed;
    }

    /// <summary>
    /// Sets current GameTile based on Enemy position
    /// </summary>
    private void SetCurrentTile()
    {
        foreach (GameTile tile in board.tiles)
        {
            if((Math.Round(transform.position.x, 1) < Math.Round(tile.transform.position.x, 1) + GameTile.tileWidth && 
                Math.Round(transform.position.x, 1) > Math.Round(tile.transform.position.x, 1) - GameTile.tileWidth) && 
                (Math.Round(transform.position.z, 1) < Math.Round(tile.transform.position.z, 1) + GameTile.tileWidth && 
                Math.Round(transform.position.z, 1) > Math.Round(tile.transform.position.z, 1) - GameTile.tileWidth))
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
        if (currentTile.south != null)
            availableDirections.Add(Vector3.back);
    }

    /// <summary>
    /// Asynchronically gets new direction
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetDirectionAsync()
    {
        while(true)
        {
            if (!GameManager.Instance.isPaused)
            {
                SetDirection();

                yield return new WaitForSeconds(UnityEngine.Random.Range(minDirectionChangeDelay, maxDirectionChangeDelay));
            }
            else
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// Randomly drawns new direction from availableDirections List of currentTile
    /// </summary>
    private void SetDirection()
    {
        System.Random random = new System.Random();
        turnDirection = availableDirections[random.Next(availableDirections.Count)];

        RotateEnemy();
    }

    /// <summary>
    /// Rotates enemy based on turnDirection
    /// </summary>
    public void RotateEnemy()
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

    /// <summary>
    /// Substract enemy health after player hit for given damage
    /// </summary>
    private void EnemyHit(int aDamage)
    {
        health -= aDamage;
        
        if (health == 0)    
            KillEnemy();
    }

    /// <summary>
    /// Kills enemy by destroying his gameObject
    /// </summary>
    private void KillEnemy()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Updates enemyRB constaints with given parameter
    /// </summary>
    private void UpdateRBConstraints(RigidbodyConstraints aConstraints)
    {
        enemyRB.constraints = aConstraints;
    }

    private void OnCollisionEnter(Collision aCollision)
    {
        if (aCollision.gameObject.tag == "Enemy" && aCollision.collider.name  != "Collider")
            UpdateRBConstraints(RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation);
        
        if (aCollision.gameObject.tag == "BulletPlayer")
            EnemyHit(aCollision.gameObject.GetComponent<BulletControl>().damage);
    }
    
    private void OnCollisionExit(Collision aCollision) 
    {
        if (aCollision.gameObject.tag == "Enemy")
            UpdateRBConstraints(RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation);
    }


}
