using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetTile : MonoBehaviour
{
    /// <summary>
    /// タイルの座標
    /// </summary>
    public Vector2Int position;
    public bool isFilled{get;set;} = false;
    public void FillTile()
    {
        if(isFilled)Debug.LogWarning($"Tile {position} is already filled.");
        isFilled = true;
    }
    public void ClearTile()
    {
        if(!isFilled)Debug.LogWarning($"Tile {position} is already clear.");
        isFilled = false;
    }
}
