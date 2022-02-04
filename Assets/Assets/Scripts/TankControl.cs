using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    public GameBoard board;
    public Rigidbody tankRB;
    public GameObject bulletPrefab;
    public Transform firePoint;
   
    public int playerHealth;
    public int shootDelay;
    public int weaponDamage;
    public float moveSpeed;
    public bool isImmune;

    private GameTile currentTile;
    private Vector3 turnDirection = Vector3.forward;
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

        tankRB.velocity = movementDirection * moveSpeed;
    }

    /// <summary>
    /// Forbids player from shooting again for time declared in GameManager
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootingDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        isShooting = false;
    }

    /// <summary>
    /// Instantiates bullet at firePoint position and starts counting shooting delay time
    /// </summary>
    private void PlayerShoot()
    {
        isShooting = true;
        StartCoroutine(ShootingDelay());
        GameObject playerBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
        playerBullet.GetComponent<BulletControl>().direction = turnDirection;
    }

    /// <summary>
    /// Substract playerHealth after enemy hit
    /// </summary>
    private void PlayerHit()
    {
        playerHealth--;
        weaponDamage--;

        if (playerHealth <= 0)
            PlayerDeath();
    }

    /// <summary>
    /// Kills player
    /// </summary>
    private void PlayerDeath()
    {
        Destroy(this.gameObject);
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

    private void OnCollisionEnter(Collision aCollision) 
    {
        if (aCollision.collider.name != "Collider" && aCollision.gameObject.tag == "Enemy")
        {
            aCollision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }

        if (aCollision.gameObject.tag == "BulletEnemy")
        {
            PlayerHit();
        }
    }

    private void OnCollisionExit(Collision aCollision)
    {
        if (aCollision.collider.name != "Collider" && aCollision.gameObject.tag == "Enemy")
        {
            aCollision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnTriggerEnter(Collider aCollider)
    {
        if (aCollider.gameObject.tag == "WeaponPowerUp")
            GameManager.Instance.OnWeaponPowerUpCatch(aCollider.gameObject);

        if (aCollider.gameObject.tag == "TimePowerUp")
            GameManager.Instance.OnTimePowerUpCatch(aCollider.gameObject);

        if (aCollider.gameObject.tag == "ClearPowerUp")
            GameManager.Instance.OnClearPowerUpCatch(aCollider.gameObject);
        
        if (aCollider.gameObject.tag == "ImmunePowerUp")
            GameManager.Instance.OnImmunePowerUpCatch(aCollider.gameObject);
    }
}
