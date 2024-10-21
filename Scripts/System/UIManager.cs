using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Stage;
    public GameObject Main;
    public GameObject Option;
    public sceneManager SceneManager;

    private Stack<GameObject> uiHistory = new Stack<GameObject>();

    public GameObject player;

    bool boolmenu = false;
    bool boolStage = false;
    bool openUI = false;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager != null) {
            if (SceneManager.ReturnSceneIndex() == 0)
            {
                UIOpen(Main);
            }
            else { boolStage = true; }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boolStage) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!openUI) { UIOpen(Menu); }
                    
                else { UIClose(); }
            }
        }
    }
    void UIOpen(GameObject uiToOpen)
    {
        UIClose();
        if (player != null) {
            player.GetComponent<PlayerController>().StopCameraMoving(true);
            player.GetComponent<PlayerController>().LockCursor(false);
        }
        openUI = true;
        uiToOpen.SetActive(true);
        uiHistory.Push(uiToOpen);
    }

    public void GoBack()
    {
        if (uiHistory.Count > 0)
        {
            uiHistory.Pop();
            GameObject previousUI = uiHistory.Peek();
            UIOpen(previousUI);
        }
    }

    public void UIClose()
    {
        openUI = false;
        if (player != null) { 
            player.GetComponent<PlayerController>().StopCameraMoving(false);
            player.GetComponent<PlayerController>().LockCursor(true);
        }
            
        Menu.SetActive(false);
        Stage.SetActive(false);
        Main.SetActive(false);
        Option.SetActive(false);
    }

    public void MenuOpen() { UIOpen(Menu); }
    public void StageOpen() { UIOpen(Stage); }
    public void MainOpen() { UIOpen(Main); }
    public void OptionOpen() { UIOpen(Option); }

    public void Quit() {
        Application.Quit();
    }
}
