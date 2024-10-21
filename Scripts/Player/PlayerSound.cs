using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public PlayerController player;
    [SerializeField] ObjectPool soundPool;

    public AudioClip[] soundClips;

    [SerializeField] float StepVolume = 0.5f;
    [SerializeField] float RunVolume = 1.0f;
    [SerializeField] float SitVolume = 0.1f;

    [SerializeField] float stepTriggerSize = 5.0f;
    [SerializeField] float sitTriggerSize = 1.0f;
    [SerializeField] float runTriggerSize = 10.0f;

    PoolAble[] audioObj;
    SphereCollider trigger;

    int soundClipIdx = 0;

    float t = 0;

    bool beforeGround = true;
    void Awake()
    {
        if (soundPool == null)
            soundPool = GameObject.Find("SoundPool").GetComponent<ObjectPool>();

        audioObj = new PoolAble[soundClips.Length];
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        DestroyAudios();
        if (player.GetWalking())
        {
            t += Time.deltaTime;
            if (player.GetSit())
            {
                if (t >= 1.0f)
                {
                    t = 0.0f;
                    PlayFootStep(SitVolume, sitTriggerSize);
                }
            }
            else if (player.GetRun())
            {
                if(t >= 0.375f)
                {
                    t = 0.0f;
                    PlayFootStep(RunVolume, runTriggerSize);
                }
            }
            else
            {
                if(t >= 0.5f)
                {
                    t = 0.0f;
                    PlayFootStep(StepVolume, stepTriggerSize);
                }
            }
        }
        if(!beforeGround && (player.OnGround() || player.OnSlope()))
        {
            PlayFootStep(StepVolume, stepTriggerSize);
        }
        beforeGround = player.OnGround() || player.OnSlope();
    }

    void DestroyAudios()
    {
        for (int i = 0; i < audioObj.Length; i++)
        {
            if (audioObj[i] == null)
                continue;

            if (!audioObj[i].GetComponent<AudioSource>().isPlaying)
            {
                audioObj[i].Destroy();
                audioObj[i] = null;
            }
        }
    }

    void PlayFootStep(float volume, float triggerSize)
    {
        var sound = soundPool.GetObject();
        sound.transform.position = player.transform.position;
        audioObj[soundClipIdx] = sound;

        var source = sound.GetComponent<AudioSource>();
        source.volume = volume;
        source.clip = soundClips[soundClipIdx++];
        soundClipIdx %= soundClips.Length;
        source.Play();

        var trigger = sound.GetComponent<SphereCollider>();
        trigger.radius = triggerSize;
    }
}
