using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Text highScoreText; // ��ʾ��ʷ��ߵ÷ֵ�UI�ı�

    private const string HIGH_SCORE_KEY = "HighScore"; // �洢��ʷ��ߵ÷ֵļ���
    void Start()
    {
        // ������ʷ��ߵ÷�
        if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
        {
            float highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY);
            highScoreText.text = "��ʷ��ߵ÷�: " + highScore.ToString();
        }
        else
        {
            highScoreText.text = "��ʷ��ߵ÷�: N/A";
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
