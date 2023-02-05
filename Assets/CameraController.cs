using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public float scrollRate = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentSize = Camera.main.orthographicSize;
        float y = Input.mouseScrollDelta.y * scrollRate;
        Camera.main.orthographicSize = currentSize + y;
    }
}
