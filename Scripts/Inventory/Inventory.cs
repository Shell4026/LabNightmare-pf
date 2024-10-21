using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class Inventory : MonoBehaviour
{
    [SerializeField] Grabber grabber;
    [SerializeField] AudioSource pickSoundSource;
    public PickAble[] items = new PickAble[4];
    public GameObject[] slot;// 인벤토리 slot
    KeyCode[] numberKeyCode = new KeyCode[7] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7 };

    void Awake()
    {
        if (grabber == null)
            grabber = GameObject.Find("Player").GetComponent<Grabber>();
    }

    void Start()
    {
        for (int i = 0; i < items.Length; ++i)
            items[i] = null;

        grabber.onUseItem += (heldItem) => { RemoveItem(heldItem); };
    }

    // Update is called once per frame
    void Update()
    {
        UsingItem();
    }
    
    public void AddItem(PickAble obj, Sprite itemImg)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i] == null)
            {
                items[i] = obj;
                slot[i].transform.GetChild(0).GetComponent<Image>().sprite = itemImg; //slot 이미지 컴포넌트의 소스 이미지 = 아이템 이미지

                if (pickSoundSource != null)
                    pickSoundSource.Play();

                //인벤 들어감(슬롯 이미지)
                //어떤 종류의 아이템인지 저장하는 인벤토리 코드
                break;
            }
                
        }
    }

    public void RemoveItem(PickAble obj)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (obj == items[i])
            {
                slot[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                items[i] = null;
            }
        }
    }

    void UsingItem()
    {
        for(int i = 0; i < items.Length; ++i)
        {
            if (Input.GetKeyDown(numberKeyCode[i]))
            {
                if (items[i] != null)
                {
                    if (items[i].CanDrop())
                    {
                        items[i].gameObject.SetActive(true);
                        Vector3 goal = transform.position + transform.forward * 1;
                        items[i].transform.position = goal;

                        slot[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                        items[i] = null;
                    }
                    else
                    {
                        grabber.SetHeldItem(items[i]);
                    }
                    break;
                }
            }
        }
    }
}
