using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ChangeHeroInfo
{
    public int teamPosition;//上阵的位置
    public long battleHeroId;//上阵佣兵的ID
}
public class ChangeHeroPanel : MonoBehaviour
{
    public ScrollRect heroesView;
    public RawImage heroShow;
    public Text heroName;
    public Text heroInfo;
    private GameObject heroShow3D;
    private Hero currentHero;
    private List<GameObject> heroes = new List<GameObject>();
    private Enity enity;
    public Button changeButton;
    public Button removeButton;
    private ItemIcon currentSelectItemIcon;
    private long openInputHeroId;
    private int mTeamPosition = -1;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //if (ItemShow != null)
        //{
        //    ItemShow3D.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(ItemShow.transform.position.x, ItemShow.transform.position.y, 10.0f));
        //}
    }
    public void UpdateSelectHero(Hero hero)
    {
        Clear();
        currentHero = hero;        
        if (Camera.main != null)
        {
            string body_prefab_name = DataManager.instance.GetConfigValueToString(hero.heroJob.ToString() + "_body" + "_" + hero.heroSex.ToString());
            heroShow3D = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", body_prefab_name);
            if (heroShow3D != null)
            {
                enity = heroShow3D.GetComponent<Enity>();
                if (enity != null)
                {
                    enity.InitEnityByHero(hero);
                }
                Quaternion quaternion = new Quaternion(0, 180, 0, 1);
                heroShow3D.transform.localRotation = quaternion;
                heroShow3D.transform.SetParent(Camera.main.transform.parent, false);

                heroShow3D.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                heroShow3D.transform.localPosition = new Vector3(0, 0, 0);
                if (heroShow != null)
                {
                    heroShow3D.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(heroShow.transform.position.x, heroShow.transform.position.y, 10.0f));
                }
            }

        }
        //heroName.text = itemTableData.name;
        //heroInfo.text = itemTableData.des;
        bool isinteam = hero.teamPosition > -1;
        removeButton.gameObject.SetActive(isinteam);
        changeButton.gameObject.SetActive(!isinteam);
    }
    public void Clear()
    {
        if (heroShow3D != null)
        {
            Destroy(heroShow3D);
            heroShow3D = null;
        }
    }
    public void InitData(int teamPosition)
    {
        this.mTeamPosition = teamPosition;
        Hero hero = DataManager.instance.GetHeroByTeamPosition(teamPosition);
        if (hero != null)
        {
            openInputHeroId = hero.id;
        }
        Dictionary<long, Hero> heroes = DataManager.instance.GetHeroes();
        long defaultid = -1;
        foreach (KeyValuePair<long, Hero> heropair in heroes)
        {
            if(defaultid == -1)
            {
                defaultid = heropair.Key;
            }
            //int teampostion = heropair.Value.teamPosition;
            //if (teampostion > -1)
            {
                AddHeroToListView(heropair.Value);
            }
            if(openInputHeroId == heropair.Value.id)
            {
                defaultid = openInputHeroId;
            }
        }
        UpdateSelectHero(heroes[defaultid]);


    }
    public void AddHeroToListView(Hero hero)
    {
        GameObject gameObject = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", "HeroIcon");
        if (gameObject != null)
        {
            gameObject.transform.SetParent(heroesView.content, false);
            HeroIcon heroicon = gameObject.GetComponent<HeroIcon>();
            if (heroicon != null)
            {
                heroicon.InitData(hero, HeroIconType.ChangeHeroPanel);
            }
            heroes.Add(gameObject);
        }
    }
    public void OnReturn()
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            Destroy(heroes[i]);
        }
        heroes.Clear();
        Clear();
        GameObject.Find("FightScene").SendMessage("CloseChangeHeroPanel");
        //GameObject.Find("HeroScene").SendMessage("ShowPanel", PanelType.HeroPanel);
    }
    public void ChangeHero()
    {
        OnReturn();
        ChangeHeroInfo changeHeroInfo = new ChangeHeroInfo
        {
            battleHeroId = currentHero.id,
            teamPosition = mTeamPosition
        };
        GameObject.Find("FightScene").SendMessage("ChangeHero", changeHeroInfo);
    }
    public void RemoveHero()
    {
        OnReturn();
        ChangeHeroInfo changeHeroInfo = new ChangeHeroInfo
        {
            battleHeroId = -1,
            teamPosition = mTeamPosition
        };
        GameObject.Find("FightScene").SendMessage("ChangeHero", changeHeroInfo);
    }
}
