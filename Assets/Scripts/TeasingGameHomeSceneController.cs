using UnityEngine;
using UnityEngine.UI;
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
        public Text bestScore;

        public void GoToGameScene()
        {
            SceneManager.LoadScene(SceneForButton.ToString());
        }

        private void Start()
        {
            int bestTime;
            if (PlayerPrefs.HasKey("bestTime"))
            {
                bestTime = PlayerPrefs.GetInt("bestTime");
            }
            else
            {
                bestTime = 180;
            }
            int minutes = bestTime / 60;
            int seconds = bestTime - minutes * 60;
            bestScore.text = "Best Time : " +(minutes < 10 ? "0" : " ") + minutes + ":" + (seconds < 10 ? "0" : " ") + seconds;
        }
    }

}