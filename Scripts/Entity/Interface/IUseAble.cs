using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseAble
{
    public Sprite ItemSprite { get; set; }

    public bool Use(GameObject target);
}
