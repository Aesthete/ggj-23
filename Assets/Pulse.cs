using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pulse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 current = transform.localScale;
        Vector2 target = current * 1.2f;
        DOTween.Sequence()
            .Append(transform.DOScale(target, 0.3f).SetDelay(0.3f))
            .Append(transform.DOScale(current, 0.3f))
            .SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
