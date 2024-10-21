using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour, IInteractable
{
    [SerializeField] AudioSource openAudio;
    [SerializeField] bool isLock = false;

    public GameObject panel;

    Material mat;
    public void Interact()
    {
        panel.GetComponent<DoorPanel>().SetLock(false);
        openAudio.Play();
    }

    public void SetLock(bool b)
    {
        isLock = b;
    }

    public bool GetLock()
    {
        return isLock;
    }

    void Start()
    {
        SetLock(isLock);
    }
}


