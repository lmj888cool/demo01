using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroIcon : MonoBehaviour
{
    public Image Icon;
    private Hero mHero;
    private int mTeamPostion = 0;
    // Start is called before the first frame update
    void Start()
    {
        //InitData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitData(Hero hero)
    {
        mHero = hero;
        HeroTableData data = DataManager.GetInstance().GetHeroTableDataByHeroId(hero.heroId);
        if(data != null)
        {
            AddIcon(data.icon);
        }
       
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
            Icon.sprite = sprite;
        }
    }
    public void OnClickBtn()
    {
        GameObject.Find("HeroScene").SendMessage("ChangeCurrentHero", mHero.id);
    }
}
