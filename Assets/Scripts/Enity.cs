﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Sorceress 魔女
//Archer 射手
[Serializable]
public enum DummyProp
{
    Head,//背部装饰，比如翅膀，背包等等
    Back,//头部装饰，比如头盔，胡子，面具，发带等等
    Chest,//胸部装饰，比如护甲等等
    Left,//左手道具，比如武器
    Right,//右手道具，比如武器
    Effect,//实体周边特效
    NULL
}
//实体基础类，英雄或怪物继承此类
public class Enity : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> dummyProp_Parent = new List<Transform>();//道具的父节点
    private Dictionary<DummyProp, GameObject>  dummyProp_Enity = new Dictionary<DummyProp, GameObject>();//道具

    public GameObject _LoadObj_Pre;
    public HeroTableData heroTableData;
    public Hero hero;
    public EnityType enityType;
    private List<Animator> animatorList = new List<Animator>();
    private bool isFly = false;
    void Start()
    {
        //dummyProp_Enity = new Dictionary<DummyProp, GameObject>();
        //1、资源加载的第一种方式，直接在面板拖拽，通常不使用
        //if (_LoadObj_Pre != null)
        //{
        //    //实例化预设物体
        //    GameObject obj = Instantiate(_LoadObj_Pre);
        //    //修改加载物体的名称
        //    obj.name = "第一种拖拽资源加载方式";
        //}

        //2、资源加载的第二种方式，使用Resources.Load加载资源
        //（注意预设需要放置在Resources目录下面，这个目录有限制，
        //最大只能加载2G的资源内容,一般不建议使用）
        //GameObject loadObj = Instantiate(Resources.Load("Prefabs/cube")) as GameObject;
        //loadObj.name = "第二种Resources资源加载方式";

        //3、资源加载的第三种方式，使用AssetBundle加载的方式加载(常用方式)
        //AssetBundle assetBundleObj = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/cube");
        //GameObject abObj = Instantiate(assetBundleObj.LoadAsset<GameObject>("cube"));
       //abObj.name = "第三种AB资源加载方式";

        //4、资源加载的第四种方式，使用AssetDatabase.LoadAssetAtPath加载资源(编辑器框架开发使用)
        //GameObject DBobj = Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Little Heroes Mega Pack/Prefabs/Customized Character Samples/Adventurer Male 01.prefab"));
        //DBobj.name = "第四种DB资源加载方式";
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void InitEnityByHero(Hero hero)
    {
        this.hero = hero;
        heroTableData = DataManager.GetInstance().GetHeroTableDataByHero(hero);
        enityType = heroTableData.id < 10000 ? EnityType.Hero : EnityType.Enemy;
        Animator animator = gameObject.GetComponent<Animator>();
        if(animator != null)
        {
            animatorList.Add(animator);
        }
        UpdateHeroEquips();

        OnPlayAnimation("idle");


    }
    public Item GetEnityEquip()
    {
        if(hero != null)
        {
            ///////先判断右手是否有武器，左手一般是防御武器////////////
            if (hero.dummyPropDic.ContainsKey(DummyProp.Right))
            {
                int itemid = hero.dummyPropDic[DummyProp.Right];

                return DataManager.GetInstance().GetGameData().GetItemById(itemid);
            }else if (hero.dummyPropDic.ContainsKey(DummyProp.Left))
            {
                int itemid = hero.dummyPropDic[DummyProp.Left];
                Item item = DataManager.GetInstance().GetGameData().GetItemById(itemid);
                if(item != null)
                {
                    ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItem(item);
                    if(itemTableData.damaged > 0)//判断左手的武器是否有伤害
                    {
                        return item;
                    }

                }
            }
        }
        return null;
    }
    public void UpdateHeroEquips()
    {
        
        Dictionary<int, Item> items = DataManager.GetInstance().GetGameData().Items;
        if(items.Count > 0)
        {
            foreach (KeyValuePair<DummyProp, int> item in hero.dummyPropDic)
            {
                int itemid = item.Value;
                if (items.ContainsKey(itemid))
                {
                     AddEquipsByDummyProp(itemid);
                }
        }
        }
       
    }
    public void AddEquipsByDummyProp(int itemid,bool isupdate = false)
    {
        Item item = DataManager.GetInstance().GetGameData().GetItemById(itemid);

        ItemTableData equipdata = DataManager.GetInstance().GetItemTableDataByItemId(item.itemId);
        if (equipdata != null)
        {
            DummyProp dummyProp = (DummyProp)equipdata.group;
            if (dummyProp_Enity.ContainsKey(dummyProp))
            {
                GameObject obj = dummyProp_Enity[dummyProp];
                dummyProp_Enity.Remove(dummyProp);
                Destroy(obj);
            }
            GameObject equip = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", equipdata.itemPrefab);
            if (equip != null && dummyProp_Parent.Count > (int)dummyProp)
            {
                dummyProp_Enity[dummyProp] = equip;
                equip.transform.SetParent(dummyProp_Parent[(int)dummyProp], false);
            }
            if (dummyProp == DummyProp.Chest)// 如果是护甲，获取护甲的动作类，需要一同播放
            {
                Animator animatorChest = equip.GetComponent<Animator>();
                if (animatorChest!= null && !animatorList.Contains(animatorChest))
                {
                    animatorList.Add(animatorChest);
                }

            }
            if (dummyProp == DummyProp.Back)//如果是翅膀，改成飞行状态
            {
                isFly = true;
                //OnPlayAnimation(AnimatorParameters.FlyIdle, ActionParamtersType.Bool);
            }

            if (isupdate)//刷新装备
            {
                //Hero hero = DataManager.GetInstance().GetGameData().GetHeroById(heroId);
                if (hero != null)
                {
                    Item olditem = null;
                    Hero oldmaster = null;
                    //先把当前装备脱下
                    if (hero.dummyPropDic.ContainsKey(dummyProp))
                    {
                        int olditemid = hero.dummyPropDic[dummyProp];
                        olditem = DataManager.GetInstance().GetGameData().GetItemById(olditemid);
                        //olditem.masterId = 0;
                    }
                    hero.dummyPropDic[dummyProp] = itemid;
                    if(item.masterId > 0)//之前有穿戴者，需要卸下来
                    {
                        oldmaster = DataManager.GetInstance().GetGameData().GetHeroById(item.masterId);
                        //if(oldmaster != null)
                        //{
                        //    oldmaster.dummyPropDic.Remove(dummyProp);
                        //}
                    }
                    if(olditem != null)
                    {
                        olditem.masterId = oldmaster != null ? oldmaster.id : 0;                       
                    }
                    if (oldmaster != null)
                    {
                        if(olditem != null)
                        {
                            oldmaster.dummyPropDic[dummyProp] = olditem.id;
                        }
                        else
                        {
                            oldmaster.dummyPropDic.Remove(dummyProp);
                        }
                    }
                    item.masterId = hero.id;

                    if(hero.teamPosition >=0)//如果再阵容需要更新战斗场景
                    {
                        //GameObject.Find("FightScene").SendMessage("InitFightingHero",null);
                    }
                }
            }
            

        }
    }
    public void ChangeEquip(int itemid)
    {
        AddEquipsByDummyProp(itemid,true);
        DataManager.GetInstance().SaveByBin();
    }
    public void OnPlayAnimation(AnimatorParameters _actionType, ActionParamtersType _ActionParamtersType, int _intValue = 1, float _floatValue = 0.0f)//播放或停止动作
    {
        for (int i = 0; i < animatorList.Count; i++)
        {
            Animator animator = animatorList[i];
            if (animator != null && animator.parameterCount > (int)_actionType)
            {
                string _animatorParameterName = animator.GetParameter((int)_actionType).name;
                switch (_ActionParamtersType)
                {
                    case ActionParamtersType.Float:
                        if (animator.GetFloat(_animatorParameterName) != _floatValue)
                            animator.SetFloat(_animatorParameterName, _floatValue);
                        break;
                    case ActionParamtersType.Int:
                        if (animator.GetInteger(_animatorParameterName) != _intValue)
                            animator.SetInteger(_animatorParameterName, _intValue);
                        break;
                    case ActionParamtersType.Bool:
                        if (animator.GetBool(_animatorParameterName) != _intValue > 0)
                            animator.SetBool(_animatorParameterName, _intValue > 0);
                        break;
                    case ActionParamtersType.Trigger:
                        animator.SetTrigger(_animatorParameterName);
                        break;
                    default:
                        break;
                }
            }
        }
      
    }
    public void OnPlayAnimation(string animationname)
    {
        //if (animator != null)
        {
            
            switch (animationname)
            {
                case "idle":
                    if (isFly)
                    {
                        OnPlayAnimation(AnimatorParameters.FlyIdle, ActionParamtersType.Bool, 1);
                    }
                    else
                    {
                        OnPlayAnimation(AnimatorParameters.Idle, ActionParamtersType.Bool, 1);
                    }
                               
                break;
                case "run":
                    if (isFly)
                    {
                        OnPlayAnimation(AnimatorParameters.FlyForward, ActionParamtersType.Bool, 1);
                    }
                    else
                    {
                        OnPlayAnimation(AnimatorParameters.Run, ActionParamtersType.Bool, 1);
                    }
                   
                    break;
                case "attack":
                    if (isFly)
                    {
                        OnPlayAnimation(AnimatorParameters.FlyMeleeRightAttack03, ActionParamtersType.Trigger, 1);
                    }
                    else
                    {
                        OnPlayAnimation(AnimatorParameters.MeleeRightAttack03, ActionParamtersType.Trigger, 1);
                    }
                    
                    break;
                case "dead":
                    if (isFly)
                    {
                        OnPlayAnimation(AnimatorParameters.FlyDie, ActionParamtersType.Bool, 1);
                    }
                    else
                    {
                        OnPlayAnimation(AnimatorParameters.Die, ActionParamtersType.Bool, 1);
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }
}