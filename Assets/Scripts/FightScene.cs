using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int SubId;
    private ChapterTableData mChapterTableData;
    private Vector3 heroStartPos = Vector3.zero;
    void Start()
    {

        
        //先初始化地图
        ChapterId = DataManager.GetInstance().GetGameData().ChapterId;
        SubId = DataManager.GetInstance().GetGameData().SubId;
        InitFightChapter();
        /////////////////必须初始化地图/////////////////////////
        teamPostionDic = new Dictionary<int, Vector3>
        {
            [0] = new Vector3(1.70f, 0,heroStartPos.z - 50f),
            [1] = new Vector3(1.22f, 0,heroStartPos.z - 50f),
            [2] = new Vector3(0.19f,0, heroStartPos.z - 50f),
            [3] = new Vector3(-1.44f,0, heroStartPos.z - 50f)
        };
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
            SceneCamera.position += new Vector3(0, 0, movePos.z);
            followHeroPos = followHero.position;
        }
    }
    public void InitFightingHero()
    {
        Dictionary<int, Hero> heroes = DataManager.GetInstance().GetGameData().Heroes;

        foreach (KeyValuePair<int,Hero> heropair in heroes)
        {
            if(teamPostionDic.ContainsKey(heropair.Value.teamPosition))//必须在阵容
            {
                HeroTableData heroTableData = DataManager.GetInstance().GetHeroTableDataByHero(heropair.Value);
                if (heroTableData != null)
                {
                    GameObject fighthero = EnityManager.GetInstance().CreateFightHero(heropair.Value);
                    fighthero.transform.SetParent(transform, false);
                    fighthero.transform.position = teamPostionDic[heropair.Value.teamPosition];
                    if (heropair.Value.teamPosition == 2)
                    {
                        followHero = fighthero.transform;
                    }
                    FightHeros.Add(fighthero);
                }
            }
           
        }
    }
    public void InitFightChapter()
    {
        mChapterTableData = DataManager.GetInstance().GetChapterTableDataByChapterIdAndSubId(ChapterId, SubId);
        string wayname = "Way" + mChapterTableData.chapterId.ToString();
        for (int i = 0; i < 3; i++)
        {
            GameObject way = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", wayname);
            if (way != null)
            {
                way.transform.SetParent(transform, false);
                Vector3 pos = Vector3.zero;
                pos.z = 120.0f * mChapterTableData.chapterId * (mChapterTableData.subId - 1) + 120.0f * i;
                way.transform.position = pos;
                if (i== 0)
                {
                    heroStartPos = pos;
                }
            }
        }
        
    }
    private void OnEnable()
    {
        if(teamPostionDic != null)
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
}
