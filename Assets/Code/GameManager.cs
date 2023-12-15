using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levels;
    private int currentLevel;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 300;
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        int idleTime = PlayerPrefs.GetInt("IdleTime", 20);
        int searchingTime = PlayerPrefs.GetInt("SearchingTime", 10);
        float aimTime = PlayerPrefs.GetFloat("AimTime", 0.5f);

        Instantiate(levels[currentLevel])
            .GetComponent<LevelManager>()
            .InitLevel(idleTime, searchingTime, aimTime);
    }
}
