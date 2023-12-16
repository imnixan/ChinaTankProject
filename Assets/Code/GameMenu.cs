using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform settingsLine;

    [SerializeField]
    private GameObject banSound,
        banMusic;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        banMusic.SetActive(PlayerPrefs.GetInt("Music", 1) != 1);
        banSound.SetActive(PlayerPrefs.GetInt("Sound", 1) != 1);
    }

    public void Settings()
    {
        if (settingsLine.anchoredPosition.x == 0)
        {
            settingsLine.DOAnchorPosX(-600, 0.5f);
        }
        else if (settingsLine.anchoredPosition.x == -600)
        {
            settingsLine.DOAnchorPosX(0, 0.5f);
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
}
