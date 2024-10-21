using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDebug : MonoBehaviour
{
    public PlayerController player;

    public PlayerHP playerHP;

    [Header("UI텍스트")]
    public TMP_Text onGround;
    public TMP_Text onSlope;
    public TMP_Text onJump;
    public TMP_Text onAir;
    public TMP_Text PlayerSpeed;

    //public TMP_Text PlayerHP;
    public TMP_Text PlayerStamina;

    [Header("디버그 활성화")]
    public bool show = true;
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        if(playerHP == null)
        {
            playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!show)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

        if (onGround != null)
            onGround.text = "OnGround: " + player.OnGround().ToString();
        if (onSlope != null)
            onSlope.text = "OnSlope: " + player.OnSlope().ToString();
        if (onJump != null)
            onJump.text = "OnJump: " + player.OnJump().ToString();
        if (onAir != null)
            onAir.text = "onAir: " + player.OnAir().ToString();
        if (PlayerSpeed != null)
            PlayerSpeed.text = "Speed: " + player.GetComponent<Rigidbody>().velocity.magnitude.ToString(); ;
        //if (PlayerHP != null)
        //    PlayerHP.text = "HP: " + player.OnDamage().ToString();
        if(PlayerStamina != null)
            PlayerStamina.text = "Stamina: " + playerHP.GetComponent<PlayerHP>().currentStamina.ToString();
    }
}
