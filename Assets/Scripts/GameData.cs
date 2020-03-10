using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public Dictionary<long,Hero> Heroes = new Dictionary<long, Hero>();
    public Dictionary<long, Item> Items = new Dictionary<long, Item>();
    public int Gold = 0;//金币数量
    public int Silver = 0;//银币数量
    public int ChapterId = 1;//当前挂机的关卡ID
    public int MapId = 1;//当前地图ID
    public int SubId=1;//当前挂机地图的小关
    public bool IsPveLose = false;//挑战Boss是否失败
    public Hero GetHeroById(long heroid)
    {
        if (Heroes.ContainsKey(heroid))
        {
            return Heroes[heroid];
        }
        return null;
    }
    public Item GetItemById(long itemid)
    {
        if (Items.ContainsKey(itemid))
        {
            return Items[itemid];
        }
        return null;
    }
}
