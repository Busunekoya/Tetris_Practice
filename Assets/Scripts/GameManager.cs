using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//参考: https://masavlog.com/programming/tetris/unity-tetris-3/
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// テトリスのミノを入れる配列
    /// </summary>
    public GameObject[] MinoPrefabs;
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
    private GameObject holdMino;
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
    public bool playing;
    /// <summary>
    /// 保持可能か
    /// </summary>
    private bool holdable;
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
    void Awake()
    {
        //ゲームスコアマネージャーの存在を確認する

        //nextMinoを設定する

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




    }
    /// <summary>
    /// currentMinoがnullであることを確認し、SetCurrentMinoメソッドを動かす
    /// nextMinoはMinoPrefabsの中からランダムに選択されるようにし、座標はNextMinoPosにする
    /// nextMinoのMinoコンポーネントからSetTransformPosメソッドを動かし、NextMinoPos
    /// </summary>
    void SetNextMino()
    {
        //currentMinoがnullであることを確認

        //SetCurrentMinoメソッドを動かす

        //nextMinoはMinoPrefabsの中からランダムに選択されるようにし、親はNextMinoPosにする

        //nextMinoのMinoコンポーネントからSetTransformPosメソッドを動かし、NextMinoPosの座標を基準にミノを設定する

    }
    void SetCurrentMino(GameObject mino)
    {
        //currentMinoを引数のminoとする

        //currentMinoComponentをcurrentMinoに再設定する

        //currentMinoの親を外す

        //currentMinoの座標を初期位置とする

        //MinoComponentより、SetChildPosコンポーネントを動かし、setTilesのfirstTilePosの位置を基準とする

        //もし動かせないのなら、ゲームオーバーとする




    }
    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    void GameOver()
    {
        //playingをfalseとする

        Debug.Log("Game Over");
    }
    //ミノを交代する
    void SwitchMino()
    {
        //holdableがtrueであり、currentMinoがnullではないことを確認する

        //holdableをfalseにする(何回も保持を変更することを防ぐため)

        //holdMinoとcurrentMinoを入れ替える




        //currentMinoがあるのなら、SetCurrentMinoで再設定し、ないのなら次のミノを設定する








    }
    /// <summary>
    /// ミノの移動
    /// </summary>
    void MinoMovement()
    {
        //左右キーが押された時、それぞれ左右に1マスずつ動かす。ただし、どのブロックもはみ出さないようにする
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

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
            
        }
    }
    /// <summary>
    /// ミノの落下
    /// </summary>
    void MinoFall()
    {
        //プレイ中であることを確認する

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
            else    //下方向に動かせないときの処理
            {
                //ミノをTileに追加するメソッドを動かす
                AddToTile(currentMino);
                //揃ったレーンがあるか調べる
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
            //fallTimerの値をfallTimeを超えないぐらいに再設定する
            fallTimer = fallTimer % fallTime;
        }
        else
        {
            //fallTimerの値にdeltaTimeを足す
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
        //currentMinoComponentがあることを確認する
        if(currentMinoComponent == null)return false;
        else
        {
            //すべてのミノブロックに対し動かせるかを確認する
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(currentMinoComponent.minoType))
            {

                //blockPosに引数分足した時の座標を調べる

                //範囲内か調べ、範囲外だった時falseを返す




            }
            //returnしなかったとき、動かせることを示すため、引数分動かす

            //位置を更新する
            currentMinoComponent.SetChildPos(currentMinoPos,setTiles);
            //trueを返す
            return true;
        }
    }
    /// <summary>
    /// 行が揃ったかを確かめる
    /// </summary>
    private void CheckLines()
    {
        //消したレーン数を数えるint型
        int DeleteLineNum = 0;
        //0行目からy最大サイズまで繰り返す
        for(int i = 0; i < setTiles.ySize; i++)
        {
            //もし、i行目がすべてそろっているのなら
            if (HasLine(i))
            {
                //i行目を消す
                DeleteLine(i);
                i--;
                //消したレーン数を増やす
                DeleteLineNum++;
            }
        }
        //消したレーンがあるのなら
        if (DeleteLineNum > 0)
        {
            //消したレーン数に応じて消す
            gameScoreManager.AddScore(DelLineScore(DeleteLineNum));
            //空のゲームオブジェクトを消す
            DeleteEmptyObject();
        }
    }
    //スコア計算
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
        //setTiles.tilesのy行目のタイルがすべてあるのならtrueを返し、1つでも抜けているのならfalseを返す
        return false;
    }
    /// <summary>
    /// 行を消す
    /// </summary>
    /// <param name="y">消す行</param>
    private void DeleteLine(int y)
    {
        //y行目のタイルを全て消す
        for(int x = 0; x < setTiles.xSize; x++)
        {
            //tileにあるゲームオブジェクトを削除する

            //tileのゲームオブジェクトを空にする

            //yよりも上のタイルを1段ずつ下にずらす





        }
    }
    //空のゲームオブジェクトを削除する
    private void DeleteEmptyObject()
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
        if(currentMinoComponent == null)return;
        else
        {
            foreach(MinoBlock minoBlock in MinoBlock.MinoTypeToBlocks(currentMinoComponent.minoType))
            {
                //回転後の座標を求める


                //回転後の座標が範囲内か調べ、範囲外なら処理を終了する




            }

            currentMinoComponent.AddAngle(angle, currentMinoPos, setTiles);
        }
    }
    /// <summary>
    /// 指定された位置にミノを移動できるかを確認する
    /// </summary>
    /// <param name="blockPos">確認する位置</param>
    /// <returns></returns>
    bool ValidMovement(Vector2Int blockPos)
    {


        return false;
    }
    //引数のミノの子オブジェクトをsetTilesに登録する
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
    //デバッグ用、文字列で現在のタイル状況を調べる
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