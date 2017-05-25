using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class StaticScreen : MonoBehaviour
{
    protected void Update()
    {
        ChangeScene();
    }
    
    /// <summary>
    /// Resize Objects in function of the current resolution.
    /// </summary>
    /// <param name="myObject"> object </param>
    protected void ResizeObject(GameObject myObject)
    {
        myObject.transform.localScale = Util.GetInstance.ResizeSpriteToScreen(this.GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                               this.GetComponent<SpriteRenderer>().sprite.bounds.size.y,
                                                                              (int)Util.Axis.Both);
    }

    /// <summary>
    /// Change scene
    /// </summary>
    protected void ChangeScene()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameController.previousScene = SceneManager.GetActiveScene().buildIndex;

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case (int)GameController.Scenes.Menu: SceneManager.LoadScene((int)GameController.Scenes.GameLoad); break;
                case (int)GameController.Scenes.GameOver: SceneManager.LoadScene((int)GameController.Scenes.Menu); break;
            }
        }
    }

    /// <summary>
    /// Change scene
    /// </summary>
    /// <param name="nextScene"> scene </param>
    protected void ChangeScene(GameController.Scenes nextScene)
    {
        GameController.previousScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((int)nextScene);
    }
}