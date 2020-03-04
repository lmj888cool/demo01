using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    public GameObject WayGameObject;
    private bool HaveTriggerEnter = false;
    public Transform[] heroPosArr;
    public Transform[] monsterPosArr;
    public Transform cameraPos;
    public float wayLenght = 120.0f;//路的长度
    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }
    public void Init()
    {
        //for (int i = 0; i < heroPosArr.Length; i++)
        //{
        //    heroPosArr[i].position = transform.TransformPoint(heroPosArr[i].position);
        //}
        //for (int j = 0; j < monsterPosArr.Length; j++)
        //{
        //    monsterPosArr[j].position = transform.TransformPoint(monsterPosArr[j].position);
        //}
        //cameraPos.position = transform.TransformPoint(cameraPos.position);
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
                ChapterTableData chapterTableData = DataManager.GetInstance().GetChapterTableDataById(DataManager.GetInstance().GetGameData().ChapterId);
                if (chapterTableData != null)
                {
                    string split = "_";
                    InitMonster(chapterTableData.monsters.Split(split.ToCharArray()));

                }
                

                //GameObject wayGameObjectCopy = Instantiate(WayGameObject, WayGameObject.transform.position, Quaternion.identity) as GameObject;
                //wayGameObjectCopy.transform.position = new Vector3(WayGameObject.transform.position.x, WayGameObject.transform.position.y, WayGameObject.transform.position.z + 360);
                //Destroy(WayGameObject, 2);
            }
            
        }
        
    }
}
