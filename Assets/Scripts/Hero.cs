using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Hero
{
    public long id;
    public HeroJob heroJob;//佣兵的职业
    public HeroQuality heroQuality;//佣兵的品质
    public HeroSex heroSex;
    public Dictionary<HeroPart, int> heroPartDic = new Dictionary<HeroPart, int>();
    public Dictionary<DummyProp, long> dummyPropDic = new Dictionary<DummyProp, long>();//道具
    public int teamPosition = -1;//在阵容那个位置，-1表示没有上阵
    public int heroLevel = 1;

}
