using UnityEngine;
using System.Collections;

public class BGLoadScene : StaticScreen 
{
    public GameObject load, connectionProblem;

    private Http http;
    private Operation_State currState;

    private enum Operation_State { START = 0, SUCCESS = 1, ERROR = 2, IDLE = 3}

    void Start()
    {
        ResizeObject(this.gameObject);
        ResizeObject(load);
        ResizeObject(connectionProblem);

        http = Http.GetInstance;
        currState = Operation_State.IDLE;

        ScreenAction(false);
    }

    new void Update()
    {
        switch(currState)
        {
            case Operation_State.START: load.transform.Rotate(Vector3.back, 5); break;
            case Operation_State.ERROR: if (Input.GetKeyDown(KeyCode.Space))
                                        {
                                            connectionProblem.SetActive(false);
                                            ScreenAction(false);
                                        } break;
        }
    }

    /// <summary>
    /// The next action of the load screen (change scene or server access).
    /// </summary>
    /// <param name="changeScene"> bollean for choose the action </param>
    private void ScreenAction(bool changeScene)
    {
        currState = Operation_State.IDLE;

        switch (GameController.previousScene)
        {
            case (int)GameController.Scenes.Menu: if (changeScene) base.ChangeScene(GameController.Scenes.Game);     
                                                  else StartCoroutine(GetHighscore(false)); break;
            case (int)GameController.Scenes.Game: if (changeScene) base.ChangeScene(GameController.Scenes.GameOver); 
                                                  else StartCoroutine(SetHighscore()); break;       
        }
    }

    /// <summary>
    /// Get the highscore from server.
    /// </summary>
    /// <param name="preServiceCall"> Boolean that determines if the response of this service is necessary to another service </param>
    /// <returns></returns>
    private IEnumerator GetHighscore(bool preServiceCall)
    {
        int highscore = 0;
        currState = Operation_State.START;

        WWWForm form = new WWWForm();
        form.AddField("sID", (int)Http.Services.GET_HIGHSCORE);
        
        yield return StartCoroutine(http.SendToServer(form));
        
        if (http.Result != null && int.TryParse(http.Result, out highscore))
        {
            PlayerPrefs.SetInt("Highscore", highscore);
            currState = Operation_State.SUCCESS;
            
            if (!preServiceCall) ScreenAction(true);
        }
        else
        {
            connectionProblem.SetActive(true);
            currState = Operation_State.ERROR;
        }
    }

    /// <summary>
    /// Set the new highscore.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetHighscore()
    {
        //Updating highscore for comparison
        StartCoroutine(GetHighscore(true));

        //waiting fot the GetHighscore service answer.
        while (true)
        {   
            //If the answer of the GetHighscore is success...
            if (currState == Operation_State.SUCCESS) 
            {
                //The current score is higher than current highscore? Save the new highscore.
                if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("Highscore")) 
                {
                    currState = Operation_State.START;

                    WWWForm form = new WWWForm();
                    form.AddField("sID", (int)Http.Services.SET_HIGHSCORE);
                    form.AddField("value", PlayerPrefs.GetInt("Score"));                 

                    yield return StartCoroutine(http.SendToServer(form));

                    if (http.Result != null)
                    {
                        currState = Operation_State.SUCCESS;
                        ScreenAction(true);
                        break;
                    }
                    else
                    {
                        currState = Operation_State.ERROR;
                        connectionProblem.SetActive(true);
                        break;
                    }
                }
                else //The current score isn't higher than current highscore? Just Change scene.
                {
                    ScreenAction(true);
                    break;
                }
            } //if GetHighscore operation have some problem, break the SetHighscore operation
            else if (currState == Operation_State.ERROR) break;
            
            //This line allow the while operation in a parallel operation.
            yield return new WaitForSeconds(0.5f);
        }
    }
}