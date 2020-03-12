using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBtns : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject lastClickBtn;//最后点击的按钮
    public Transform[] sceneList;//场景摄像机们儿
    public int showSceneIndex=1;//进游戏后默认显示那个场景
    public Image selectBg;
    public Button[] menuBtnsArr;
    Transform currentScene;
    void Start()
    {
        //if(menuBtnsArr.Length > showSceneIndex)
        {
            OnClickBtn(showSceneIndex);
            //ShowSceneBySceneName(sceneList[showSceneIndex].gameObject.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickBtn(int index)
    {
        lastClickBtn = menuBtnsArr[index].gameObject;
        selectBg.transform.position = lastClickBtn.transform.position;
        string needChangeToSceneName = lastClickBtn.name + "Scene";//需要切换到的场景名
        ShowSceneBySceneName(needChangeToSceneName);
    }
    void ShowSceneBySceneName(string sceneName)
    {
        bool haveThisScene = false;
        for (int i = 0; i < sceneList.Length; i++)
        {
            Transform scene = sceneList[i];
            if (scene != null)
            {
                string cameraName = scene.gameObject.name;//摄像机场景名
                if (cameraName == sceneName)
                {
                    haveThisScene = true;
                    scene.gameObject.SetActive(true);
                    //Transform[] transforms = scene.gameObject.GetComponents<Transform>();
                    //for (int j = 0; j < transforms.Length; j++)
                    //{
                    //    transforms[j].gameObject.SetActive(true);
                    //}
                    currentScene = scene;
                }
            }

        }
        if (haveThisScene)
        {
            for (int i = 0; i < sceneList.Length; i++)
            {
                Transform scene = sceneList[i];
                if (scene != null)
                {
                    string cameraName = scene.gameObject.name;//摄像机场景名
                    if (cameraName != sceneName)
                    {
                        scene.gameObject.SetActive(false);
                        //Transform[] transforms = scene.gameObject.GetComponents<Transform>();
                        //for (int j = 0; j < transforms.Length; j++)
                        //{
                        //    transforms[j].gameObject.SetActive(false);
                        //}
                        
                    }

                }

            }
        }
        
    }
}
