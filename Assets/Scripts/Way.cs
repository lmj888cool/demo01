using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    public GameObject WayGameObject;
    private bool HaveTriggerEnter = false;
    public Transform[] heroPosArr;
    public Transform[] monsterPosArr;
    // Start is called before the first frame update
    void Start()
    {
        //if(WayGameObject != null)
        //{
        //    GameObject enity = EnityManager.GetInstance().CreateEnity(EnityType.Enemy, "SimpleZombies_01_Blue");
        //    enity.transform.SetParent(WayGameObject.transform, false);
        //    enity.transform.position = new Vector3(WayGameObject.transform.position.x + Random.Range(0.0f, 15.0f),0, WayGameObject.transform.position.z + Random.Range(0.0f, 15.0f));
        //    GameObject enity2 = EnityManager.GetInstance().CreateEnity(EnityType.Enemy, "SimpleZombies_BeachBabe_Green");
        //    enity2.transform.SetParent(WayGameObject.transform, false);
        //    enity2.transform.position = new Vector3(WayGameObject.transform.position.x + Random.Range(0.0f, 15.0f), 0, WayGameObject.transform.position.z + Random.Range(0.0f, 15.0f));
        //    GameObject enity3 = EnityManager.GetInstance().CreateEnity(EnityType.Enemy, "SimpleZombies_Nazi_Grey");
        //    enity3.transform.SetParent(WayGameObject.transform, false);
        //    enity3.transform.position = new Vector3(WayGameObject.transform.position.x + Random.Range(0.0f, 15.0f), 0, WayGameObject.transform.position.z + Random.Range(0.0f, 15.0f));
        //}


    }
    public void Init()
    {
        for (int i = 0; i < heroPosArr.Length; i++)
        {
            heroPosArr[i].position = transform.parent.TransformPoint(heroPosArr[i].position);
        }
        for (int i = 0; i < monsterPosArr.Length; i++)
        {
            monsterPosArr[i].position = transform.parent.TransformPoint(monsterPosArr[i].position);
        }
    }
    public void InitMonster(string[] monsterArr)
    {
        if (WayGameObject != null)
        {
            for (int i = 0; i < monsterArr.Length; i++)
            {
                int monsterid = int.Parse(monsterArr[i]);
                HeroTableData heroTableData = DataManager.GetInstance().GetHeroTableDataByHeroId(monsterid);
                if (heroTableData != null)
                {
                    GameObject enity = EnityManager.GetInstance().CreateEnity(EnityType.Enemy, heroTableData.heroPrefab);
                    enity.transform.SetParent(WayGameObject.transform.parent.transform, false);
                    enity.transform.position = monsterPosArr[i].position;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (HaveTriggerEnter)
            return;
        if(other.gameObject.tag == EnityType.Hero.ToString())
        {
            HaveTriggerEnter = true;
            if(WayGameObject != null)
            {
                GameObject wayGameObjectCopy = Instantiate(WayGameObject, WayGameObject.transform.position, Quaternion.identity) as GameObject;
                wayGameObjectCopy.transform.position = new Vector3(WayGameObject.transform.position.x, WayGameObject.transform.position.y, WayGameObject.transform.position.z + 360);
                Destroy(WayGameObject, 2);
            }
            
        }
        
    }
}
