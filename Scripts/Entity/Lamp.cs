using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] Light light;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] Material offMaterial;

    [SerializeField] bool flicker = false;
    [Header("빛 깜빡임 범위")]
    public float flickerTimeStart = 2.0f;
    public float flickerTimeEnd = 5.0f;
    [Header("켜지는데 걸리는 시간")]
    public float onTime = 0.2f;

    float flickerRnd = 0.0f;
    float t = 0.0f;

    Material originalMat;
    void Start()
    {
        if(renderer != null)
            originalMat = renderer.material;

        flickerRnd = Random.Range(flickerTimeStart, flickerTimeEnd);
    }

    void Update()
    {
        if (light == null)
            return;

        if (flicker)
        {
            t += Time.deltaTime;
            if(t >= flickerRnd)
            {
                light.enabled = false;
                if(renderer != null && offMaterial != null)
                    renderer.material = offMaterial;
                flickerRnd = Random.Range(flickerTimeStart, flickerTimeEnd);
                Invoke(nameof(LightOn), onTime);

                t = 0;
            }
        }
    }

    void LightOn()
    {
        if(originalMat != null)
            renderer.material = originalMat;
        light.enabled = true;
    }
}
