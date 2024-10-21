using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Monster
{
    [Header("굉음")]
    [SerializeField] protected AudioClip wakeSound;
    [Header("플레이어 발각 거리")]
    [SerializeField] float detectDis = 3.0f;

    Animator anim;
    bool init = false;
    bool awake = false;
    bool hunt = false;

    int aggro = 0;

    Vector3 soundPos;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    new void Start()
    {
        base.Start();
        Invoke(nameof(Init), 1.0f);
    }

    void Init()
    {
        init = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        PlayIdleAudio();
    }

    protected override void StateControl()
    {
        base.StateControl();

        if (state == MonsterState.idle)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 0.0f, Time.deltaTime * 4.0f));
        }
        else if (state == MonsterState.patrol || state == MonsterState.move)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 1.0f, Time.deltaTime));

            if (aggro > 2)
            {
                float soundToMonsterDis = (transform.position - soundPos).sqrMagnitude;
                if (soundToMonsterDis < detectDis * detectDis)
                {
                    float plyerToSoundDis = (player.transform.position - soundPos).sqrMagnitude;
                    if (plyerToSoundDis < detectDis * detectDis)
                    {
                        SetState(MonsterState.idle);
                        PlayRoarAnim();
                    }
                }
            }
        }
        else if(state == MonsterState.hunt)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 5.0f, Time.deltaTime));
            agent.speed = 4.0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var physSound = collision.gameObject.GetComponent<PhysSound>();
        if(physSound != null)
        {
            physSound.SetOwner(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!init)
            return;

        if (other.CompareTag("Sound"))
        {
            var poolAble = other.gameObject.GetComponent<PoolAble>();
            if (poolAble != null)
            {
                if(poolAble.GetOwner() != this.gameObject)
                {
                    ++aggro;

                    soundPos = other.transform.position;
                    movePosition = soundPos;

                    if (!awake)
                    {
                        PlayWakeAnim();
                    }
                    else
                    {
                        if (!hunt)
                            SetState(MonsterState.move);
                    }
                }
            }
        }
    }

    public void Wake()
    {
        PlayWakeAnim();
    }

    void PlayWakeAnim()
    {
        anim.SetInteger("state", 1);
    }

    void PlayRoarAnim()
    {
        anim.SetInteger("state", 3);
    }

    void FinishWakeAnim()
    {
        audio.volume = 0.5f;

        awake = true;
        anim.SetInteger("state", 2);
        SetState(MonsterState.move);
    }

    void FinishRoarAnim()
    {
        anim.SetInteger("state", 2);
        SetState(MonsterState.hunt);
        hunt = true;
    }

    public void PlayWakeSound()
    {
        audio.clip = wakeSound;
        audio.Play();
    }
}
