using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    public GameBoard board;
    public GameTile currentTile;
    public Rigidbody tankRB;
    public GameObject bulletPrefab;
    public Transform firePoint;
    [HideInInspector]
    public Vector3 turnDirection = Vector3.forward;

    private bool isShooting;

    void Start()
    {
        board = GameObject.Find("GameBoard").GetComponent<GameBoard>();
    }
    void Update()
    {
        PlayerMove();
        SetCurrentTile();
        
        if (Input.GetKey(KeyCode.Space) && !isShooting)
            PlayerShoot();
    }

    /// <summary>
    /// Changes player position by detected inputs
    /// </summary>
    private void PlayerMove()
    {
        Vector3 movementDirection = Vector3.zero;
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementDirection = Vector3.forward;
            turnDirection = Vector3.forward;

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);                             
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDirection = Vector3.back;
            turnDirection = Vector3.back;

            transform.rotation = Quaternion.Euler(180f, 0f, 180f);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection = Vector3.left;
            turnDirection = Vector3.left;

            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection = Vector3.right;
            turnDirection = Vector3.right;

            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        tankRB.velocity = movementDirection * GameManager.instance.gameSpeed * 2f;
    }

    /// <summary>
    /// Forbids player from shooting again for time declared in GameManager
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootingDelay()
    {
        yield return new WaitForSeconds(GameManager.instance.shootSpeed);
        isShooting = false;
    }

    /// <summary>
    /// Instantiates bullet at firePoint position and starts counting shooting delay time
    /// </summary>
    private void PlayerShoot()
    {
        isShooting = true;
        StartCoroutine("ShootingDelay");
        Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Sets current GameTile based on player position
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
                break;
            }
        }
    }

    /// <summary>
    /// Changes rigidbody constraints of enemy after collision
    /// </summary>
    private void OnCollisionEnter(Collision aCollision) 
    {
        if (aCollision.collider.name != "Collider" && aCollision.gameObject.tag == "Enemy")
        {
            aCollision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
