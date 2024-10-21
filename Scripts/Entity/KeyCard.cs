using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : PickAble
{
    [SerializeField] DoorPanel targetPanel;
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }
    public override bool Use(GameObject target)
    {
        if (targetPanel.gameObject != target)
            return false;

        if(targetPanel.GetLock())
        {
            targetPanel.SetLock(false);
            return true;
        }
        return false;
    }
}
