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
    public TetTile[,] tiles{get;set;}// = new TetTile[xSize , ySize];
    // Start is called before the first frame update
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
                GameObject tile = Instantiate(tilePrefab, ariaObject.TransformPoint(-5+x, -10+y, 0), Quaternion.identity);
                tile.transform.parent = ariaObject;
                tile.GetComponent<TetTile>().position = new Vector2Int(x, y);
                tiles[x, y] = tile.GetComponent<TetTile>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
