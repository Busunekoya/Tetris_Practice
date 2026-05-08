using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] MinoPrefabs = new GameObject[7];
    private GameObject currentMino;
    private GameObject nextMino;
    private GameObject holdMino;
    public Vector2 defaultMinoPosition;
    public bool playing = true;
    private float fallTime = 1f;
    private float fallTimer = 0f;
    void Start()
    {
        currentMino = Instantiate(MinoPrefabs[1], defaultMinoPosition, Quaternion.identity);
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
        nextMino = null;
    }
    void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentMino.transform.position += AddMinoMove(-1, 0);
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentMino.transform.position += AddMinoMove(1, 0);
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
            currentMino.transform.Rotate(0, 0, -90);
        }
    }
    Vector3 AddMinoMove(int x, int y)
    {
        return new Vector3(x, y, 0);
    }
    void MinoFall()
    {
        if(!playing)return;

        if(fallTimer >= fallTime)
        {
            currentMino.transform.position += AddMinoMove(0, -1);
            fallTimer = fallTimer % fallTime;
        }
        else
        {
            fallTimer += Time.deltaTime;
        }
    }
}
