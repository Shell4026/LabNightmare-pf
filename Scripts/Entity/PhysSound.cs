using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysSound : MonoBehaviour
{
    [SerializeField] ObjectPool soundPool;
    public AudioClip[] hitSound;
    public float minSpeed = 1f;
    public float maxSpeed = 6f;
    [SerializeField] float maxTriggerSize = 10.0f;
    [Header("게임 시작 후 이 시간 까지는 작동x")]
    [SerializeField] float initTime = 1.5f;

    PoolAble[] audioObj;
    int audioIdx = 0;
    float volume = 0.0f;

    bool init = false;

    GameObject owner;
    private void Awake()
    {
        if(soundPool == null)
            soundPool = GameObject.Find("SoundPool").GetComponent<ObjectPool>();
    }
    void Start()
    {
        audioObj = new PoolAble[hitSound.Length];
        if (maxSpeed < 0)
            maxSpeed = 1;

        Invoke(nameof(Init), initTime);
    }

    void Init()
    {
        init = true;
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < audioObj.Length; i++)
        {
            var audio = audioObj[i];
            if (audio == null)
                continue;
            var source = audio.GetComponent<AudioSource>();
            if (!source.isPlaying)
            {
                audioObj[i] = null;
                audio.Destroy();
            }
        }

        if(owner != null)
        {
            Invoke(nameof(ResetOwner), 3.0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!init)
            return;
        if (soundPool == null)
            return;
        if (audioObj[audioIdx] != null)
            return;

        float v = collision.relativeVelocity.magnitude;
        if (v > minSpeed)
        {
            int contactCount = collision.contactCount;
            audioObj[audioIdx] = soundPool.GetObject(owner);
            audioObj[audioIdx].transform.position = collision.GetContact(0).point;

            var source = audioObj[audioIdx].GetComponent<AudioSource>();
            int rnd = Random.Range(0, hitSound.Length);
            source.clip = hitSound[rnd];
            source.Play();
            volume = (v - minSpeed) / (maxSpeed - minSpeed);
            volume = Mathf.SmoothStep(0.0f, 1.0f, volume);
            source.volume = volume;

            var trigger = audioObj[audioIdx].GetComponentInChildren<SphereCollider>();
            float t = (v - minSpeed) / (maxSpeed - minSpeed);
            trigger.radius = Mathf.Lerp(0, maxTriggerSize, t);

            ++audioIdx;
            audioIdx %= hitSound.Length;
        }
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public void ResetOwner()
    {
        owner = null;
    }

    public GameObject GetOwner()
    {
        return owner;
    }
}
