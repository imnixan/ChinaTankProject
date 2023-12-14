using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera camera;
    private Vector3 camPos;
    private float maxY,
        minY,
        maxX,
        minX;

    private void Start()
    {
        camera = Camera.main;
        Vector2 cameraSizePlus = camera.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 cameraSizeMinus = camera.ViewportToWorldPoint(Vector2.zero);
        Debug.Log(cameraSizePlus);
        Debug.Log(cameraSizeMinus);
        maxY = 7.6f - cameraSizePlus.y;
        minY = -7.6f - cameraSizeMinus.y;
        maxX = 12.6f - cameraSizePlus.x;
        minX = -12.6f - cameraSizeMinus.x;
        camPos = camera.transform.position;
    }

    private void FixedUpdate()
    {
        bool vertPosOk = transform.position.y <= maxY && transform.position.y >= minY;
        bool horPosOk = transform.position.x <= maxX && transform.position.x >= minX;

        if (vertPosOk)
        {
            camPos.y = transform.position.y;
        }
        if (horPosOk)
        {
            camPos.x = transform.position.x;
        }
        camera.transform.position = camPos;
    }
}
