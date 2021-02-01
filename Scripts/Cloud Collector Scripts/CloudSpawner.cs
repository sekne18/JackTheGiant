using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject[] clouds;
    //we don't want other classes to see this variable.

    private float distanceBetweenClouds = 3f;
    // distance on y axis (up-down) between clouds.

    private float minX, maxX;
    // we use these variables to position our clouds horizontally.
    // to determine maximum left/right so they stay on screen.

    private float lastCloudPositionY;
    // stores us the last cloud's Y position.
    // in order to spawn new clouds.

    private float controlX;
    //control the x (horizontal) position of the cloud. to make it jumpable
    //for the player

    [SerializeField]
    private GameObject[] collectables;

    private GameObject player;
    // get player stored in this variable so when we start the game, we position
    // the player on top of the first cloud.

    void Awake()
    {
        controlX = 0;
        SetMinAndMaxX();
        CreateClouds();
        player = GameObject.Find("Player");
        // find the game object that has the name player.

        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].SetActive(false);
        }
    }

    void Start()
    {
        PositionThePlayer();
    }

    //We are going to set the minimum and maximum x (horizontal) for clouds:
    void SetMinAndMaxX()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        //screen coordinates which we'll convert into unity's "world" coordinates.
        maxX = bounds.x - 0.5f; // max x in the camera view's width.
        // 0.5f to reduce width
        //of where cloud can spawn to the rightmost side.
        minX = -bounds.x + 0.5f;
    }

    void Shuffle(GameObject[] arrayToShuffle) // will shuffle our clouds array.
    {
        for (int i = 0; i < arrayToShuffle.Length; i++)
        {
            GameObject temp = arrayToShuffle[i];//a temporary game object
            int random = Random.Range(i, arrayToShuffle.Length); // random index from i to length of array.
            arrayToShuffle[i] = arrayToShuffle[random];
            arrayToShuffle[random] = temp;

            // arraytoSuffle[i] = 3 meaning temp = 3
            // arrayToShuffle[i] = arrayToShuffle[random]
            // let's say arrayToShuffle[random] = 5, arrayToShuffle[i] = 5
            // That's why we saved it in our temp variable.
            // Now we take arrayToShuffle[random] which at the moment has the value of 5
            //arrayToShuffle[random] = temp; to reposition these variables.

        }

    }


    void CreateClouds()
    {
        Shuffle(clouds); //see shuffle custom Fn above. Shuffle the array of clouds.
        //clouds replaces "arrayToShuffle".

        float positionY = 0f;

        for (int i = 0; i < clouds.Length; i++)
        {
            Vector3 temp = clouds[i].transform.position;

            temp.y = positionY;

            if (controlX == 0) // this controls the x position of the clouds, making them zigzags positionally.

            // if we only used random range between min and max X, they could

            // be positioned right below each other.
            {
                temp.x = Random.Range(0.0f, maxX);
                controlX = 1;

            }
            else if (controlX == 1)
            {
                temp.x = Random.Range(0.0f, minX);
                controlX = 2;
            }
            else if (controlX == 2)
            {
                temp.x = Random.Range(1.0f, maxX);
                controlX = 3;
            }
            else if (controlX == 3)
            {
                temp.x = Random.Range(-1.0f, minX);
                controlX = 0;
            }

            lastCloudPositionY = positionY;
            // each iteration of the loop we are going to save

            // a new value to our last cloud position y

            // so on the last iteration of the loop we'll know which is the

            // last position of the cloud, as needed below in OnTriggerEnter2D fn.
            
            //reassign it back to our clouds

            clouds[i].transform.position = temp;

            positionY -= distanceBetweenClouds;
        }


    }

    void PositionThePlayer()
    {
        //We need to get a reference to all of the clouds in the game.
        GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
        // find dark clouds ("Deadly")
        GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

        for (int i = 0; i < darkClouds.Length; i++)
        {
            if (darkClouds[i].transform.position.y == 0f)
            {
                Vector3 t = darkClouds[i].transform.position;

                darkClouds[i].transform.position = new Vector3(cloudsInGame[0].transform.position.x, cloudsInGame[0].transform.position.y, cloudsInGame[0].transform.position.z);
                // this repositions the dark cloud to the position of the cloud (after Vector3) so that it's NOT the first cloud in the game.
                cloudsInGame[0].transform.position = t;
                // this if block switches the dark cloud if encountered first with the next cloud.
            }
        }

        Vector3 temp = cloudsInGame[0].transform.position;

        for (int i = 1; i < cloudsInGame.Length; i++)
        {
            if (temp.y < cloudsInGame[i].transform.position.y)
            {
                temp = cloudsInGame[i].transform.position;


            }
            // reassign if line code to temp cloud position.
            temp.y += 0.2f;
            // adjust as necessary. eg. Course says 0.8f but that was much higher than
            // the cloud. At 0.2 he starts ON the cloud EVERY TIME.
            player.transform.position = temp;
            // this positions the player on top of the first cloud.
        }



    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Cloud" || target.tag == "Deadly")
        {
            if (target.transform.position.y == lastCloudPositionY)
            {
                // if it's a cloud or dark cloud, and it's the last one

                // then we need to respawn clouds.
                Shuffle(clouds);
                Shuffle(collectables);

                Vector3 temp = target.transform.position;

                for (int i = 0; i < clouds.Length; i++)
                {
                    if (!clouds[i].activeInHierarchy) //! = če elementi obaka niso aktivni v hiearhiji
                    {
                        if (controlX == 0)
                        {
                            temp.x = Random.Range(0.0f, maxX);
                            controlX = 1;

                        }
                        else if (controlX == 1)
                        {
                            temp.x = Random.Range(0.0f, minX);
                            controlX = 2;
                        }
                        else if (controlX == 2)
                        {
                            temp.x = Random.Range(1.0f, maxX);
                            controlX = 3;
                        }
                        else if (controlX == 3)
                        {
                            temp.x = Random.Range(-1.0f, minX);
                            controlX = 0;
                        }

                        temp.y -= distanceBetweenClouds;

                        lastCloudPositionY = temp.y;

                        clouds[i].transform.position = temp;
                        clouds[i].SetActive(true);

                        int random = Random.Range(0, collectables.Length);

                        if (clouds[i].tag != "Deadly")
                        {
                            if (!collectables[random].activeInHierarchy)
                            {
                                Vector3 temp2 = clouds[i].transform.position;
                                temp2.y += 0.7f;

                                if (collectables[random].tag == "Life")
                                {
                                    if (PlayerScore.lifeCount < 2)
                                    {
                                        collectables[random].transform.position = temp2;
                                        collectables[random].SetActive(true);
                                    }
                                }
                                else
                                {
                                    collectables[random].transform.position = temp2;
                                    collectables[random].SetActive(true);
                                }
                            }


                        }
                    }
                }
            }
        }
    }
}
