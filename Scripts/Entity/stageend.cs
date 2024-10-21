using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageend : MonoBehaviour, IInteractable
{
 
    public GameObject manager;
    public int stage;


    public void Interact()
    {
        manager.GetComponent<sceneManager>().LoadStage(stage);
    }



    void Awake()
    {

    }
    void Start()
    {

    }
}


