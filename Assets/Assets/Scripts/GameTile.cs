using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    public GameTile east, west, north, south;
    public int index;
    public bool hideNeighbours = false;
    public bool showNeighbours = false;
    public const float tileWidth = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        if (hideNeighbours)
        {
            hideNeighbours = false;
            if (east != null)
                east.gameObject.SetActive(false);
            if (west != null)
                west.gameObject.SetActive(false);
            if (north != null)
                north.gameObject.SetActive(false);
            if (south != null)
                south.gameObject.SetActive(false);
        }
        if (showNeighbours)
        {
            showNeighbours = false;
            if (east != null)
                east.gameObject.SetActive(true);
            if (west != null)
                west.gameObject.SetActive(true);
            if (north != null)
                north.gameObject.SetActive(true);
            if (south != null)
                south.gameObject.SetActive(true);
        }
    }
}
