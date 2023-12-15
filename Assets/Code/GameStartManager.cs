using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public void StartGame()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerPrefs.SetInt("Round", 0);
        PlayerPrefs.SetInt("HP", 3);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}
