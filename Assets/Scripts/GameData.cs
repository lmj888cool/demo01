using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public Dictionary<int,Hero> Heroes = new Dictionary<int, Hero>();
    public Dictionary<int,Item> Items = new Dictionary<int, Item>();
    public int Gold = 0;
    public int Silver = 0;
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
