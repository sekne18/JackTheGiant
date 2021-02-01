using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool gameStartedFromMainMenu, gameRestartedAfterPlayerDied;

    [HideInInspector]
    public int score, coinScore, lifeScore;

    // Start is called before the first frame update
    void Awake()
    {
        MakeSingleton();
    }

    void Start()
    {
        InitializeVariables();
    }

    void LevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            if (gameRestartedAfterPlayerDied)
            {
                GameplayController.instance.SetScore(score);
                GameplayController.instance.SetcoinScore(coinScore);
                GameplayController.instance.SetLifeScore(lifeScore);

                PlayerScore.scoreCount = score;
                PlayerScore.coinCount = coinScore;
                PlayerScore.lifeCount = lifeScore;
            }
            else if (gameStartedFromMainMenu)
            {
                PlayerScore.scoreCount = 0;
                PlayerScore.coinCount = 0;
                PlayerScore.lifeCount = 2;

                GameplayController.instance.SetScore(0);
                GameplayController.instance.SetcoinScore(0);
                GameplayController.instance.SetLifeScore(2);

            }
            
        }

        
    }

    void InitializeVariables()
    {
        if (!PlayerPrefs.HasKey("Game Initialized"))
        {
            GamePreferences.SetEasyDifficultyState(0);
            GamePreferences.SetEasyDifficultyCoinScore(0);
            GamePreferences.SetEasyDiffucultyHighscore(0);

            GamePreferences.SetMediumDifficultyState(1);
            GamePreferences.SetMediumDifficultyCoinScore(0);
            GamePreferences.SetMediumDifficultyHighscore(0);

            GamePreferences.SetHardDifficultyState(0);
            GamePreferences.SetHardDifficultyCoinScore(0);
            GamePreferences.SetHardDifficultyHighscore(0);

            GamePreferences.SetMusicState(0);

            PlayerPrefs.SetInt("Game Initialized", 123);

        }


    }



    //iz scene v scene se podvajajo Gamemanagerji, tukaj to prepreči
    void MakeSingleton()
    {
        if(instance != null) //če je več managerje
        {
             Destroy(gameObject); //uniči gameobject
        }
        else
        {
             instance = this;
             DontDestroyOnLoad(gameObject); //ne uniči gameobject
        }
        
    }

    public void CheckGameStatus(int score, int coinScore, int lifeScore) //Preverimo ali imamo na volje še kaj zivljenj
    {
        if(lifeScore < 0)
        {

            if(GamePreferences.GetEasyDifficultyState() == 1)
            {
                int highScore = GamePreferences.GetEasyDifficultyHighscore();
                int coinHighScore = GamePreferences.GetEasyDifficultyCoinScore();

                if(highScore < score)
                {
                    GamePreferences.SetEasyDiffucultyHighscore(score);
                }
                if(coinHighScore < coinScore)
                {
                    GamePreferences.SetEasyDifficultyCoinScore(coinScore);
                }


            }
            if (GamePreferences.GetMediumDifficultyState() == 1)
            {
                int highScore = GamePreferences.GetMediumDifficultyHighscore();
                int coinHighScore = GamePreferences.GetMediumDifficultyCoinScore();

                if (highScore < score)
                {
                    GamePreferences.SetMediumDifficultyHighscore(score);
                }
                if (coinHighScore < coinScore)
                {
                    GamePreferences.SetMediumDifficultyCoinScore(coinScore);
                }


            }
            if (GamePreferences.GetHardDifficultyState() == 1)
            {
                int highScore = GamePreferences.GetHardDifficultyHighscore();
                int coinHighScore = GamePreferences.GetHardDifficultyCoinScore();

                if (highScore < score)
                {
                    GamePreferences.SetHardDifficultyHighscore(score);
                }
                if (coinHighScore < coinScore)
                {
                    GamePreferences.SetHardDifficultyCoinScore(coinScore);
                }


            }


            gameStartedFromMainMenu = false;
            gameRestartedAfterPlayerDied = false;

            GameplayController.instance.GameOverShowPanel(score, coinScore);
        }
        else
        {
            this.score = score;
            this.coinScore = coinScore;
            this.lifeScore = lifeScore;

            GameplayController.instance.SetScore(score);
            GameplayController.instance.SetcoinScore(coinScore);
            GameplayController.instance.SetLifeScore(lifeScore);


            gameStartedFromMainMenu = false;
            gameRestartedAfterPlayerDied = true;

            GameplayController.instance.PlayerDiedRestartTheGame();

        }
    }


}
