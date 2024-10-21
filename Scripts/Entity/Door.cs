using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IOn, IOff
{
    [SerializeField] bool open = false;

    public AudioSource doorSound;
    public AudioClip openSound;
    public AudioClip closeSound;

    Animator animator;

    public delegate void OnOpen();
    public OnOpen onOpen = () => { };

    public void On()
    {
        if (doorSound != null)
        {
            if (openSound != null)
            {
                doorSound.clip = openSound;
                doorSound.Play();
            }
        }
        animator.SetBool("on", true);
        open = true;
        onOpen();
    }

    public void Off()
    {
        if (doorSound != null)
        {
            if (closeSound != null)
            {
                doorSound.clip = closeSound;
                doorSound.Play();
            }
        }
        animator.SetBool("on", false);
        open = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (open)
            On();
        else
            Off();
    }

    public void Toggle()
    {
        if (open)
            Off();
        else
            On();
    }

    public bool IsOpen()
    {
        return open;
    }
}
