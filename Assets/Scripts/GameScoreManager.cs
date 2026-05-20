using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    public int score{get;set;}= 0;
    public int level{get;set;}= 1;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;
    void Start()
    {
        ScoreText.text = score.ToString("d8");
        LevelText.text = level.ToString();
    }
    public void AddScore(int points)
    {
        score += points;
        ScoreText.text = score.ToString("d8");
        level = score / 10000 + 1;
        LevelText.text = level.ToString();
    }
}
