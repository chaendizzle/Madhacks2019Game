using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Canvas canvas;
    private GameObject scoreField;
    
    public static GameOver GetInstance()
    {
        return GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOver>();
    }

    public void Lose()
    {
        int score = (int)ScoreBar.GetInstance().score;
        scoreField.GetComponent<Text>().text = "Score: " + score;
        ScoreBar.GetInstance().SetVisible(false);
        canvas.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        scoreField = transform.Find("GameOver").Find("ScoreText").gameObject;
        canvas.enabled = false;
    }
}
