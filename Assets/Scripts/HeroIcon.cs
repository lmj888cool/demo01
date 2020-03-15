using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum HeroIconType
{
    HeroScene,
    FgihtScene,
    ChangeHeroPanel,
    NULL
}

public class HeroIcon : MonoBehaviour
{
    public Image icon;
    public Image noteam;
    private Hero mHero;
    private int mTeamPostion = -1;
    public Text[] qualityTxtArr;
    private HeroIconType heroIconType = HeroIconType.NULL;
    // Start is called before the first frame update
    void Start()
    {
        //InitData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitData(Hero hero,HeroIconType heroicontype = HeroIconType.HeroScene)
    {
        //Reset();
        heroIconType = heroicontype;
        mHero = hero;
        HeroTableData data = DataManager.GetInstance().GetHeroTableDataByHeroId(0);
        if(data != null)
        {
            AddIcon(data.icon);
        }
        icon.gameObject.SetActive(true);
        noteam.gameObject.SetActive(false);
        Text text = qualityTxtArr[(int)hero.heroQuality];
        text.gameObject.SetActive(true);
        GetComponent<Image>().color = text.color;
        //GameObject obj = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", "Sword 01 Black");
        //if (obj != null && Camera.main != null)
        //{
        //    obj.transform.SetParent(Camera.main.transform.parent,false);
        //    //if (ItemShow != null && heroSceneCamera != null)
        //    {
        //        Vector3 pos = transform.position;
        //        obj.transform.position = Camera.main.ScreenToWorldPoint(pos);
        //    }

        //}

    }
    public void AddIcon(string iconname)
    {
        Sprite sprite = DataManager.GetInstance().CreateSpriteFromAssetsBundle("", iconname);
        if (sprite != null)
        {
            icon.sprite = sprite;
        }
    }
    public void OnClickBtn()
    {
        //判断是否装备
        if (heroIconType == HeroIconType.NULL)
        {
            GameObject.Find("FightScene").SendMessage("OpenChangeHeroPanel", mTeamPostion);
        }
        else
        {
            switch (heroIconType)
            {
                case HeroIconType.HeroScene:
                    GameObject.Find("HeroScene").SendMessage("ChangeCurrentHero", mHero.id);
                    break;
                case HeroIconType.FgihtScene:
                    GameObject.Find("FightScene").SendMessage("OpenChangeHeroPanel", mTeamPostion);
                    break;
                case HeroIconType.ChangeHeroPanel:
                    GameObject.Find("changeHeroPanel").SendMessage("UpdateSelectHero", mHero);
                    break;
                default:
                    break;
            }
        }
        
    }
    public void SetTeamPosition(int teamPostion)
    {
        mTeamPostion = teamPostion;
    }
    public void Reset()
    {
        icon.gameObject.SetActive(false);
        noteam.gameObject.SetActive(true);
        for (int i = 0; i < qualityTxtArr.Length; i++)
        {
            qualityTxtArr[i].gameObject.SetActive(false);
        }
        GetComponent<Image>().color = Color.white;
        heroIconType = HeroIconType.NULL;
        mTeamPostion = -1;
    }
}
