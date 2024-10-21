using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour, IInteractable
{
    public Door door;
    [SerializeField] AudioSource openAudio;
    [SerializeField] AudioSource closeAudio;
    [SerializeField] AudioSource lockAudio;
    [SerializeField] bool isLock = false;

    Material mat;

    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        GetComponent<MeshRenderer>().material = mat;
    }
    void Start()
    {
        SetLock(isLock);
    }

    public void Interact()
    {
        if(isLock)
        {
            lockAudio.Play();
            return;
        }

        if (door.IsOpen())
        {
            if (closeAudio != null)
                closeAudio.Play();
        }
        else
        {
            if (openAudio != null)
                openAudio.Play();
        }
        door.Toggle();
    }

    public void SetLock(bool b)
    {
        isLock = b;

        if (isLock)
            mat.SetColor("_EmissionColor", Color.red);
        else
            mat.SetColor("_EmissionColor", Color.green);
    }

    public bool GetLock()
    {
        return isLock;
    }
}
