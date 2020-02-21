using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//对象枚举
public enum EnityType
{
    Enemy,
    Hero,
    Bomb,
    NULL
}
public class EnityManager
{
    // Start is called before the first frame update
    public GameObject[] EnitysInGame;

    public GameObject[] EnityPrefabs;//预制的怪物列表
    public HPManager hpManager;
    private int createEnityIndex = 0;
    private static EnityManager sEnityManager;
    public static EnityManager GetInstance()
    {
        if (sEnityManager == null)
        {
            sEnityManager = new EnityManager();
        }
        return sEnityManager;
    }
    public EnityManager()
    {
        GameObject gameObject = GameObject.Find("HPLayer");
        if(gameObject != null)
        {
            hpManager = gameObject.GetComponent<HPManager>();
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 获取当前场景内所有目标对象
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public int GetEnitysCountInGame(EnityType enemy)
    {
        EnitysInGame = GameObject.FindGameObjectsWithTag(enemy.ToString());//遍历找出所有怪物
        return EnitysInGame.Length;
    }
    public GameObject CreateEnity(EnityType enityType,string enityName)
    {
        GameObject obj = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", enityName);
        GameObject AI = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", "AI");
        obj.transform.SetParent(AI.transform, false);
        AIBase AIScript = AI.GetComponent<AIBase>();
        AIScript.selfType = enityType;
        AIScript.attackType = EnityType.Hero;
        AIScript.isCanMove = true;
        AIScript.searchRadius = 25.0f;
        AIScript.attackRadius = 1.0f;
        AIScript.moveSpeed = 0.05f;
        AIScript.moveAnimatorType = AnimatorParameters.Walk;
        AIScript.animator = obj.GetComponent<Animator>();
        AI.name = enityName + createEnityIndex;
        createEnityIndex++;
        return AI;
    }
    public GameObject CreateFightHero(Hero hero)
    {
        GameObject AI = null;
        HeroTableData heroTableData = DataManager.GetInstance().GetHeroTableDataByHero(hero);
        if (heroTableData != null)
        {
            GameObject obj = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", heroTableData.heroPrefab);
            Enity enity = obj.GetComponent<Enity>();
            if(enity != null)
            {
                enity.InitEnityByHero(hero);
            }
            AI = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", "AI");
            obj.transform.SetParent(AI.transform, false);
            AIBase AIScript = AI.GetComponent<AIBase>();
            AIScript.InitAIBaseByEnity(enity);
            AI.name = heroTableData.name + createEnityIndex;
            createEnityIndex++;
        }
        return AI;
    }
    public EnityType GetAttackEnityType(EnityType enityType)
    {
        EnityType attackEnityType = EnityType.NULL;
        switch (enityType)
        {
            case EnityType.Enemy:
                attackEnityType = EnityType.Hero;
                break;
            case EnityType.Hero:
                attackEnityType = EnityType.Enemy;
                break;
            case EnityType.Bomb:
                attackEnityType = EnityType.Enemy;
                break;
            default:
                break;
        }
        return attackEnityType;
    }

}
