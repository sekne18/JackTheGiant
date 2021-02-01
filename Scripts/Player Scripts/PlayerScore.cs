using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private AudioClip coinClip, lifeClip;

    private CameraScript cameraScript;

    private Vector3 previousPosition;
    private bool countScore;

    public static int scoreCount; //static ker bom dostopal do njih izven skripte
    public static int lifeCount;
    public static int coinCount;

    private void Awake()
    {
        cameraScript = Camera.main.GetComponent<CameraScript>();
    }

    void Start()
    {
        previousPosition = transform.position;
        countScore = true;
    }

    
    void Update()
    {
        CountScore(); // kličem tukej ker želim preverjati vsaki frame
    }

    void CountScore()//seštevanje kovancev po 1 za vsak Y, ki se premakne pri padanju
    {
        if (countScore)
        {
            if(transform.position.y < previousPosition.y)
            {
                scoreCount++;
            }
            previousPosition = transform.position;
            GameplayController.instance.SetScore(scoreCount);
            
        }
    }

     void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Coin")
        {
            coinCount++;
            scoreCount += 200;

            GameplayController.instance.SetScore(scoreCount);
            GameplayController.instance.SetcoinScore(coinCount);

            AudioSource.PlayClipAtPoint(coinClip, transform.position);
            target.gameObject.SetActive(false);
        }
        if (target.tag == "Life")
        {
            lifeCount++;
            scoreCount += 300;

            GameplayController.instance.SetScore(scoreCount);
            GameplayController.instance.SetLifeScore(lifeCount);

            AudioSource.PlayClipAtPoint(lifeClip, transform.position);
            target.gameObject.SetActive(false);
        }

        if(target.tag == "Bounds")
        {
            cameraScript.moveCamera = false;
            countScore = false;


            transform.position = new Vector3(500, 500, 0); //premakne igralca izven kamere
            lifeCount--;
            GameManager.instance.CheckGameStatus(scoreCount, coinCount, lifeCount);

        }

        if (target.tag == "Deadly")
        {
            cameraScript.moveCamera = false;
            countScore = false;

            

            transform.position = new Vector3(500, 500, 0); //premakne igralca izven kamere
            lifeCount--;

            GameManager.instance.CheckGameStatus(scoreCount, coinCount, lifeCount);
        }

     }








}
