using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTrigger : MonoBehaviour
{
    [SerializeField]
    PickAble obj;
    [SerializeField] OnAble[] targets;

    bool flag = false;
    void Update()
    {
        if (flag)
            return;
        if (!obj.gameObject.activeSelf)
        {
            foreach (var target in targets)
                target.On();
            flag = true;
        }
    }
}
