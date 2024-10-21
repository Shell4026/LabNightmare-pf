using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject[] objs;
    [SerializeField] OnAble[] targets;

    private void OnTriggerEnter(Collider other)
    {
        foreach(var obj in objs)
        {
            if(other.gameObject == obj)
            {
                foreach(var target in targets)
                    target.On();
                break;
            }
        }
    }
}
