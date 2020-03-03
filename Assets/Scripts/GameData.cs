using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public Dictionary<int,Hero> Heroes = new Dictionary<int, Hero>();
    public Dictionary<int,Item> Items = new Dictionary<int, Item>();
    public int Gold = 0;//金币数量
    public int Silver = 0;//银币数量
    public int ChapterId = 1;//当前挂机的大关
    public int SubId=1;//当前挂机的小关
    public bool IsPveLose = false;//挑战Boss是否失败
    public Hero GetHeroById(int heroid)
    {
        if (Heroes.ContainsKey(heroid))
        {
            return Heroes[heroid];
        }
        return null;
    }
    public Item GetItemById(int itemid)
    {
        if (Items.ContainsKey(itemid))
        {
            return Items[itemid];
        }
        return null;
    }
}
