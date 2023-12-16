using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    public int EnemyLeft;

    [SerializeField]
    private int enemyStart;

    private GameManager gm;
    private List<TankAi> tanks = new List<TankAi>();

    public int idleTime,
        searchingTime;
    public float aimTime;

    public void InitLevel(int idleTime, int searchingTime, float aimTime)
    {
        this.gm = FindAnyObjectByType<GameManager>();
        this.idleTime = idleTime;
        this.searchingTime = searchingTime;
        this.aimTime = aimTime;
        for (int i = 0; i < enemyStart; i++)
        {
            SpawnTank(spawnPoints[i].transform.position);
        }
    }

    private void OnEnable()
    {
        TankAi.TankExploded += OnTankExploded;
    }

    private void OnDisable()
    {
        TankAi.TankExploded -= OnTankExploded;
    }

    private void OnTankExploded(TankAi tank)
    {
        tanks.Remove(tank);
        if (EnemyLeft > 0)
        {
            SpawnTank(spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position);
        }
        else if (tanks.Count <= 0)
        {
            gm.WinGame();
        }
    }

    private void SpawnTank(Vector2 pos)
    {
        EnemyLeft--;
        TankAi tank = Instantiate(enemyPrefab, pos, new Quaternion()).GetComponent<TankAi>();
        tank.Init(idleTime, searchingTime, aimTime);
        tanks.Add(tank);
        Debug.Log($"Spawn tank, left {EnemyLeft}");
    }
}
