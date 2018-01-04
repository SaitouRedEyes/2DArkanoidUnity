using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Http 
{
    private static Http instance;
    private string result;    

    public enum Services
    {
        GET_HIGHSCORE = 0, SET_HIGHSCORE = 1
    }

    public string Result { get { return result; } }

    private Http()
    {
        result = "";
    }

    public static Http GetInstance
    {
        get
        {
            if (instance == null) instance = new Http();

            return instance;
        }
    }

    public IEnumerator SendToServer(WWWForm parameters)
    {
        WWW connection = new WWW("http://localhost/arkanoid_server_php/requests.php", parameters);

        float elapsedTime = 0;

        while (!connection.isDone)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 10.0f) break;
            yield return null;
        }

        if (!connection.isDone || !string.IsNullOrEmpty(connection.error))
        {
            result = null;
            yield break;
        }
        
        result = connection.text.Trim();
        connection.Dispose();
    }
}