using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] levels;
    private void Awake()
    {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Application.targetFrameRate = 300;

    }

    private void Start()
    {
        
    }
}

