using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PanelType
{
    HeroPanel,
    ChangeEquipPanel
}
public class HeroScene : MonoBehaviour
{
    // Start is called before the first frame update
    //public List<GameObject> LCustomizedCharacterSamples;
    public GameObject HeroChair;
    public Enity CurrentHero;
    public GameObject[] LCustomizedCharacterSamples;
    public GameObject Prefab;
    public GameObject HeroPanel;
    public ChangeEquipPanel ChangeEquipPanel;
    public int HeroIndex = 78;
    public ScrollRect HeroHeadScrollView;
    public Button[] EquipsSlot;//装备槽
    private List<ItemIcon> Equips = new List<ItemIcon>();//装备图标
    public Dropdown dropDown;
    private AnimatorAction currentAnimatorAction;
    void Start()
    {
        //LCustomizedCharacterSamples = DataManager.GetInstance().GetGameObjectsByPath("Prefabs/Customized Character Samples", ".prefab");
        if(Prefab != null)
        {
            LCustomizedCharacterSamples = GameObject.FindGameObjectsWithTag("Prefab_Hero");
        }
        // SetCuurentHero();
        currentAnimatorAction = AnimatorAction.Idle;
        ShowPanel(PanelType.HeroPanel);
        InitHeroHead();
        InitDropDown();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InitHeroHead()
    {
        if(HeroHeadScrollView != null)
        {
            Dictionary<int,Hero> heroes = DataManager.GetInstance().GetGameData().Heroes;
            foreach (KeyValuePair<int,Hero> heroPair in heroes)
            {
                if (heroPair.Value.teamPosition > -1)
                {
                    GameObject gameObject = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", "HeroIcon");
                    if (gameObject != null)
                    {
                        gameObject.transform.SetParent(HeroHeadScrollView.content, false);
                        HeroIcon head = gameObject.GetComponent<HeroIcon>();
                        head.InitData(heroPair.Value);
                    }
                    if(heroPair.Value.teamPosition == 0)
                    {
                        HeroIndex = heroPair.Key;
                    }
                }
            }
            //if (heroes.Count > 0)
            {
                SetCuurentHero();
            }
            
        }
    }
    public void GetFrontHero()
    {
        HeroIndex = --HeroIndex < 0 ? 0: HeroIndex;
        
        SetCuurentHero();
    }
    public void GetNextHero()
    {
        HeroIndex++;
        HeroIndex = HeroIndex >= LCustomizedCharacterSamples.Length ? LCustomizedCharacterSamples.Length - 1 : HeroIndex;
        SetCuurentHero();
    }
    private void SetCuurentHero()
    {
        Dictionary<int,Hero> heroes = DataManager.GetInstance().GetGameData().Heroes;
        if (heroes.ContainsKey(HeroIndex))
        {
            if (CurrentHero != null)
            {
                CurrentHero.transform.SetParent(null, false);
                CurrentHero.gameObject.SetActive(false);
            }
            Hero hero = heroes[HeroIndex];
            if (HeroChair != null && hero != null)
            {
                HeroTableData data = DataManager.GetInstance().GetHeroTableDataByHeroId(hero.heroId);
                GameObject heroPrefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", data.heroPrefab);
                if (heroPrefab != null)
                {
                    CurrentHero = heroPrefab.GetComponent<Enity>();
                    CurrentHero.transform.SetParent(HeroChair.transform, false);
                    CurrentHero.animatorAction = currentAnimatorAction;
                    CurrentHero.InitEnityByHero(hero);
                    
                    UpdateEquipShow();

                }

            }
        }
    }
    //显示佣兵装备
    public void UpdateEquipShow()
    {
        for (int i = 0; i < Equips.Count; i++)
        {
            Destroy(Equips[i].gameObject);
        }
        Equips.Clear();
        foreach (KeyValuePair<DummyProp, int> dummyPropPair in CurrentHero.hero.dummyPropDic)
        {
            int equipid = dummyPropPair.Value;
            int equipindex = (int)dummyPropPair.Key;
            if (equipid > 0 && EquipsSlot.Length > equipindex)
            {
                Item item = DataManager.GetInstance().GetGameData().GetItemById(equipid);
                Button equipKuang = EquipsSlot[equipindex];
                if (item != null && equipKuang != null)
                {
                    GameObject gameObject = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", "ItemIcon");
                    if (gameObject != null)
                    {
                        gameObject.transform.SetParent(equipKuang.transform,false);
                        gameObject.transform.localScale = gameObject.transform.localScale * 0.7f;
                        ItemIcon equip = gameObject.GetComponent<ItemIcon>();
                        if (equip != null)
                        {
                            equip.InitData(item,ItemIconType.HeroPanel_EquipsSlot);
                        }
                        Equips.Add(equip);
                    }
                }
            }
            else
            {

            }
        }
        
    }
    public void ChangeCurrentHero(int heroindex)
    {
        HeroIndex = heroindex;
        SetCuurentHero();
        
    }
    public void OnClickEquip(int dummyProp)
    {
        ShowPanel(PanelType.ChangeEquipPanel);
        ChangeEquipPanel.InitData((DummyProp)dummyProp, CurrentHero);
    }
    public void ShowPanel(PanelType panelType)
    {
        HeroPanel.SetActive(HeroPanel.name == panelType.ToString());
        ChangeEquipPanel.gameObject.SetActive(ChangeEquipPanel.name == panelType.ToString());
        HeroChair.SetActive(PanelType.HeroPanel == panelType);
    }
    public void ChangeEquip(Item equipdata)
    {
        CurrentHero.ChangeEquip(equipdata.id);
        UpdateEquipShow();
    }
    public void RemoveEquip(Item equipdata)
    {
        CurrentHero.RemoveEquip(equipdata.id);
        UpdateEquipShow();
    }
    public void OnPlayAnimation(int animatiotype)
    {
        if (CurrentHero == null)
            return;
        CurrentHero.OnPlayAnimation((AnimatorAction)animatiotype);

    }
    public void InitDropDown()
    {
        //是否可以点击
        dropDown.interactable = true;
        dropDown.ClearOptions();
        #region 添加下拉选项，，，设置文字，底图
        //添加一个下拉选项
        //Dropdown.OptionData data = new Dropdown.OptionData();
        //data.text = "动作";
        //data.image = "指定一个图片做背景不指定则使用默认"；
        //dropDown.options.Add(data);

        //另一种添加方式 , 不过用起来并不比第一个方便，
        List<Dropdown.OptionData> listOptions = new List<Dropdown.OptionData>();
        string[] options = Enum.GetNames(typeof(AnimatorAction));
        for (int i = 0; i < options.Length; i++)
        {
            listOptions.Add(new Dropdown.OptionData(options[i]));
        }
        
        //listOptions.Add(new Dropdown.OptionData("方案三"));
        dropDown.AddOptions(listOptions);

        //设置显示字体大小
        dropDown.captionText.fontSize = 20;
        //dropDown.captionImage = "底图";
        //设置要复制字体大小
        dropDown.itemText.fontSize = 22;
        //dropDown.itemImage = "底图";

        //PS：我一般是使用循环 使用第一种形式添加
        #endregion

        #region 添加完成就可以使用了，那么当我们想要复用怎么办呢？，这时就用到了移除OptionData，下面的每个注释打开都是一个功能
        //直接清理掉所有的下拉选项，
        //dropDown.ClearOptions();
        //亲测不是很好用
        //dropDown.options.Clear(); 

        //对象池回收时，有下拉状态的，直接干掉... (在极限点击测试的情况下会出现)
        if (dropDown.transform.childCount == 3)
        {
            //Destroy(dropDown.transform.GetChild(2).gameObject);
        }

        //移除指定数据   参数：OptionData
        //dropDown.options.Remove(data);
        //移除指定位置   参数:索引
        //dropDown.options.RemoveAt(0);
        #endregion

        #region 添加监听函数
        //当点击后值改变是触发 (切换下拉选项)
        dropDown.onValueChanged.AddListener((int v) => OnValueChange(v));
        //若有多个，可以将自己当做参数传递进去，已做区分。
        //dropDown.onValueChanged_1.AddListener((int v) => OnValueChange(dropDown.gameobject,v));
        #endregion
    }
    /// <summary>
    /// 当点击后值改变是触发 (切换下拉选项)
    /// </summary>
    /// <param name="v">是点击的选项在OptionData下的索引值</param>
    void OnValueChange(int v)
    {
        //切换选项 时处理其他的逻辑...
        Debug.Log("点击下拉控件的索引是..." + v);
        if (CurrentHero == null)
            return;
        CurrentHero.OnPlayAnimation((AnimatorAction)v);
        currentAnimatorAction = (AnimatorAction)v;
    }	
}
