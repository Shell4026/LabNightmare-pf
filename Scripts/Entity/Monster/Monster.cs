using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] PlayerHP playerHP;
    [SerializeField] protected new AudioSource audio;
    [SerializeField] protected AudioSource footAudio;
    [SerializeField] protected AudioClip[] idleSounds;
    [SerializeField] protected AudioClip[] footStepSounds;

    [Header("순찰 경로")]
    protected Vector3 movePosition;
    [SerializeField] protected Transform[] patrol;
    [Header("이동 대기 시간")]
    public float waitMin = 1.0f;
    public float waitMax = 3.0f;
    [Header("스탯")]
    public float damage = 50.0f;
    [SerializeField] float attackRange = 2.0f;
    public enum MonsterState
    {
        idle,
        move,
        patrol,
        hunt,
        attack
    }

    [SerializeField]protected MonsterState state = MonsterState.patrol;
    protected NavMeshAgent agent;

    bool wait = false;
    int patrolIdx = 0;
    int footStepIdx = 0;

    float playerDisSqr = 0.0f;

    RaycastHit hit;

    protected void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if(playerHP == null)
        {
            playerHP = player.GetComponent<PlayerHP>();
        }
    }

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrol.Length > 0)
            agent.SetDestination(patrol[0].position);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        StateControl();
        playerDisSqr = (player.transform.position - transform.position).sqrMagnitude;
    }

    protected void MoveTo(Vector3 pos)
    {
        if (!wait)
        {
            agent.SetDestination(pos);
            if (agent.remainingDistance < 0.5f)
            {
                wait = true;
                state = MonsterState.idle;

                float rnd = Random.Range(waitMin, waitMax);
                Invoke(nameof(MoveNextPatrol), rnd);
            }
        }
    }

    void SetWaitFalse()
    {
        wait = false;
    }

    protected void Patrol()
    {
        if (patrol.Length == 0)
            return;

        if (!wait)
        {
            if (agent.remainingDistance < 0.5f)
            {
                wait = true;
                state = MonsterState.idle;

                float rnd = Random.Range(waitMin, waitMax);
                Invoke(nameof(MoveNextPatrol), rnd);
            }
        }
    }
    public void MoveNextPatrol()
    {
        state = MonsterState.patrol;
        Transform target = patrol[patrolIdx++];
        patrolIdx %= patrol.Length;

        agent.SetDestination(target.position);
        wait = false;
    }

    public MonsterState GetState()
    {
        return state;
    }
    public void SetState(MonsterState state)
    {
        this.state = state;
    }
    protected virtual void StateControl()
    {
        if (state == MonsterState.idle)
        {
            agent.isStopped = true;
        }
        else if(state == MonsterState.move)
        {
            agent.isStopped = false;
            MoveTo(movePosition);
        }
        else if(state == MonsterState.patrol)
        {
            agent.isStopped = false;
            Vector3 pos = transform.position;
            pos.y += 1;
            
            if(Physics.Raycast(pos, transform.forward, out hit, 2.0f))
            {
                if(hit.collider.CompareTag("Door"))
                {
                    var panels = GameObject.FindGameObjectsWithTag("Panel");
                    float panelDisSqr = (panels[0].transform.position - transform.position).sqrMagnitude;
                    int panelIdx = 0;
                    for (int i = 0; i < panels.Length; ++i)
                    {
                        float dis = (panels[i].transform.position - transform.position).sqrMagnitude;
                        if (dis < panelDisSqr)
                        {
                            panelDisSqr = dis;
                            panelIdx = i;
                        }
                    }
                    var panel = panels[panelIdx].GetComponent<DoorPanel>();
                    if(panel.door.IsOpen() == false)
                    {
                        panel.SetLock(false);
                        panel.Interact();
                    }
                }
            }
            Patrol();
        }
        else if(state == MonsterState.hunt)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);

            if(playerDisSqr < attackRange * attackRange)
            {
                SetState(MonsterState.attack);
            }
        }
        else if(state == MonsterState.attack)
        {
            agent.isStopped = false;
            playerHP.Damage(damage);
        }

    }

    public void PlayIdleAudio()
    {
        if (audio == null)
            return;

        if(!audio.isPlaying)
        {
            int rnd = Random.Range(0, idleSounds.Length);
            audio.clip = idleSounds[rnd];
            audio.Play();
        }
    }

    public void PlayFootStepAudio()
    {
        if (footAudio == null || !footAudio.gameObject.activeSelf)
            return;

        footAudio.clip = footStepSounds[footStepIdx++];
        footStepIdx %= footStepSounds.Length;

        footAudio.Play();
    }

    public float GetPlayerDistanceSqr()
    {
        return playerDisSqr;
    }
}
