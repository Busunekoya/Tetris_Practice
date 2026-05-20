using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//参考: https://masavlog.com/programming/tetris/unity-tetris-3/
public class GameManager : MonoBehaviour
{
    public GameObject[] MinoPrefabs = new GameObject[7];
    private GameObject currentMino;
    private GameObject nextMino;
    private GameObject holdMino = null;
    public Transform NextMinoPos;
    public Transform HoldMinoPos;
    public Vector2 defaultMinoPosition;
    public bool playing = true;
    private bool holdable = true;
    private float rawfallTime = 1f;
    private float rawfalldownTime = 0.1f;
    private float fallTime = 1f;
    private float fallTimer = 0f;
    private Vector2Int firstTilePos = new Vector2Int(5, 18);
    private Vector2Int currentMinoPos;
    public SetTiles setTiles;
    public GameScoreManager gameScoreManager;
    [SerializeField]private Transform FallenMinos;
    void Awake()
    {
        if(gameScoreManager == null)Debug.LogError("GameScoreManager not found");
        nextMino = Instantiate(MinoPrefabs[Random.Range(0, MinoPrefabs.Length)], defaultMinoPosition, Quaternion.identity);
    }
    void Start()
    {
        SetNextMino();
    }
    void Update()
    {
        if(!playing)return;
        MinoMovement();
        MinoRotation();
        MinoFall();
    }
    /// <summary>
    /// 次のミノを設定する
    /// </summary>
    void SetNextMino()
    {
        if(currentMino != null)return;
        SetCurrentMino(nextMino);
        nextMino = Instantiate(MinoPrefabs[Random.Range(0, MinoPrefabs.Length)], NextMinoPos);
    }
    void SetCurrentMino(GameObject mino)
    {
        currentMino = mino;
        currentMino.transform.parent = null;
        currentMinoPos = firstTilePos;
        currentMino.transform.position = setTiles.tilePosition(currentMinoPos);

        if (!isMoveAble(Vector2Int.zero))
        {
            GameOver();
        }
    }
    void GameOver()
    {
        playing = false;
        Debug.Log("Game Over");
    }
    void SwitchMino()
    {
        if(!holdable || currentMino == null) return;
        holdable = false;
        GameObject tempMino = holdMino;
        holdMino = currentMino;
        holdMino.transform.SetParent(HoldMinoPos);
        holdMino.transform.position = HoldMinoPos.TransformPoint(Vector3.zero);
        currentMino = tempMino;
        if(currentMino != null)
        {
            SetCurrentMino(currentMino);
        }
        else
        {
            SetNextMino();
        }
    }
    /// <summary>
    /// ミノの移動
    /// </summary>
    void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(isMoveAble(new Vector2Int(-1,0)))
            {
                currentMino.transform.position = setTiles.tilePosition(currentMinoPos);
            }
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(isMoveAble(new Vector2Int(1, 0)))
            {
                currentMino.transform.position = setTiles.tilePosition(currentMinoPos);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            fallTime = rawfalldownTime * Mathf.Pow(0.9f, gameScoreManager.level - 1);
        }
        else
        {
            fallTime = rawfallTime * Mathf.Pow(0.9f, gameScoreManager.level - 1);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchMino();
        }
    }
    /// <summary>
    /// ミノの回転
    /// </summary>
    void MinoRotation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(isRotateAble(-90))currentMino.transform.Rotate(0, 0, -90);
        }
    }
    void MinoFall()
    {
        if(!playing)return;

        if(fallTimer >= fallTime)
        {
            if(isMoveAble(new Vector2Int(0, -1)))
            {
                currentMino.transform.position = setTiles.tilePosition(currentMinoPos);
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    gameScoreManager.AddScore(10);
                }
            }
            else
            {
                AddToTile(currentMino);
                CheckLines();
                //ミノが落ちきったときの処理
                currentMino = null;
                SetNextMino();
                if(!holdable)
                {
                    holdable = true;
                }
            }
            fallTimer = fallTimer % fallTime;
        }
        else
        {
            fallTimer += Time.deltaTime;
        }
    }
    // Minoがステージ内に収まっているかの判定
    bool isMoveAble(Vector2Int move)
    {
        Mino mino = currentMino.GetComponent<Mino>();
        if(mino == null)return false;
        else
        {
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(mino.minoType))
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle, minoBlock.position);
                Vector2Int blockPos = rotatePosition + currentMinoPos + move;
                if(!ValidMovement(blockPos))
                {
                    return false;
                }
            }
            for(int i = 0; i < MinoBlock.MinoTypeToBlocks(mino.minoType).Length; i++)
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle, MinoBlock.MinoTypeToBlocks(mino.minoType)[i].position);
                Vector2Int blockPos = rotatePosition + currentMinoPos + move;// - minPos;
                mino.positions[i] = blockPos;
            }
            currentMinoPos += move;
            return true;
        }
    }
    /// <summary>
    /// 行が揃ったかを確かめる
    /// </summary>
    private void CheckLines()
    {
        bool isDelete = false;
        int DeleteLineNum = 0;
        for(int i = 0; i < setTiles.ySize; i++)
        {
            if (HasLine(i))
            {
                isDelete = true;
                DeleteLine(i);
                RowDown(i);
                i--;
                DeleteLineNum++;
            }
        }
        if (isDelete)
        {
            //Debug.Log(DeleteLineNum);
            gameScoreManager.AddScore(DelLineScore(DeleteLineNum));
            DeleatEmptyObject();
        }
    }
    private int DelLineScore(int lineCount)
    {
        if(lineCount <= 0)return 0;
        else
        {
            return 1000 * lineCount + 100 * total(lineCount);
        }
        int total(int count)
        {
            int result = 0;
            for(int i = 0; i < count; i++)
            {
                result += i;
            }
            return result;
        }
    }
    /// <summary>
    /// 指定された行にブロックが並んでいるかを確認する
    /// </summary>
    /// <param name="y">確認する行</param>
    /// <returns></returns>
    private bool HasLine(int y)
    {
        for(int x = 0; x < setTiles.xSize; x++)
        {
            if(setTiles.tiles[x, y].tile == null)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 行を消す
    /// </summary>
    /// <param name="y">消す行</param>
    private void DeleteLine(int y)
    {
        for(int x = 0; x < setTiles.xSize; x++)
        {
            Destroy(setTiles.tiles[x, y].tile);
            setTiles.tiles[x, y].tile = null;
        }
    }
    private void DeleatEmptyObject()
    {
        foreach(Transform child in FallenMinos)
        {
            //Debug.Log($"Deleting child: {child.name},{child.childCount}");
            if(child.childCount <= 0)
            {
                Destroy(child.gameObject);
            }
        }
    }
    /// <summary>
    /// 行を下にずらす
    /// </summary>
    /// <param name="y">消した行</param>
    private void RowDown(int y)
    {
        for(int i = 0; i < setTiles.xSize; i++)
        {
            for(int j = y; j < setTiles.ySize-1; j++)
            {
                setTiles.tiles[i,j].tile = setTiles.tiles[i,j+1].tile;
                setTiles.tiles[i,j+1].tile = null;
                if(setTiles.tiles[i,j].tile != null)setTiles.tiles[i,j].tile.transform.position = setTiles.tilePosition(i, j); //tiles[i,j].transform.position;
            }
        }
    }
    /// <summary>
    /// 回転可能か確かめる
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <returns></returns>
    bool isRotateAble(int angle)
    {
        Mino mino = currentMino.GetComponent<Mino>();
        if(mino == null)return false;
        else
        {
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(mino.minoType))
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle +angle, minoBlock.position);
                Vector2Int blockPos = rotatePosition + currentMinoPos;
                if(!ValidMovement(blockPos))
                {
                    return false;
                }
            }

            mino.AddAngle(angle);

            for(int i = 0; i < MinoBlock.MinoTypeToBlocks(mino.minoType).Length; i++)
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle, MinoBlock.MinoTypeToBlocks(mino.minoType)[i].position);
                Vector2Int blockPos = rotatePosition + currentMinoPos;
                mino.positions[i] = blockPos;
            }
            return true;
        }
    }
    /// <summary>
    /// 指定された位置にミノを移動できるかを確認する
    /// </summary>
    /// <param name="blockPos">確認する位置</param>
    /// <returns></returns>
    bool ValidMovement(Vector2Int blockPos)
    {
        if(blockPos.x < 0 || blockPos.x >= setTiles.xSize || blockPos.y < 0 )return false;
        if(setTiles.tiles[blockPos.x, blockPos.y].tile != null)return false;
        else return true;
    }
    void AddToTile(GameObject minoObject)
    {
        Mino mino = minoObject.GetComponent<Mino>();
        if(mino == null)
        {
            Debug.LogError("Mino component not found");
            return;
        }
        for(int i = 0; i < mino.transform.childCount; i++)
        {
            setTiles.tiles[mino.positions[i].x,mino.positions[i].y].FillTile(mino.transform.GetChild(i).gameObject);
        }
        minoObject.transform.SetParent(FallenMinos);
    }
}