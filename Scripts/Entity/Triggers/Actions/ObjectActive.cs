using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActive : OnAble
{
    [SerializeField] bool toggle = false;
    [SerializeField] GameObject[] targets;
    public override void On()
    {
        foreach(var target in targets)
        {
            if(toggle)
            {
                target.SetActive(!target.activeSelf);
            }
            else
            {
                target.SetActive(true);
            }
        }
    }
}
