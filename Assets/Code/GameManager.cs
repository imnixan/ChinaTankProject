using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] levels;
    [SerializeField]
    private GameObject[] spawnPoses, bonuses;
    private int currentLevel;
    private void Awake()
    {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Application.targetFrameRate = 300;

    }

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        Instantiate(levels[currentLevel]);
        spawnPoses = GameObject.FindGameObjectsWithTag("SpawnPoint");

    }
}

