using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWake : OnAble
{
    [SerializeField] Tank tank;
    public override void On()
    {
        tank.Wake();
    }
}
