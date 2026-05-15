using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetTile : MonoBehaviour
{
    /// <summary>
    /// 保持中のタイル
    /// </summary>
    public GameObject tile{get;set;} = null;
    public void FillTile(GameObject hasTile)
    {
        if(tile != null)Debug.LogWarning($"Tile {this.transform.position} is already filled.");
        //isFilled = true;
        tile = hasTile;
    }
    public void ClearTile()
    {
        if(tile == null)Debug.LogWarning($"Tile {this.transform.position} is already clear.");
        tile = null;
        //isFilled = false;
    }
}
