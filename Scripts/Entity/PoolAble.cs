using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAble : MonoBehaviour
{
    ObjectPool pool;
    GameObject owner;

    public void Destroy()
    {
        if (!pool)
            return;

        owner = null;
        pool.Release(this);
    }

    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public GameObject GetOwner()
    {
        return owner;
    }
}
