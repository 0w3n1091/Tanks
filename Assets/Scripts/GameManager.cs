using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameSpeed = 1f;
    public float bulletSpeed = 1f;
    public float shootSpeed = 1f;

    void Awake()
    {
        instance = this;
    }
}
