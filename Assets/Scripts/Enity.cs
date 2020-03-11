using System.Collections;
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
    public GameObject materialBody;

    public GameObject _LoadObj_Pre;
    public HeroTableData heroTableData;
    public Hero hero = null;
    public Monster monster = null;
    public EnityType enityType;
    private List<Animator> animatorList = new List<Animator>();
    private bool isFly = false;//是否是飞行状态
    private bool isSpear = false;//是否拿着长矛
    private AnimationTableData animationTableData;
    public AnimatorAction animatorAction = AnimatorAction.Idle;//当前的动作状态
    private AnimatorParameters animatorParameters = AnimatorParameters.NULL;//当前勾选的动作参数
    void Start()
    {
        
    }
    void Update()
    {

    }
    public void InitEnityByHero(Hero hero)
    {

        this.hero = hero;
        enityType = EnityType.Hero;
        foreach (KeyValuePair<HeroPart,int> part in hero.heroPartDic)
        {
            DIYTableData dIYTableData = DataManager.instance.GetDIYTableDatasById(part.Value);
            if(dIYTableData != null)
            {
                if (dIYTableData.dummyProp != -1)
                {
                    GameObject diyPrefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", dIYTableData.prefab);
                    if (diyPrefab != null)
                    {
                        diyPrefab.transform.SetParent(dummyProp_Parent[(int)dIYTableData.dummyProp], false);
                    }
                }
                else
                {
                    Material material = DataManager.GetInstance().CreateMaterialFromAssetsBundle("enemy", dIYTableData.prefab);
                    SkinnedMeshRenderer skinnedMeshRenderer = materialBody.GetComponent<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer != null && material != null)
                    {
                        skinnedMeshRenderer.material = material;

                    }
                }
            }

            
        }
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null)
        {
            animatorList.Add(animator);
        }
        UpdateHeroEquips();

        OnPlayAnimation(animatorAction);


    }
    public void InitEnityByMonster(Monster monster)
    {

        this.monster = monster;
        enityType = EnityType.Enemy;
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null)
        {
            animatorList.Add(animator);
        }
        UpdateHeroEquips();

        OnPlayAnimation(animatorAction);


    }
    public Item GetEnityEquip()
    {
        if(hero != null)
        { 
            ///////先判断右手是否有武器，左手一般是防御武器////////////
            if (hero.dummyPropDic.ContainsKey(DummyProp.Right))
            {
                long itemid = hero.dummyPropDic[DummyProp.Right];

                return DataManager.GetInstance().GetGameData().GetItemById(itemid);
            }
            //else if (hero.dummyPropDic.ContainsKey(DummyProp.Left))
            //{
            //    int itemid = hero.dummyPropDic[DummyProp.Left];
            //    Item item = DataManager.GetInstance().GetGameData().GetItemById(itemid);
            //    if(item != null)
            //    {
            //        ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItem(item);
            //        if(itemTableData.damaged > 0)//判断左手的武器是否有伤害
            //        {
            //            return item;
            //        }

            //    }
            //}
        }
        return null;
    }
    public void UpdateHeroEquips()
    {
        
        Dictionary<long, Item> items = DataManager.GetInstance().GetGameData().Items;
        if(items.Count > 0)
        {
            foreach (KeyValuePair<DummyProp, long> item in hero.dummyPropDic)
            {
                long itemid = item.Value;
                if (items.ContainsKey(itemid))
                {
                    UpdateEquipsByDummyProp(itemid);
                }
            }
        }
        if (animationTableData == null)
        {
            animationTableData = DataManager.GetInstance().GetAnimationTableDataById(1);
        }
       
    }
    public void UpdateEquipsByDummyProp(long itemid,bool isAdd = true,bool isupdate = false)
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
            if (isAdd)
            {
                GameObject equip = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", equipdata.itemPrefab);
                if (equip != null && dummyProp_Parent.Count > (int)dummyProp)
                {
                    dummyProp_Enity[dummyProp] = equip;
                    equip.transform.SetParent(dummyProp_Parent[(int)dummyProp], false);
                }
                if (dummyProp == DummyProp.Chest)// 如果是护甲，获取护甲的动作类，需要一同播放
                {
                    Animator animatorChest = equip.GetComponent<Animator>();
                    if (animatorChest != null && !animatorList.Contains(animatorChest))
                    {
                        animatorList.Add(animatorChest);
                    }

                }
                if (dummyProp == DummyProp.Back)//如果是翅膀，改成飞行状态
                {
                    //isFly = true;
                    //OnPlayAnimation(AnimatorParameters.FlyIdle, ActionParamtersType.Bool);
                }
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
                        long olditemid = hero.dummyPropDic[dummyProp];
                        olditem = DataManager.GetInstance().GetGameData().GetItemById(olditemid);
                        hero.dummyPropDic.Remove(dummyProp);
                        //olditem.masterId = 0;
                    }
                    
                    if (isAdd)
                    {
                        hero.dummyPropDic[dummyProp] = itemid;
                        if (item.masterId > 0)//之前有穿戴者，需要卸下来
                        {
                            oldmaster = DataManager.GetInstance().GetGameData().GetHeroById(item.masterId);
                            //if(oldmaster != null)
                            //{
                            //    oldmaster.dummyPropDic.Remove(dummyProp);
                            //}
                        }
                        item.masterId = hero.id;
                    }
                    if (olditem != null)
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
                   

                    if(hero.teamPosition >=0)//如果再阵容需要更新战斗场景
                    {
                        //GameObject.Find("FightScene").SendMessage("InitFightingHero",null);
                    }
                }
            }

            if(dummyProp == DummyProp.Left || dummyProp == DummyProp.Right)//更新动作
            {
                Item itemEquip = GetEnityEquip();
                if(itemEquip != null)
                {
                    ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItem(itemEquip);
                    if(itemTableData != null)
                    {
                        animationTableData = DataManager.GetInstance().GetAnimationTableDataById(itemTableData.animationId);
                        if (isupdate)
                        {
                            OnPlayAnimation(animatorAction);
                        }
                    }
                   
                }
            }
            

        }
    }
    public void ChangeEquip(long itemid)
    {
        UpdateEquipsByDummyProp(itemid,true,true);
        OnPlayAnimation(animatorAction);
        DataManager.GetInstance().SaveByBin();
    }
    public void RemoveEquip(long itemid)
    {
        UpdateEquipsByDummyProp(itemid,false, true);
        OnPlayAnimation(animatorAction);
        DataManager.GetInstance().SaveByBin();
    }
    public void OnPlayAnimation(AnimatorParameters _actionType, ActionParamtersType _ActionParamtersType, int _intValue = 1, float _floatValue = 0.0f)//播放或停止动作
    {
        for (int i = 0; i < animatorList.Count; i++)
        {
            Animator animator = animatorList[i];
            if (animator != null)
            {
                if (animator.parameterCount > (int)_actionType)
                {
                    if (_ActionParamtersType != ActionParamtersType.Trigger)
                    {
                        animatorParameters = _actionType;
                    }
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
                else
                {
                    animator.Play("Idle");
                }

            }
            
        }
      
    }
    public AnimatorParameters GetAnimatorParametersByName(string name)
    {
        AnimatorParameters animatorParameters = AnimatorParameters.NULL;
        foreach (AnimatorParameters i in Enum.GetValues(typeof(AnimatorParameters)))
        {
            if (i.ToString() == name)
            {
                animatorParameters = i;
            }
        }
        return animatorParameters;
    }
    public void OnPlayAnimation(AnimatorAction _actionType, int _intValue = 1, float _floatValue = 0.0f)
    {
        if(animatorParameters != AnimatorParameters.NULL)//说明当前有一个动作被勾选，需要重置
        {
            OnPlayAnimation(animatorParameters, ActionParamtersType.Bool, 0);
            animatorParameters = AnimatorParameters.NULL;
        }
        animatorAction = _actionType;
        //先重置为idle状态，比如飞行状态播放其他动作必须flyidle始终勾选
        OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Idle), ActionParamtersType.Bool, _intValue);
        switch (_actionType)
        {
            case AnimatorAction.Idle:
                    OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Idle), ActionParamtersType.Bool, _intValue);
                break;
            case AnimatorAction.Walk:
                    OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Walk), ActionParamtersType.Bool, _intValue);
                break;
            case AnimatorAction.Run:
                    OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Run), ActionParamtersType.Bool, _intValue);
                break;
            case AnimatorAction.Attack:
                    OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Attack), ActionParamtersType.Trigger, _intValue);
                break;
            case AnimatorAction.Die:
                    OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Die), ActionParamtersType.Trigger, _intValue);
                break;
            case AnimatorAction.Hit:
                OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Hit), ActionParamtersType.Trigger, _intValue);
                break;
            case AnimatorAction.Stunned:
                OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Stunned), ActionParamtersType.Bool, _intValue);
                break;
            case AnimatorAction.Victory:
                OnPlayAnimation(GetAnimatorParametersByName(animationTableData.Victory), ActionParamtersType.Bool, _intValue);
                break;
            default:
                break;
        }
    }
}
