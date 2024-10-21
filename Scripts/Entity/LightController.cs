using System.Collections;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private bool isLightOn = true;
    private GameObject[] lights;

    void Start()
    {
        // "Light" 태그를 가진 모든 오브젝트를 한 번만 찾음
        lights = GameObject.FindGameObjectsWithTag("Light");

        // 시작할 때 불을 켜는 상태로 설정
        Toggle(true);
        StartCoroutine(ToggleLightRoutine());
    }

    void Toggle(bool state)
    {
        isLightOn = state;

        // 이미 찾은 오브젝트들의 상태를 설정
        foreach (GameObject light in lights)
        {
            light.SetActive(isLightOn);
        }

    }

    public bool GetState()
    {
        return isLightOn;
    }

    IEnumerator ToggleLightRoutine()
    {
        while (true)
        {
            // 4~6초 동안 기다린 후 불을 끔
            yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 6f));
            Toggle(false);

            // 0.5~1초 동안 기다린 후 불을 켬
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));
            Toggle(true);
        }
    }
}
