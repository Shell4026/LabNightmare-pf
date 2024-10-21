using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Monster
{
    [Header("차저")]
    [SerializeField] AudioClip screamClip;
    [SerializeField] Transform detectPoint;
    [SerializeField] Transform seePoint;
    [SerializeField] LightController lightController;
    Camera playerCam;
    Animator anim;

    RaycastHit hit;

    [Header("추적 속력")]
    [SerializeField] float speed = 10.0f;
    [Header("추적 어그로 수치")]
    [SerializeField] float aggroMax = 4.0f;
    [Header("초당 어그로 증가량")]
    [SerializeField] float aggroIncrease = 1.5f;
    [Header("초당 어그로 감소량")]
    [SerializeField] float aggroDecrease = 0.5f;
    [Header("어그로 올라가는 거리")]
    [SerializeField] float aggroDistance = 5.0f;
    float aggro = 0.0f;

    float speedAtStart = 0.0f;

    bool aggroUp = false;
    bool flag = false;
    bool hunt = false;
    bool seeMe = false;

    new void Awake()
    {
        base.Awake();
        playerCam = Camera.main;
    }

    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        speedAtStart = agent.speed;
    }

    new void Update()
    {
        base.Update();

        aggroUp = false;
        seeMe = false;

        bool light = lightController.GetState();

        float disSqr = GetPlayerDistanceSqr();
        if (disSqr <= aggroDistance * aggroDistance) //첫번째 조건 - 특정 거리 내
            aggroUp = true;

        if (light)
        {
            bool detect = Detect();
            if (Physics.Linecast(playerCam.transform.position, detectPoint.position, out hit)) //두번째 조건 - 적이 날 볼 수 있을 때
            {
                if (hit.collider.gameObject == gameObject)
                {
                    aggroUp = true;
                }
            }

            if (Physics.Linecast(playerCam.transform.position, seePoint.position, out hit)) //세번째 조건 - 적을 보고 있을 때
            {
                if (hit.collider.gameObject == seePoint.gameObject)
                {
                    if (detect)
                    {
                        seeMe = true;
                        aggroUp = true;
                    }
                }
            }
        }

        if(aggroUp)
            aggro += aggroIncrease * Time.deltaTime;

        aggro -= aggroDecrease * Time.deltaTime;
        
        if (aggro < 0)
        {
            hunt = false;
            aggro = 0.0f;
            flag = false;
        }
        if (aggro > aggroMax)
        {
            hunt = true;
            aggro = aggroMax;
            if (!flag)
            {
                SetState(MonsterState.idle);
                anim.Play("Scream", 0);
                flag = true;
            }
        }

        if (hunt)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("ScreamEnd"))
            {
                anim.SetInteger("state", 0);
                SetState(MonsterState.hunt);
            }
        }

        if (state != MonsterState.attack)
        {
            if (!seeMe)
            {
                if (aggro < aggroMax)
                {
                    if (!hunt)
                    {
                        SetState(MonsterState.patrol);
                    }
                    else
                    {
                        SetState(MonsterState.hunt);
                    }
                }
                else
                {
                    SetState(MonsterState.hunt);
                }
            }
            else //괴물을 보고 있는 경우
            {
                SetState(MonsterState.idle);
            }
        }
    }

    protected override void StateControl()
    {
        base.StateControl();
        if(state == MonsterState.idle)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 0.0f, Time.deltaTime * 4.0f));

            footAudio.gameObject.SetActive(false);
        }
        else if(state == MonsterState.patrol)
        {
            float currentForwardValue = anim.GetFloat("forward");
            agent.speed = speedAtStart;
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, agent.speed, Time.deltaTime * 4.0f));

            footAudio.gameObject.SetActive(true);
        }
        else if (state == MonsterState.hunt)
        {
            agent.speed = speed;
            anim.SetFloat("forward", agent.speed);

            footAudio.gameObject.SetActive(true);
        }
        else if(state == MonsterState.attack)
        {
            var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("AttackEnd"))
            {
                SetState(MonsterState.hunt);
            }
            if(!stateInfo.IsName("Attack") && !stateInfo.IsName("AttackEnd"))
            {
                anim.Play("Attack", 0);
            }
        }
    }

    bool Detect()
    {
        Vector3 view = playerCam.WorldToViewportPoint(seePoint.position);
        if (view.x >= 0 && view.x <= 1 &&
            view.y >= 0 && view.y <= 1 &&
            view.z > 0)
        {
            return true;
        }
        return false;
    }

    public void SetAggro(float aggro)
    {
        this.aggro = aggro;
    }

    public void SetAggroDecrease(float decrease)
    {
        aggroDecrease = decrease;
    }
    void PlayScreamSound()
    {
        audio.clip = screamClip;
        audio.Play();
    }
}
