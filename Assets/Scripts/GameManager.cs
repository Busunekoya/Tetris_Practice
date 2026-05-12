using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//参考: https://masavlog.com/programming/tetris/unity-tetris-3/
public class GameManager : MonoBehaviour
{
    public GameObject[] MinoPrefabs = new GameObject[7];
    private GameObject currentMino;
    private GameObject nextMino;
    private GameObject holdMino;
    public Transform NextMinoPos;
    public Vector2 defaultMinoPosition;
    public bool playing = true;
    private float fallTime = 1f;
    private float fallTimer = 0f;
    public int Xmin{get; set;}
    public int Xmax{get; set;}
    void Awake()
    {
        nextMino = Instantiate(MinoPrefabs[Random.Range(0, MinoPrefabs.Length)], defaultMinoPosition, Quaternion.identity);
        Xmin = Mathf.FloorToInt(defaultMinoPosition.x - 5);
        Xmax = Mathf.FloorToInt(defaultMinoPosition.x + 5);
    }
    void Start()
    {
        SetNextMino();
    }
    void Update()
    {
        MinoMovement();
        MinoRotation();
        MinoFall();
    }
    void SetNextMino()
    {
        if(currentMino != null)return;
        currentMino = nextMino;
        currentMino.transform.parent = null;
        currentMino.transform.position = defaultMinoPosition;
        nextMino = Instantiate(MinoPrefabs[Random.Range(0, MinoPrefabs.Length)], NextMinoPos);
    }
    void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(isMoveAble(currentMino, AddMinoMove(-1, 0)))currentMino.transform.position += AddMinoMove(-1, 0);
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(isMoveAble(currentMino, AddMinoMove(1, 0)))currentMino.transform.position += AddMinoMove(1, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            fallTime = 0.1f;
        }
        else
        {
            fallTime = 1f;
        }
    }
    void MinoRotation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(isRotateAble(currentMino, -90))currentMino.transform.Rotate(0, 0, -90);
        }
    }
    Vector3Int AddMinoMove(int x, int y)
    {
        return new Vector3Int(x, y, 0);
    }
    void MinoFall()
    {
        if(!playing)return;

        if(fallTimer >= fallTime)
        {
            if(isMoveAble(currentMino, AddMinoMove(0, -1)))currentMino.transform.position += AddMinoMove(0, -1);
            else
            {
                //ミノが落ちきったときの処理
                currentMino = null;
                SetNextMino();
            }
            fallTimer = fallTimer % fallTime;
        }
        else
        {
            fallTimer += Time.deltaTime;
        }
    }
    // Minoがステージ内に収まっているかの判定
    bool isMoveAble(GameObject minoObject, Vector3Int move)
    {
        Mino mino = minoObject.GetComponent<Mino>();
        if(mino == null)return false;
        else
        {
            int roundX = Mathf.RoundToInt(minoObject.transform.position.x);
            int roundY = Mathf.RoundToInt(minoObject.transform.position.y);
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(mino.minoType))
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle, minoBlock.position);
                if(roundX + rotatePosition.x + move.x < Xmin || roundX + rotatePosition.x + move.x >= Xmax || roundY + rotatePosition.y + move.y < -10)
                {
                    return false;
                }
            }
            return true;
        }
    }
    bool isRotateAble(GameObject minoObject, int angle)
    {
        Mino mino = minoObject.GetComponent<Mino>();
        if(mino == null)return false;
        else
        {
            int roundX = Mathf.RoundToInt(minoObject.transform.position.x);
            int roundY = Mathf.RoundToInt(minoObject.transform.position.y);
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(mino.minoType))
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle +angle, minoBlock.position);
                if(roundX + rotatePosition.x < Xmin || roundX + rotatePosition.x >= Xmax || roundY + rotatePosition.y < -10)
                {
                    return false;
                }
            }
            mino.AddAngle(angle);
            Debug.Log(mino.angle);
            return true;
        }
    }
}