using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject player;
    Light flashLight;
    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(player.GetComponent<PlayerController>().keyFlash))
        {
            flashLight.enabled = !flashLight.enabled;
        }
    }
}

