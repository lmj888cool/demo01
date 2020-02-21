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
    void Start()
    {
        //LCustomizedCharacterSamples = DataManager.GetInstance().GetGameObjectsByPath("Prefabs/Customized Character Samples", ".prefab");
        if(Prefab != null)
        {
            LCustomizedCharacterSamples = GameObject.FindGameObjectsWithTag("Prefab_Hero");
        }
        // SetCuurentHero();
        ShowPanel(PanelType.HeroPanel);
        InitHeroHead();
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
            if (HeroChair != null)
            {
                HeroTableData data = DataManager.GetInstance().GetHeroTableDataByHeroId(heroes[HeroIndex].heroId);
                GameObject hero = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", data.heroPrefab);
                if (hero != null)
                {
                    CurrentHero = hero.GetComponent<Enity>();
                    CurrentHero.transform.SetParent(HeroChair.transform, false);
                    CurrentHero.InitEnityByHero(heroes[HeroIndex]);
                }

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
        ChangeEquipPanel.InitData((DummyProp)dummyProp);
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
    }
    public void OnPlayAnimation(string animationname)
    {
        if (CurrentHero == null)
            return;
        CurrentHero.OnPlayAnimation(animationname);

    }
}
