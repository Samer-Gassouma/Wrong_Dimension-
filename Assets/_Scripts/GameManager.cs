using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
   
    public  Text ScoreText;
    public Text HealthText;
    public Text GameOverText;
    public Text RestartText;
    public Text QuitText;
    public Text GameTitleText;


    static int score = 0;

    public bool gameOver = false;

    public static void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

 

    private void Update()
    {
        ScoreText.text = "Score: " + score;

        if (gameOver)
        {
            RestartText.text = "Press 'R' to Restart";
            QuitText.text = "Press 'Q' to Quit";
            GameOverText.text = "Game Over!";
            GameTitleText.text = "Press 'R' to Restart or 'Q' to Quit";
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        

    }

}
