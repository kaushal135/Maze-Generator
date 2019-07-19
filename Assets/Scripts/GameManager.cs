using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public mazeLogic mazePrefab;
    private mazeLogic mazeInstance;

    public Player playerPrefab;
    private Player playerInstance;


	// Use this for initialization
	void Start () {
        startGame();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            restartGame();
        }
	}

    private void startGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        mazeInstance = Instantiate(mazePrefab) as mazeLogic;
        mazeInstance.Generate();
        //StartCoroutine(mazeInstance.Generate());

        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
    }

    private void restartGame()
    {
        //StopAllCoroutines();//optional
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
        startGame();
    }

}
