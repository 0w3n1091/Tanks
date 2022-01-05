using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameBoard gameBoard;
    public TankControl playerTank;
    public Vector2 boardSize;
    
    void Awake()
    {
        gameBoard.Initialize(boardSize);
        playerTank.transform.position = new Vector3(gameBoard.tiles[0].transform.position.x,
                                                    gameBoard.tiles[0].transform.position.y + 0.21f, 
                                                    gameBoard.tiles[0].transform.position.z);
    }
}
