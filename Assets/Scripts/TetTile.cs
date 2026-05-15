using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetTile : MonoBehaviour
{
    /// <summary>
    /// タイルの座標
    /// </summary>
    public Vector2Int position;
    //public bool isFilled{get;set;} = false;
    public GameObject tile{get;set;} = null;
    public void FillTile(GameObject hasTile)
    {
        if(tile != null)Debug.LogWarning($"Tile {position} is already filled.");
        //isFilled = true;
        tile = hasTile;
    }
    public void ClearTile()
    {
        if(tile == null)Debug.LogWarning($"Tile {position} is already clear.");
        tile = null;
        //isFilled = false;
    }
}
