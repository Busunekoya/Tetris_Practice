using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//参考: https://masavlog.com/programming/tetris/unity-tetris-3/
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// テトリスのミノを入れる配列
    /// </summary>
    public GameObject[] MinoPrefabs = new GameObject[7];
    /// <summary>
    /// 動かすミノ
    /// </summary>
    private GameObject currentMino;
    /// <summary>
    /// currentMinoについているMinoComponent
    /// </summary>
    private Mino currentMinoComponent;
    /// <summary>
    /// 次のミノ
    /// </summary>
    private GameObject nextMino;
    /// <summary>
    /// ホールド中のミノ
    /// </summary>
    private GameObject holdMino = null;
    /// <summary>
    /// 次のミノを表示する場所
    /// </summary>
    public Transform NextMinoPos;
    /// <summary>
    /// ホールド中のミノを表示する場所
    /// </summary>
    public Transform HoldMinoPos;
    /// <summary>
    /// ゲーム中か
    /// </summary>
    public bool playing = true;
    /// <summary>
    /// 保持可能か
    /// </summary>
    private bool holdable = true;
    /// <summary>
    /// 落ちるまでの時間設定
    /// </summary>
    private float rawfallTime = 1f;
    /// <summary>
    /// 下降中に落ちるまでの時間設定
    /// </summary>
    private float rawfalldownTime = 0.1f;
    /// <summary>
    /// 落ちるまでの時間
    /// </summary>
    private float fallTime = 1f;
    /// <summary>
    /// 落ちるまでのタイマー
    /// </summary>
    private float fallTimer = 0f;
    /// <summary>
    /// 初期位置
    /// </summary>
    private Vector2Int firstTilePos = new Vector2Int(5, 18);
    /// <summary>
    /// 現在の位置
    /// </summary>
    private Vector2Int currentMinoPos;
    /// <summary>
    /// タイルのプログラム
    /// </summary>
    public SetTiles setTiles;
    /// <summary>
    /// スコアマネージャー
    /// </summary>
    public GameScoreManager gameScoreManager;
    /// <summary>
    /// 落ち切ったミノを保存するTransform
    /// </summary>
    [SerializeField]private Transform FallenMinos;
    /// <summary>
    /// 1. ゲームスコアマネージャーの存在を確認する
    /// 2. nextMinoを設定する
    /// </summary>
    void Awake()
    {
        if(gameScoreManager == null)Debug.LogError("GameScoreManager not found");
        nextMino = Instantiate(MinoPrefabs[Random.Range(0, MinoPrefabs.Length)], NextMinoPos);
    }
    /// <summary>
    /// SetNextMinoメソッドを動かす
    /// </summary>
    void Start()
    {
        SetNextMino();
    }
    /// <summary>
    /// もし、playing=trueならMinoMovement、MinoRotation、MinoFallメソッドを動かす
    /// </summary>
    void Update()
    {
        if(!playing)return;
        MinoMovement();
        MinoRotation();
        MinoFall();
    }
    /// <summary>
    /// currentMinoがnullであることを確認し、SetCurrentMinoメソッドを動かす
    /// nextMinoはMinoPrefabsの中からランダムに選択されるようにし、座標はNextMinoPosにする
    /// nextMinoのMinoコンポーネントからSetTransformPosメソッドを動かし、NextMinoPos
    /// </summary>
    void SetNextMino()
    {
        //currentMinoがnullであることを確認
        if(currentMino != null)return;
        //SetCurrentMinoメソッドを動かす
        SetCurrentMino(nextMino);
        //nextMinoはMinoPrefabsの中からランダムに選択されるようにし、親はNextMinoPosにする
        nextMino = Instantiate(MinoPrefabs[Random.Range(0, MinoPrefabs.Length)], NextMinoPos);
        //nextMinoのMinoコンポーネントからSetTransformPosメソッドを動かし、NextMinoPosの座標を基準にミノを設定する
        nextMino.GetComponent<Mino>().SetTransformPos(NextMinoPos);
    }
    void SetCurrentMino(GameObject mino)
    {
        //currentMinoを引数のminoとする
        currentMino = mino;
        //currentMinoComponentをcurrentMinoに再設定する
        currentMinoComponent = currentMino.GetComponent<Mino>();
        //currentMinoの親を外す
        currentMino.transform.parent = null;
        //currentMinoの座標を初期位置とする
        currentMinoPos = firstTilePos;
        //MinoComponentより、SetChildPosコンポーネントを動かし、setTilesのfirstTilePosの位置を基準とする
        currentMinoComponent.SetChildPos(firstTilePos, setTiles);
        //もし動かせないのなら、ゲームオーバーとする
        if (!isMoveAble(Vector2Int.zero))
        {
            GameOver();
        }
    }
    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    void GameOver()
    {
        //playingをfalseとする
        playing = false;
        Debug.Log("Game Over");
    }
    //ミノを交代する
    void SwitchMino()
    {
        //holdableがtrueであり、currentMinoがnullではないことを確認する
        if(!holdable || currentMino == null) return;
        //holdableをfalseにする(何回も保持を変更することを防ぐため)
        holdable = false;
        //holdMinoとcurrentMinoを入れ替える
        GameObject tempMino = holdMino;
        holdMino = currentMino;
        holdMino.GetComponent<Mino>().SetTransformPos(HoldMinoPos);
        currentMino = tempMino;
        //currentMinoがあるのなら、SetCurrentMinoで再設定し、ないのなら次のミノを設定する
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
        //左右キーが押された時、それぞれ左右に1マスずつ動かす。ただし、どのブロックもはみ出さないようにする
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isMoveAble(new Vector2Int(-1,0));
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isMoveAble(new Vector2Int(1, 0));
        }
        //下キーが押されているかどうかでそれぞれ対応する値に設定する
        if (Input.GetKey(KeyCode.DownArrow))
        {
            fallTime = rawfalldownTime * Mathf.Pow(0.9f, gameScoreManager.level - 1);
        }
        else
        {
            fallTime = rawfallTime * Mathf.Pow(0.9f, gameScoreManager.level - 1);
        }
        //スペースキーが押された時、保持ミノを入れ替える
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
        //上キーが押された時、-90°回転させる、ただし、はみ出すようであれば回転させない
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotateAble(-90);
        }
    }
    /// <summary>
    /// ミノの落下
    /// </summary>
    void MinoFall()
    {
        //プレイ中であることを確認する
        if(!playing)return;
        //もし、fallTimerの値がfallTimeよりも大きいならば、下方向に移動する
        if(fallTimer >= fallTime)
        {
            //下方向に移動する
            if(isMoveAble(new Vector2Int(0, -1)))
            {
                //もし、下キーが押されているのなら加点する
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    gameScoreManager.AddScore(10);
                }
            }
            else
            {
                //ミノをTileに追加するメソッドを動かす
                AddToTile(currentMino);
                CheckLines();
                //ミノが落ちきったときの処理
                currentMino = null;
                SetNextMino();
                if(!holdable)
                {
                    holdable = true;
                }
                //Debug.Log(TileString());
            }
            fallTimer = fallTimer % fallTime;
        }
        else
        {
            fallTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// Minoがステージ内に収まっているかの判定
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    bool isMoveAble(Vector2Int move)
    {
        //Mino mino = currentMino.GetComponent<Mino>();
        if(currentMinoComponent == null)return false;
        else
        {
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(currentMinoComponent.minoType))
            {
                Vector2Int rotatePosition = Mino.Rotate(currentMinoComponent.angle, minoBlock.position);
                Vector2Int blockPos = rotatePosition + currentMinoPos + move;
                if(!ValidMovement(blockPos))
                {
                    return false;
                }
            }

            currentMinoPos += move;
            currentMinoComponent.SetChildPos(currentMinoPos,setTiles);
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
                i--;
                DeleteLineNum++;
            }
        }
        if (isDelete)
        {
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
            for(int yr = y; yr < setTiles.ySize - 1; yr++)
            {
                setTiles.tiles[x,yr].UpdateTile(setTiles.tiles[x,yr +1].tile);
                setTiles.tiles[x,yr +1].tile = null;
            }
        }
    }
    private void DeleatEmptyObject()
    {
        foreach(Transform child in FallenMinos)
        {
            if(child.childCount <= 0)
            {
                Destroy(child.gameObject);
            }
        }
    }
    /// <summary>
    /// 回転可能か確かめる
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <returns></returns>
    void isRotateAble(int angle)
    {
        Mino mino = currentMino.GetComponent<Mino>();
        if(mino == null)return;// false;
        else
        {
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(mino.minoType))
            {
                Vector2Int rotatePosition = Mino.Rotate(mino.angle +angle, minoBlock.position);
                Vector2Int blockPos = rotatePosition + currentMinoPos;
                if(!ValidMovement(blockPos))
                {
                    return;
                }
            }

            mino.AddAngle(angle, currentMinoPos, setTiles);
            return;
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
    private string TileString()
    {
        string result = "";
        for(int y = 0; y < setTiles.ySize; y++)
        {
            for(int x = 0; x < setTiles.xSize; x++)
            {
                result += setTiles.tiles[x, y].tile != null ? "X" : "0";
            }
            result += "\n";
        }
        return result;
    }
}