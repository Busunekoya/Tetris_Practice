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
    // Start is called before the first frame update
    void Start()
    {
        //ariaObjectにこのゲームオブジェクトのTransformを代入
        ariaObject = this.gameObject.transform;
        //タイルを10x20の範囲で生成
        for(int x = 0; x < 10; x++)
        {
            for(int y = 0; y < 20; y++)
            {
                GameObject tile = Instantiate(tilePrefab, ariaObject.TransformPoint(-5+x, -10+y, 0), Quaternion.identity);
                tile.transform.parent = ariaObject;
                tile.GetComponent<TetTile>().position = new Vector2Int(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
