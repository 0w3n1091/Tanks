using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    
    public Rigidbody bulletRB;
    private TankControl player;
    private Vector3 direction;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<TankControl>();

        direction = player.turnDirection;
        Destroy(gameObject, 3f);

    }

    void Update()
    {
        transform.position += direction * 0.01f * GameManager.instance.bulletSpeed * GameManager.instance.gameSpeed;
    }
    
    void OnCollisionEnter(Collision collision) 
    {
        //destroy bullet immediately after collision with another object
        Destroy(gameObject);
    }
}