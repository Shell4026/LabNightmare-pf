using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class Scout : Monster
{
    [Header("스카웃")]
    Camera playerCamera;
    [SerializeField] GameObject face;
    [SerializeField] SkinnedMeshRenderer render;

    [SerializeField] AudioClip screamSound;
    [SerializeField] AudioClip detectSound;
    [SerializeField] AudioClip detectSound2;
    Animator anim;
    Material faceMat;

    int aggro = 0;

    bool flag = false;

    new void Awake()
    {
        base.Awake();

        playerCamera = Camera.main;
        anim = GetComponent<Animator>();
        faceMat = render.material;
    }

    void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        PlayIdleAudio();

        if(aggro == 1)
        {
            if (!flag)
            {
                faceMat.SetColor("_EmissionColor", new Color(191.0f / 255.0f, 32.0f / 255.0f, 0, 1.0f) * 2.4f);
                render.material = faceMat;
                flag = true;
            }
        }
        else if(aggro > 1)
        {
            if (!flag)
            {
                faceMat.SetColor("_EmissionColor", new Color(255.0f / 255.0f, 32.0f / 255.0f, 0.2f, 1.0f) * 2.4f);
                render.material = faceMat;
                flag = true;
            }
        }

        if (state == MonsterState.patrol)
        {
            RaycastHit faceHit;
            //Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 20);
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out faceHit, 20.0f))
            {
                if (faceHit.collider.transform.gameObject == face)
                {
                    if (aggro == 0)
                    {
                        anim.Play("FD_Scream_A", 0);
                        SetState(MonsterState.idle);
                        flag = false;
                    }
                    else if (aggro == 1)
                    {
                        anim.Play("Ability_Use_In_0", 0);
                        SetState(MonsterState.idle);
                        flag = false;
                    }
                    ++aggro;
                }
            }
        }
    }

    protected override void StateControl()
    {
        base.StateControl();
        if (state == MonsterState.idle)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 0.0f, Time.deltaTime * 4.0f));

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("MoveOnGround"))
            {
                SetState(MonsterState.patrol);
            }
            else if(stateInfo.IsName("Ability_End"))
            {
                anim.Play("MoveOnGround", 0);
                SetState(MonsterState.hunt);
            }
        }
        else if (state == MonsterState.patrol || state == MonsterState.move)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 1.0f, Time.deltaTime));
        }
        else if (state == MonsterState.hunt)
        {
            float currentForwardValue = anim.GetFloat("forward");
            anim.SetFloat("forward", Mathf.Lerp(currentForwardValue, 5.0f, Time.deltaTime));
            agent.speed = 4.0f;
        }
    }

    void PlayScreamAudio()
    {
        if (screamSound != null)
        {
            audio.clip = screamSound;
            audio.Play();
        }
    }

    void PlayDetectAuido()
    {
        if (detectSound != null)
        {
            audio.clip = detectSound;
            audio.Play();
        }
    }
    void PlayDetect2Auido()
    {
        if (detectSound2 != null)
        {
            audio.clip = detectSound2;
            audio.Play();
        }
    }
}
