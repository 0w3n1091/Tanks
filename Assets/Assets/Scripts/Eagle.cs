using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Eagle : MonoBehaviour
{
    private void OnCollisionEnter(Collision aCollision)
    {
        if (aCollision.gameObject.tag == "BulletPlayer" || aCollision.gameObject.tag == "BulletEnemy")
        {
            SceneManager.LoadScene("Game");
        }
    }
}
