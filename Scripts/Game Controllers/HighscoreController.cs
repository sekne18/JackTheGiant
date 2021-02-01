using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour
{
    [SerializeField]
    private Text scoreText, coinText;





    // Start is called before the first frame update
    void Start()
    {
        SetScoreBaseOnDifficulty();

    }

    void SetScore(int score, int coinScore)
    {
        scoreText.text = score.ToString();
        coinText.text = coinScore.ToString();

    }

    void SetScoreBaseOnDifficulty()
    {
        if(GamePreferences.GetEasyDifficultyState() == 1)
        {
            SetScore(GamePreferences.GetEasyDifficultyHighscore(), GamePreferences.GetEasyDifficultyCoinScore());
        }
        if (GamePreferences.GetMediumDifficultyState() == 1)
        {
            SetScore(GamePreferences.GetMediumDifficultyHighscore(), GamePreferences.GetMediumDifficultyCoinScore());
        }
        if (GamePreferences.GetHardDifficultyState() == 1)
        {
            SetScore(GamePreferences.GetHardDifficultyHighscore(), GamePreferences.GetHardDifficultyCoinScore());
        }

    }


    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }




































}
