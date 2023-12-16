using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunReload : MonoBehaviour
{
    [SerializeField]
    private Image scale;
    private RectTransform rt;
    private Image image;

    [SerializeField]
    private Transform tank;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.enabled = false;
        scale.enabled = false;
    }

    public void TurnOn()
    {
        image.enabled = true;
        scale.enabled = true;
        StartCoroutine(ReloadAnim());
    }

    IEnumerator ReloadAnim()
    {
        for (float i = 0; i <= 1.5f; i += 0.01f)
        {
            scale.fillAmount = i / 1.5f;
            yield return new WaitForSeconds(0.01f);
        }
        image.enabled = false;
        scale.enabled = false;
    }

    private void FixedUpdate()
    {
        rt.position = tank.position - new Vector3(0, 1, 0);
    }
}
