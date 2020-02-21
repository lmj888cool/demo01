using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;//唯一性ID
    public int itemId;//ItemTableData里面的ID
    public int masterId;//拥有者
    public int itemLevel;
}
