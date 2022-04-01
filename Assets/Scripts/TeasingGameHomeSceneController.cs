using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


namespace TeasingGame
{
    public enum TeasingGameScene :int 
    {
        Home,
        Game,
    }
public class TeasingGameHomeSceneController : MonoBehaviour
{
    public TeasingGameScene SceneForButton;

    // Update is called once per frame
    void Update()
    {
       /* if (Input.anyKeyDown)
        {
                GoToGameScene();
        }*/
    }


    public void GoToGameScene()
    {
        SceneManager.LoadScene(SceneForButton.ToString());
    }
}
}