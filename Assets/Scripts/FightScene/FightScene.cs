using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Way;//跑道
    public float moveSpeed = 0.08f;
    public Transform SceneCamera;
    public Transform followHero;
    private Vector3 followHeroPos;
    private Dictionary<int, Vector3> teamPostionDic;
    private List<GameObject> FightHeros = new List<GameObject>();
    private int ChapterId;
    private int MapId;
    private ChapterTableData mChapterTableData;
    private Transform[] heroStartPos = null;
    public IndexBtn[] indexBtns;
    public ChangeMap changeMap;
    void Start()
    {

        
        //先初始化地图
        ChapterId = DataManager.GetInstance().GetGameData().ChapterId;
        InitFightChapter();
        InitIndexBtns();
        /////////////////必须初始化地图/////////////////////////

        //teamPostionDic = new Dictionary<int, Vector3>
        //{
        //    [0] = new Vector3(1.70f, 0,heroStartPos.z - 50f),
        //    [1] = new Vector3(1.22f, 0,heroStartPos.z - 50f),
        //    [2] = new Vector3(0.19f,0, heroStartPos.z - 50f),
        //    [3] = new Vector3(-1.44f,0, heroStartPos.z - 50f)
        //};
        InitFightingHero();
        //////////////////////////////////////////////
        if (SceneCamera != null && followHero != null)
        {
            followHeroPos = followHero.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneCamera != null && followHero!= null)
        {
            Vector3 movePos = followHero.position - followHeroPos;
            SceneCamera.position += new Vector3(0, movePos.y, movePos.z);
            followHeroPos = followHero.position;
        }
    }
    public void InitFightingHero()
    {
        Dictionary<long, Hero> heroes = DataManager.GetInstance().GetGameData().Heroes;

        foreach (KeyValuePair<long,Hero> heropair in heroes)
        {
            if(heropair.Value.teamPosition > -1)//必须在阵容
            {
                HeroTableData heroTableData = DataManager.GetInstance().GetHeroTableDataByHero(heropair.Value);
                if (heroTableData != null)
                {
                    GameObject fighthero = EnityManager.GetInstance().CreateFightHero(heropair.Value);
                    fighthero.transform.SetParent(transform, false);
                    if(heroStartPos != null && heroStartPos.Length > heropair.Value.teamPosition)
                    {
                        fighthero.transform.position = heroStartPos[heropair.Value.teamPosition].position;
                    }                      
                    //if (heropair.Value.teamPosition == 2)
                    {
                        followHero = fighthero.transform;
                    }
                    FightHeros.Add(fighthero);
                }
            }
           
        }
    }
    public void InitIndexBtns()
    {
        if (indexBtns != null)
        {
            int huf = indexBtns.Length / 2;
            int startchapterid = ChapterId > huf ? ChapterId - huf : 1;
            for (int i = 0; i < indexBtns.Length; i++)
            {
                indexBtns[i].InitByChapterId(startchapterid + i);
            }
        }
    }
    public string GetMapName()
    {
        return "Way"+ MapId.ToString();
    }
    public void InitFightChapter(bool isSetCameraPos = true)
    {
        mChapterTableData = DataManager.GetInstance().GetChapterTableDataById(ChapterId);
        MapId = mChapterTableData.mapId;
        if (changeMap != null)
        {
            changeMap.Init(mChapterTableData.mapName, mChapterTableData.mapId.ToString());
        }

        for (int i = 0; i < 3; i++)
        {
            Way wayScript = CreateWay(i);
            if (i == 0)
            {
                heroStartPos = wayScript.heroPosArr;
                if (SceneCamera != null && isSetCameraPos)
                {
                    SceneCamera.position = wayScript.cameraPos.position;
                }
            }
        }
        
    }
    private void OnEnable()
    {
        //if(FightHeros != null)
        {
            for (int i = 0; i < FightHeros.Count; i++)
            {
                Enity fighthero = FightHeros[i].GetComponentInChildren<Enity>();
                if(fighthero != null)
                {
                    fighthero.UpdateHeroEquips();
                }
            }
        }
        
    }
    public void OnWinTest()
    {
        GameObject[] _gameObjects = GameObject.FindGameObjectsWithTag(EnityType.Enemy.ToString());
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            AIBase AIScript = _gameObjects[i].GetComponent<AIBase>();
            if(AIScript != null)
            {
                AIScript.KillSelf();
            }
        }
        OnWin();
    }
    public Way CreateWay(int index)
    {
        Way wayScript = null;
        string wayname = GetMapName();
        GameObject way = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", wayname);
        if (way != null)
        {
            wayScript = way.GetComponentInChildren<Way>();
            if (wayScript != null)
            {
                way.transform.SetParent(transform, false);
                way.gameObject.tag = "Way";
                Vector3 pos = Vector3.zero;
                pos.z = wayScript.wayLenght * (mChapterTableData.subId + index -1);
                //pos.y = -wayScript.wayLenght * (MapId - 1);
                way.transform.position = pos;
            }
        }
        return wayScript;
    }
    public void ClearWay()
    {
        //string wayname = GetMapName();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Way");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
    public void OnWin()
    {
        DataManager.GetInstance().GetGameData().ChapterId++;
        DataManager.GetInstance().SaveByBin();
        ChapterId = DataManager.GetInstance().GetGameData().ChapterId;
        mChapterTableData = DataManager.GetInstance().GetChapterTableDataById(ChapterId);
        if (mChapterTableData != null)
        {
            InitIndexBtns();
            if (MapId == mChapterTableData.mapId)//说明在同一地图
            {
                CreateWay(2);
            }
            else
            {
                //进入下一地图

                ClearWay();

                InitFightChapter(false);
                for (int i = 0; i < FightHeros.Count; i++)
                {
                    Vector3 pos = FightHeros[i].transform.position;
                    pos.z = heroStartPos[i].position.z;
                    pos.x = heroStartPos[i].position.x;
                    FightHeros[i].transform.position = pos;
                }
            }
            
        }
    }
}
