using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAggro : OnAble
{
    [SerializeField] Charger monster;
    public override void On()
    {
        monster.SetAggro(100.0f);
        monster.SetAggroDecrease(0.0f);
    }
}
