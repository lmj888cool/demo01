using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeroManager
{
    public static readonly RandomHeroManager instance = new RandomHeroManager();
    public RandomHeroManager()
    {

    }
    public GameObject OnRandomOneHero()
    {
        Enity enity = null;
        HeroJob heroJob = (HeroJob)Random.Range((int)HeroJob.Archer, (int)HeroJob.NULL);
        HeroSex sex = Random.Range(0, 100) > 50 ? HeroSex.Female : HeroSex.Male;
        List<DIYTableData> dIYTableDatas = DataManager.GetInstance().GetDIYTableDatasByHeroJobAndSex(heroJob, sex);

        ///////获取身体/////////////////////////////////////////////
        int bodyIndex = Random.Range(0, dIYTableDatas.Count);
        DIYTableData body = dIYTableDatas[bodyIndex];
        string body_prefab_name = DataManager.instance.GetConfigValueToString(heroJob.ToString() + "_body" + "_" + sex.ToString());
        GameObject bodyPrefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", body_prefab_name);
        Material material = DataManager.GetInstance().CreateMaterialFromAssetsBundle("enemy", body.prefab);
        if (bodyPrefab != null)
        {
            enity = bodyPrefab.GetComponent<Enity>();
            SkinnedMeshRenderer skinnedMeshRenderer = bodyPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null && material != null)
            {
                skinnedMeshRenderer.material = material;
            }
            if (enity != null)
            {

                #region 添加随机发型
                List<DIYTableData> dIYTableDataHairs = DataManager.GetInstance().GetDIYTableDatasByHeroPartAndSex(HeroPart.Hair, sex);
                if (dIYTableDataHairs.Count > 0)
                {
                    int hairIndex = Random.Range(0, dIYTableDataHairs.Count);
                    DIYTableData hairdata = dIYTableDataHairs[hairIndex];
                    GameObject hairPrefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", hairdata.prefab);
                    if (hairPrefab != null)
                    {
                        hairPrefab.transform.SetParent(enity.dummyProp_Parent[(int)DummyProp.Head], false);
                    }
                }
                #endregion

                #region 男性根据年龄添加随机胡子
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
                        GameObject beardPrefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", bearddata.prefab);
                        if (beardPrefab != null)
                        {
                            beardPrefab.transform.SetParent(enity.dummyProp_Parent[(int)DummyProp.Head], false);
                        }
                    }
                }
                #endregion
                #region 根据职业添加初始武器
                //是否需要配置初始武器呢？
                //待定
                #endregion



            }
            return bodyPrefab;
        }
        return null;
    }
}
