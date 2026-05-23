using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public MinoType minoType;
    public int angle{get;set;} = 0;
    public Vector2Int[] positions{get;set;} = new Vector2Int[4];
    public void AddAngle(int addAngle, Vector2Int currentMinoPos,SetTiles setTiles)
    {
        angle = (360 + angle + addAngle) % 360;

        SetChildPos(currentMinoPos,setTiles);
    }
    public void SetChildPos(Vector2Int currentMinoPos,SetTiles setTiles)
    {
        transform.position = setTiles.tilePosition(currentMinoPos);
        for(int i = 0; i < 4; i++)
        {
            Vector2Int rotatePos = Mino.Rotate(angle, MinoBlock.MinoTypeToBlocks(minoType)[i].position);
            positions[i] = rotatePos + currentMinoPos;
            transform.GetChild(i).position = setTiles.tilePosition(positions[i]);
        }
    }
    public void SetTransformPos(Transform parentTransform)
    {
        transform.parent = parentTransform;
        transform.position = parentTransform.TransformPoint(Vector2.zero);

        for(int i = 0; i < 4; i++)
        {
            transform.GetChild(i).position = parentTransform.TransformPoint(Vector2IntToVector2(MinoBlock.MinoTypeToBlocks(minoType)[i].position));
        }
    }
    Vector2 Vector2IntToVector2(Vector2Int vector2Int)
    {
        return new Vector2(vector2Int.x,vector2Int.y);
    }
    /// <summary>
    /// ミノブロックの座標を回転させる関数
    /// </summary>
    /// <param name="angle">弧度法で指定する</param>
    /// <param name="vector2Int">回転対象の座標</param>
    /// <returns></returns>
    public static Vector2Int Rotate(int angle,Vector2Int vector2Int)
    {
        return new Vector2Int(
            Mathf.RoundToInt(vector2Int.x * Mathf.Cos(Mathf.Deg2Rad*angle) - vector2Int.y * Mathf.Sin(Mathf.Deg2Rad*angle)),
            Mathf.RoundToInt(vector2Int.x * Mathf.Sin(Mathf.Deg2Rad*angle) + vector2Int.y * Mathf.Cos(Mathf.Deg2Rad*angle))
        );
    }
}
/// <summary>
/// ミノの種類
/// </summary>
public enum MinoType
{
    I=0, O, S, Z, J, L, T
}
public class MinoBlock
{
    /// <summary>
    /// ミノブロックの位置。ミノの中心を(0, 0)としたときの相対座標
    /// </summary>
    public Vector2Int position{get;set;}
    public MinoBlock(int x, int y)
    {
        position = new Vector2Int(x, y);
    }
    /// <summary>
    /// ミノの種類からミノブロックの配列を返す
    /// </summary>
    /// <param name="minoType">ミノの種類</param>
    /// <returns></returns>
    public static MinoBlock[] MinoTypeToBlocks(MinoType minoType)
    {
        switch (minoType)
        {
            case MinoType.I:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(1, 0), new MinoBlock(2, 0), new MinoBlock(-1, 0) };
            case MinoType.O:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(1, 0), new MinoBlock(1, 1), new MinoBlock(0, 1) };
            case MinoType.S:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(0, 1), new MinoBlock(-1, 0), new MinoBlock(1, 1) };
            case MinoType.Z:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(0, 1), new MinoBlock(1, 0), new MinoBlock(-1, 1) };
            case MinoType.J:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(-1, 0), new MinoBlock(1, 0), new MinoBlock(-1, 1) };
            case MinoType.L:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(-1, 0), new MinoBlock(1, 0), new MinoBlock(1, 1) };
            case MinoType.T:
                return new MinoBlock[] { new MinoBlock(0, 0), new MinoBlock(1, 0), new MinoBlock(-1, 0), new MinoBlock(0, 1) };
            default:
                return null;
        }
    }
}