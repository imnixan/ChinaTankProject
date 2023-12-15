using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levels;
    private int currentLevel;
    private int playerHp;
    private int playerCoins;
    private int round;
    private int tanksLeft;

    [SerializeField]
    private TextMeshProUGUI coinsCounter,
        livesCounter,
        tanksCounter,
        roundScreen;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 300;
    }

    private void Start()
    {
        playerCoins = PlayerPrefs.GetInt("Coins");
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        round = PlayerPrefs.GetInt("Round");
        playerHp = PlayerPrefs.GetInt("HP", 3);

        int idleTime = 20 - (round * 2);
        if (idleTime < 0)
        {
            idleTime = 0;
        }
        int searchingTime = 10;
        if (searchingTime < 5)
        {
            searchingTime = 5;
        }
        float aimTime = 0.5f - (round / 10);
        if (aimTime < 0.15f)
        {
            aimTime = 0.15f;
        }
        LevelManager level = Instantiate(levels[currentLevel]).GetComponent<LevelManager>();
        level.InitLevel(idleTime, searchingTime, aimTime);
        tanksLeft = level.EnemyLeft;
        coinsCounter.text = playerCoins.ToString();
        livesCounter.text = playerHp.ToString();
        tanksCounter.text = tanksLeft.ToString();
        roundScreen.text = $"{round + 1}-{currentLevel + 1}";
    }

    private void OnEnable()
    {
        TankAi.TankExploded += OnEnemyDead;
    }

    private void OnDisable()
    {
        TankAi.TankExploded -= OnEnemyDead;
    }

    private void OnEnemyDead(TankAi tank)
    {
        tanksLeft--;
        playerCoins++;
        tanksCounter.text = tanksLeft.ToString();
        coinsCounter.text = playerCoins.ToString();
    }

    public void PlayerDead(PlayerMain player)
    {
        playerHp--;
        livesCounter.text = playerHp.ToString();

        if (playerHp > 0)
        {
            player.Respawn();
        }
        else
        {
            Debug.Log("Lose");
            player.gameObject.SetActive(false);
            LoseGame();
        }
    }

    public void LoseGame()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerPrefs.SetInt("Round", 0);
        PlayerPrefs.SetInt("HP", 3);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

    public void WinGame()
    {
        Debug.Log("WInGame");
        NextLevel();
    }

    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Length)
        {
            currentLevel = 0;
            round++;
            PlayerPrefs.SetInt("Round", round);
        }
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("HP", playerHp);
        PlayerPrefs.SetInt("Coins", playerCoins);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}
