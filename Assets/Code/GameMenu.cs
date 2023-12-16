using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Buffers;
using System.Collections;
using TMPro;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform settingsLine;

    [SerializeField]
    private GameObject banSound,
        banMusic;

    [SerializeField]
    private Image loadingScreen,
        bar;
    private GameStartManager gsm;

    [SerializeField]
    private TextMeshProUGUI recordShow;

    private void Start()
    {
        gsm = GetComponent<GameStartManager>();
        loadingScreen.color = new Color(0, 0, 0, 0);
        bar.fillAmount = 0;
        loadingScreen.gameObject.SetActive(false);
        Screen.orientation = ScreenOrientation.Portrait;
        banMusic.SetActive(PlayerPrefs.GetInt("Music", 1) != 1);
        banSound.SetActive(PlayerPrefs.GetInt("Sound", 1) != 1);
    }

    public void Settings()
    {
        if (settingsLine.anchoredPosition.x >= -50)
        {
            settingsLine.DOAnchorPosX(-600, 0.2f);
        }
        else if (settingsLine.anchoredPosition.x <= -550)
        {
            settingsLine.DOAnchorPosX(0, 0.23f);
        }
    }

    public void BanMusic()
    {
        int newMusic = banMusic.activeSelf ? 1 : 0;
        PlayerPrefs.SetInt("Music", newMusic);
        PlayerPrefs.Save();
        banMusic.SetActive(newMusic != 1);
    }

    public void BanSound()
    {
        int newSound = banSound.activeSelf ? 1 : 0;
        PlayerPrefs.SetInt("Sound", newSound);
        PlayerPrefs.Save();
        banSound.SetActive(newSound != 1);
    }

    public void PlayGame()
    {
        loadingScreen.gameObject.SetActive(true);
        int record = PlayerPrefs.GetInt("Record", 11);
        recordShow.text = $"{Mathf.Round(record / 10)}-{Mathf.Round(record % 10)}";
        Sequence loadShow = DOTween
            .Sequence()
            .Append(loadingScreen.DOColor(Color.white, 0.5f))
            .AppendCallback(() =>
            {
                StartCoroutine(LadingScreen());
            });
    }

    IEnumerator LadingScreen()
    {
        WaitForSeconds timer = new WaitForSeconds(0.01f);
        for (float i = 0; i <= 1f; i += 0.01f)
        {
            bar.fillAmount = i;
            yield return timer;
        }
        gsm.StartGame();
    }
}
