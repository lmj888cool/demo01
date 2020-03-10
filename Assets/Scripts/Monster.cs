using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Monster
{
    public long id;
    public bool isBoss = false;//是不是boss
    public HeroQuality heroQuality;//品质
    public Dictionary<HeroPart, int> heroPartDic = new Dictionary<HeroPart, int>();
    public Dictionary<DummyProp, long> dummyPropDic = new Dictionary<DummyProp, long>();//道具
    public int teamPosition;
    public int monsterLevel;

}