using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBase : MonoBehaviour
{
    public static event UnityAction baseDestoyed;

    private void OnDestroy()
    {
        baseDestoyed?.Invoke();
    }
}
