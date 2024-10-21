using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PoolAble prefab;
    public int capacity;

    LinkedList<PoolAble> objs;
    void Awake()
    {
        objs = new LinkedList<PoolAble>();
        for (int i = 0; i < capacity; ++i)
        {
            objs.AddLast(Instantiate(prefab, transform));
            objs.Last.Value.SetPool(this);
            objs.Last.Value.gameObject.SetActive(false);
        }
    }

    public PoolAble GetObject(GameObject owner = null, Transform parent = null)
    {
        if (objs.Count > 0)
        {
            var obj = objs.First.Value;
            obj.SetOwner(owner);
            obj.gameObject.SetActive(true);
            objs.RemoveFirst();
            if (parent != null)
            {
                obj.transform.parent = parent;
                obj.transform.localPosition = Vector3.zero;
                
            }
            return obj;
        }
        Debug.LogError("가능한 가용 오브젝트가 없습니다!");
        return null;
    }

    public void Release(PoolAble obj)
    {
        obj.transform.parent = transform;
        objs.AddLast(obj);
        obj.gameObject.SetActive(false);
    }
}
