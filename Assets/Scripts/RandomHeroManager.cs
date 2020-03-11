using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeroManager
{
    public static readonly RandomHeroManager instance = new RandomHeroManager();
    public RandomHeroManager()
    {

    }
    public Hero OnRandomOneHero()
    {

        HeroJob heroJob = (HeroJob)Random.Range((int)HeroJob.Archer, (int)HeroJob.NULL);
        HeroSex sex = Random.Range(0, 100) > 50 ? HeroSex.Female : HeroSex.Male;
        HeroQuality heroQuality = (HeroQuality)Random.Range((int)HeroQuality.B, (int)HeroQuality.NULL);
        Hero hero = new Hero
        {
            id = GetRandomId(),
            heroLevel = 1,
            heroJob = heroJob,
            heroQuality = heroQuality,
            heroSex = sex
        };
        ///////获取身体/////////////////////////////////////////////
        List<DIYTableData> dIYTableDatas = DataManager.GetInstance().GetDIYTableDatasByHeroJobAndSex(heroJob, sex);
        if (dIYTableDatas.Count > 0)
        {
            int bodyIndex = Random.Range(0, dIYTableDatas.Count);
            DIYTableData body = dIYTableDatas[bodyIndex];
            hero.heroPartDic[HeroPart.Body] = body.id;
        }
        List<DIYTableData> dIYTableDataHairs = DataManager.GetInstance().GetDIYTableDatasByHeroPartAndSex(HeroPart.Hair, sex);
        if (dIYTableDataHairs.Count > 0)
        {
            int hairIndex = Random.Range(0, dIYTableDataHairs.Count);
            DIYTableData hairdata = dIYTableDataHairs[hairIndex];
            hero.heroPartDic[HeroPart.Hair] = hairdata.id;
        }
        int min_age = DataManager.instance.GetConfigValueToInt("min_age");
        int max_age = DataManager.instance.GetConfigValueToInt("max_age");
        int have_beard_age = DataManager.instance.GetConfigValueToInt("have_beard_age");
        int age = Random.Range(min_age, max_age);//随机年龄
        if (age < have_beard_age && sex == HeroSex.Male)//xx岁以下不添加胡子
        {
            List<DIYTableData> dIYTableDataBeards = DataManager.GetInstance().GetDIYTableDatasByHeroPartAndSex(HeroPart.Beard, sex);
            if (dIYTableDataBeards.Count > 0)
            {
                int beardIndex = Random.Range(0, dIYTableDataBeards.Count);
                DIYTableData bearddata = dIYTableDataBeards[beardIndex];
                hero.heroPartDic[HeroPart.Beard] = bearddata.id;
            }
        }
        return hero;
    }
    public long GetRandomId()
    {
        Random rnd = new Random();
        string rndid = Random.Range(100, 999).ToString();
        string strdatetime = System.DateTime.Now.ToString("yyyyMMddhhmmss");
        string rndstr = strdatetime + rndid;
        return long.Parse(rndstr);
    }
}
