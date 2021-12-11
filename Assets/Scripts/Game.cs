using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameBoard gameBoard;
    public Vector2 boardSize;
    
    void Awake()
    {
        gameBoard.Initialize(boardSize);
    }
}
