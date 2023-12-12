using System.Collections;
using UnityEngine;
using SAP2D;
using static UnityEngine.GraphicsBuffer;

public class TankMover : MonoBehaviour
{
    private SAP2DAgent agent;

    public void Init(Transform target)
    {
        agent = GetComponent<SAP2DAgent>();
        agent.Target = target;
        agent.CanMove = true;
    }

    public void StopMoving()
    {
        agent.CanMove = false;
    }

    public void Move()
    {
        agent.CanMove = true;
    }

    public bool IsAchievable(Vector3 pos)
    {
        return agent.grid.GetTileDataAtWorldPosition(pos).isWalkable;
    }
}
