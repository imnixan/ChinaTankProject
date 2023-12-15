using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject[] bonuses;

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    private int EnemyLeft;

    private List<TankAi> tanks = new List<TankAi>();

    public int idleTime,
        searchingTime;
    public float aimTime;

    public void InitLevel(int idleTime, int searchingTime, float aimTime)
    {
        this.idleTime = idleTime;
        this.searchingTime = searchingTime;
        this.aimTime = aimTime;
        foreach (var spawnPoint in spawnPoints)
        {
            SpawnTank(spawnPoint.transform.position);
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
        else
        {
            Debug.Log("WIN");
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
