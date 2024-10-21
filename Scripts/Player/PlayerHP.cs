using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] Animator fadeOut;
    [SerializeField] PlayerController player;
    [SerializeField] sceneManager manager;

    public Slider HP_bar;
    //public Text hp;

    public Slider Stamina_bar;

    public float playerHP = 100.0f; // HP

    public float MaxStamina = 100.0f; // 스태미나
    public float minStamina = 20.0f; // 스태미나 
    public float currentStamina; // 현재 스태미나

    public float StaminaDecayRate = 15; // 현재 스태미나 감소 비율
    public float staminaRecoveryRate = 10.0f; // 현재 스태미나 회복 비율


    bool tired = false;

    void Awake()
    {
        if (fadeOut == null)
            fadeOut = GameObject.Find("FadeOut").GetComponent<Animator>();
        if (manager == null)
            manager = GameObject.Find("SceneManager").GetComponent<sceneManager>();
    }

    void Start()
    {
        currentStamina = MaxStamina;
    }
    // Update is called once per frame
    void Update()
    {
        if (player.GetRun() || player.OnAir())
        {
            currentStamina -= StaminaDecayRate * Time.deltaTime;

            if (currentStamina < 0.0f)
            {
                currentStamina = 0.0f;
                player.SetTired(true);
            }
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;  
            
            if (currentStamina >= MaxStamina)
                currentStamina = MaxStamina;

            if (tired)
            {
                if (currentStamina > minStamina)
                {
                    tired = false;
                    player.SetTired(false);
                }
            }
        }

        if (currentStamina < minStamina)
            tired = true;

        Stamina_bar.value = currentStamina;
    }

    public void Damage(float dmg)
    {
        playerHP -= dmg;
        if(playerHP <= 0)
        {
            player.SetStop(true);
            fadeOut.SetTrigger("fadeOut");
            Invoke(nameof(Death), 3.0f);
        }

        HP_bar.value = playerHP;
    }

    void Death()
    {
        manager.LoadCurrentStage();
    }
}


    

