using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int previousScene;
    public GameObject border;

    private int lineNumber, columnNumber, score, highscore;
    private Text textComponentScore, textComponentHighscore;
    private Util util;
    private GameObject[] borders;

    public enum Scenes
    {
        Menu = 0, Game = 1, GameOver = 2, GameWin = 3, GameLoad = 4
    }

    void Start()
    {
        util = Util.GetInstance;

        SetupBorders();

        score = 0;
        highscore = PlayerPrefs.GetInt("Highscore").Equals("defaultValue") ? 0 : PlayerPrefs.GetInt("Highscore");

        textComponentScore = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        textComponentHighscore = GameObject.FindGameObjectWithTag("Highscore").GetComponent<Text>();
        textComponentHighscore.text = "Highscore: " + highscore;

        lineNumber = 2;
        columnNumber = (int)Mathf.Round(Screen.width / 100);

        SetupBlocks();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    /// <summary>
    /// Score Manager
    /// </summary>
    public int ScoreValue
    {
        set 
        {
            if (value <= 30)
            {
                score += value;
                textComponentScore.text = "Score: " + score;
            }
        }
    }

    /// <summary>
    /// Configure the blocks in function of the current screen resolution.
    /// </summary>
    public void SetupBlocks()
    {
        lineNumber++;
        GameObject block = Resources.Load("Prefabs/Block") as GameObject;

        float percentX = 10, percentY = 90;
        float spaceBetweenBlocks = ((util.GetScreenResolution.x * 2) / columnNumber) - util.GetScreenPositionPercent(5.0f, 5.0f).x;

        for (int currLine = 0; currLine < lineNumber; currLine++)
        {
            for (int currColumn = 0; currColumn < columnNumber; currColumn++)
            {
                Vector3 adjustedPosition = util.GetAdjustedPosition(util.GetScreenPositionPercent(percentX, percentY));

                adjustedPosition = new Vector3(adjustedPosition.x + currColumn * (block.transform.localScale.x / 2 + spaceBetweenBlocks),
                                               adjustedPosition.y - currLine   * (block.transform.localScale.y / 2));

                Instantiate(block, adjustedPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Configure the bounds of the game in function of the current screen resolution.
    /// </summary>
    private void SetupBorders()
    {
        borders = new GameObject[4];
        
        //Set positions
        borders[0] = (GameObject)Instantiate(Resources.Load("Prefabs/Border"), util.GetAdjustedPosition(util.GetScreenPositionPercent(0, 50)), Quaternion.identity);
        borders[1] = (GameObject)Instantiate(Resources.Load("Prefabs/Border"), util.GetAdjustedPosition(util.GetScreenPositionPercent(100, 50)), Quaternion.identity);
        borders[2] = (GameObject)Instantiate(Resources.Load("Prefabs/Border"), util.GetAdjustedPosition(util.GetScreenPositionPercent(50, 100)), Quaternion.identity);
        borders[3] = (GameObject)Instantiate(Resources.Load("Prefabs/Border"), util.GetAdjustedPosition(util.GetScreenPositionPercent(50, -3)), Quaternion.identity);

        //Set rotations
        borders[2].transform.localEulerAngles = new Vector3(0, 0, 90);
        borders[3].transform.localEulerAngles = new Vector3(0, 0, 90);

        //Set scales
        borders[0].transform.localScale = Util.GetInstance.ResizeSpriteToScreen(borders[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                                borders[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y, (int)Util.Axis.Height);
        borders[1].transform.localScale = Util.GetInstance.ResizeSpriteToScreen(borders[1].GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                                borders[1].GetComponent<SpriteRenderer>().sprite.bounds.size.y, (int)Util.Axis.Height);
        borders[2].transform.localScale = Util.GetInstance.ResizeSpriteToScreen(borders[2].GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                                borders[2].GetComponent<SpriteRenderer>().sprite.bounds.size.y, (int)Util.Axis.RotatedWidth);
        borders[3].transform.localScale = Util.GetInstance.ResizeSpriteToScreen(borders[3].GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                                borders[3].GetComponent<SpriteRenderer>().sprite.bounds.size.y, (int)Util.Axis.RotatedWidth);

        //Set tags
        borders[0].tag = "VerticalBound";
        borders[1].tag = "VerticalBound";
        borders[2].tag = "TopBound";
        borders[3].tag = "BottomBound";
    }

    /// <summary>
    /// Set the score in the player prefs to check (in load screen) if this score is the highscore.
    /// </summary>
    public void GameOver()
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene((int)GameController.Scenes.GameLoad);
    }
}