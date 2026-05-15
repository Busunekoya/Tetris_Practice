using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class SetTiles : MonoBehaviour
{
    /// <summary>
    /// タイルのプレハブ
    /// </summary>
    [SerializeField] private GameObject tilePrefab;
    private Transform ariaObject;
    public int xSize{get;set;} = 10;
    public int ySize{get;set;} = 20;
    public TetTile[,] tiles{get;set;}
    void Awake()
    {
        tiles = new TetTile[xSize, ySize];
        //ariaObjectにこのゲームオブジェクトのTransformを代入
        ariaObject = this.gameObject.transform;
        //タイルをxSize x ySizeの範囲で生成
        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                GameObject tile = Instantiate(tilePrefab, ariaObject.TransformPoint(x-(xSize/2), y-(ySize/2), 0), Quaternion.identity);
                tile.transform.parent = ariaObject;
                tiles[x, y] = tile.GetComponent<TetTile>();
            }
        }
    }
    public Vector2 tilePosition(Vector2Int pos)
    {
        return tilePosition(pos.x,pos.y);
    }
    public Vector2 tilePosition(int x, int y)
    {
        return tiles[x, y].transform.position;
    }
}
