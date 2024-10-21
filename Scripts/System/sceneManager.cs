using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class sceneManager : MonoBehaviour
{
    public GameObject endobject;
    public GameObject player;
    int currentSceneIndex=0;
    // Start is called before the first frame update
    void Start()
    {
        if (0==SceneManager.GetActiveScene().buildIndex) { }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadStage(int currentScene)
    {
        SceneManager.LoadScene(currentScene);
        // 마지막 씬인 경우 첫 번째 씬으로 돌아갑니다.
/*        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }*/
    }

    public void NextStage()
    {
        currentSceneIndex = ReturnSceneIndex();
        currentSceneIndex = currentSceneIndex + 1;
        LoadStage(currentSceneIndex);

    }
    public void LoadMain() { LoadStage(0); }
    public void Load1Stage() { LoadStage(1); }
    public void Load2Stage() { LoadStage(2); }
    public void Load3Stage() { LoadStage(3); }
    public void LoadTutorialStage() { LoadStage(4); }
    public void LoadCurrentStage() { LoadStage(ReturnSceneIndex()); }
    public void Quit1() { Debug.Log("1.sceneCount); "); }
    public int ReturnSceneIndex() { return SceneManager.GetActiveScene().buildIndex; }

    /*    void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                m_IsPlayerAtExit = true;
            }
        }*/
    /*    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
        {
            if (!m_HasAudioPlayed)
            {
                audioSource.Play();
                m_HasAudioPlayed = true;
            }
            m_Timer += Time.deltaTime;
            imageCanvasGroup.alpha = m_Timer / fadeDuration;
            if (m_Timer > fadeDuration + displayImageDuration)
            {
                if (doRestart)
                {
                    SceneManager.LoadScene(0);
                }
                else { Application.Quit(); }

            }
        }*/
}
