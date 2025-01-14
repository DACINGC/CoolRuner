using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Text highScoreText; // 显示历史最高得分的UI文本

    private const string HIGH_SCORE_KEY = "HighScore"; // 存储历史最高得分的键名
    void Start()
    {
        // 加载历史最高得分
        if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
        {
            float highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY);
            highScoreText.text = "历史最高得分: " + highScore.ToString();
        }
        else
        {
            highScoreText.text = "历史最高得分: N/A";
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
