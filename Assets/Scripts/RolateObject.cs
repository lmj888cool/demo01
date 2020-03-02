using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolateObject : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 beginTouchPos;//触点
    Vector3 beginTouchPosMoblie;//触点
    bool startRolateObject = false;
    public bool isAuto = false;
    void Start()
    {
        beginTouchPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAuto)
        {
            /////////////////////////触摸屏////////////////////////
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount == 1) //[color=Red]如果点击手指touch了  并且手指touch的状态为移动的[/color]
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 touchposition = Input.GetTouch(0).position;  //[color=Red]获取手指touch最后一帧移动的xy轴距离[/color]
                    transform.Rotate(Vector3.up, beginTouchPosMoblie.x - touchposition.x);
                }
                beginTouchPosMoblie = Input.GetTouch(0).position;

            }
            else
            {
                beginTouchPosMoblie = Vector3.zero;
            }
#endif
            ////////////////////////////PC/////////////////////
            if (Input.GetButtonUp("Fire1"))
            {
                startRolateObject = false;//松开鼠标左键
            }

            if (Input.GetButtonDown("Fire1"))//按下鼠标左键
            {
                startRolateObject = true;
                beginTouchPos = Input.mousePosition;

            }
            if (startRolateObject)
            {
                print("nput.mousePosition.y - beginTouchPos.y:" + (Input.mousePosition.y - beginTouchPos.y));
                transform.Rotate(Vector3.up, (beginTouchPos.x - Input.mousePosition.x) * 2.0f);
                beginTouchPos = Input.mousePosition;
            }
        }
        else
        {
            transform.Rotate(Vector3.up, 1.0f);
        }

    }
    private void OnDisable()
    {
        startRolateObject = false;
    }
}
