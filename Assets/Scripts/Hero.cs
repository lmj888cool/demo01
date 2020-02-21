using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Hero
{
    public int id;
    public int heroId;
    public Dictionary<DummyProp, int> dummyPropDic = new Dictionary<DummyProp, int>();//道具
    public int teamPosition;//在阵容那个位置，-1表示没有上阵\
    public int heroLevel;
}
