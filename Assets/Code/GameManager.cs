using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Kino;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levels;

    [SerializeField]
    private Image levelStatus;
    private int currentLevel;
    private int playerHp;
    private int playerCoins;
    private int round;
    private int tanksLeft;
    private int totalTanks;

    [SerializeField]
    private GameObject uiCanvas,
        recordTable;

    [SerializeField]
    private RectTransform pauseWindow,
        winWindow,
        loseWindow;

    [SerializeField]
    private TextMeshProUGUI coinsCounter,
        livesCounter,
        roundScreen;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 300;
    }

    private PlayerMain player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMain>();
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
        tanksLeft = level.EnemyLeft;
        totalTanks = tanksLeft;
        level.InitLevel(idleTime, searchingTime, aimTime);
        coinsCounter.text = playerCoins.ToString();
        livesCounter.text = playerHp.ToString();
        levelStatus.fillAmount = ((float)totalTanks - (float)tanksLeft) / (float)totalTanks;
        roundScreen.text = $"{round + 1}-{currentLevel + 1}";
    }

    private void OnEnable()
    {
        TankAi.TankExploded += OnEnemyDead;
        PlayerBase.baseDestoyed += LoseGame;
    }

    private void OnDisable()
    {
        TankAi.TankExploded -= OnEnemyDead;
        PlayerBase.baseDestoyed -= LoseGame;
        Time.timeScale = 1;
    }

    private void OnEnemyDead(TankAi tank)
    {
        tanksLeft--;
        playerCoins++;
        levelStatus.fillAmount = ((float)totalTanks - (float)tanksLeft) / (float)totalTanks;
        coinsCounter.text = playerCoins.ToString();
    }

    public void PlayerDead()
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
        uiCanvas.SetActive(false);
        loseWindow.DOAnchorPosX(0, 0.5f);
        int oldRecord = PlayerPrefs.GetInt("Record", 11);
        int current = (round + 1) * 10 + (currentLevel + 1);
        if (current > oldRecord)
        {
            PlayerPrefs.SetInt("Record", current);
            PlayerPrefs.Save();
            recordTable.SetActive(true);
        }
        else
        {
            recordTable.SetActive(false);
        }
    }

    public void Restart()
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
        uiCanvas.SetActive(false);
        winWindow.DOAnchorPosX(0, 0.5f);
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

    public void BuyHP()
    {
        if (playerCoins >= 15)
        {
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(buySound, Vector3.zero);
            }
            playerCoins -= 15;
            playerHp++;
            livesCounter.text = playerHp.ToString();
            coinsCounter.text = playerCoins.ToString();
        }
    }

    [SerializeField]
    private AudioClip buySound;

    public void BuyShield()
    {
        if (playerCoins >= 7)
        {
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(buySound, Vector3.zero);
            }
            playerCoins -= 7;
            player.TurnOnShield();
            coinsCounter.text = playerCoins.ToString();
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            SetPause();
        }
        else if (Time.timeScale == 0)
        {
            UnPause();
        }
    }

    private void SetPause()
    {
        uiCanvas.SetActive(false);
        Sequence pause = DOTween
            .Sequence()
            .Append(pauseWindow.DOAnchorPosY(0, 0.2f))
            .AppendCallback(() =>
            {
                Time.timeScale = 0;
            });
    }

    public void UnPause()
    {
        uiCanvas.SetActive(true);
        Time.timeScale = 1;
        Sequence pause = DOTween.Sequence().Append(pauseWindow.DOAnchorPosY(2000, 0.2f));
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
