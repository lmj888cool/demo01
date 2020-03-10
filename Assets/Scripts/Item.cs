using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public long id;//唯一性ID
    public int itemId;//ItemTableData里面的ID
    public long masterId;//拥有者
    public int itemLevel;
}
