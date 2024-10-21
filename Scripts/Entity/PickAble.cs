using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UI;

public class PickAble : Grabable, IUseAble
{
    [SerializeField] Inventory inventory;

    [SerializeField] Sprite itemSprite; // 획득 아이템 이미지
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = ItemSprite; } }

    [SerializeField] bool canDrop = false;

    void Awake()
    {
        if(inventory == null)
        {
            inventory = GameObject.Find("Inventory_UI").GetComponent<Inventory>();
        }
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
    }

    public override void Grab(Transform owner, Grabber grabOwner)
    {
        Debug.Log("grab");
        Inventory_Saving();
        grabOwner.GrabClear();
    }

    void Inventory_Saving()
    {
        inventory.AddItem(this, itemSprite);
        gameObject.SetActive(false);  // 잡은 오브젝트 지우기
    }

    public void SetCanDrop(bool b)
    {
        canDrop = b;
    }

    public bool CanDrop()
    {
        return canDrop;
    }

    public virtual bool Use(GameObject target)
    {
        return false;
    }
}
