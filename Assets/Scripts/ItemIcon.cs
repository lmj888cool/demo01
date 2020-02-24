using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemIconType
{
    ChangeEquipPanel,//更换装备的列表
    HeroPanel_EquipsSlot,//佣兵界面的装备槽
    NULL
}
public class ItemIcon : MonoBehaviour
{
    public Image Icon;
    public Image bg;
    private Item Data;
    private ItemIconType itemIconType;
    // Start is called before the first frame update
    void Start()
    {
        //InitData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitData(Item item, ItemIconType itemIconType)
    {
        Data = item;
        this.itemIconType = itemIconType;
        //ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItem(item);
        //ItemQuality itemQuality = (ItemQuality)itemTableData.quality;
        //switch (itemQuality)
        //{
        //    case ItemQuality.Black:
        //        bg.color = new Color(0,0,0,1);
        //        break;
        //    case ItemQuality.Green:
        //        bg.color = new Color(0, 1, 0, 1);
        //        break;
        //    case ItemQuality.Blue:
        //        bg.color = new Color(0, 0, 1, 1);
        //        break;
        //    case ItemQuality.Purple:
        //        bg.color = new Color(1, 0, 200.0f / 255.0f, 1);
        //        break;
        //    case ItemQuality.Yellow:
        //        bg.color = new Color(1, 150.0f / 255.0f, 0, 1);
        //        break;
        //    case ItemQuality.Red:
        //        bg.color = new Color(1, 0, 0, 1);
        //        break;
        //    default:
        //        break;
        //}
        AddIcon(DataManager.GetInstance().GetItemTableDataByItem(item).icon);

    }
    public void AddIcon(string iconname)
    {
        Sprite sprite = DataManager.GetInstance().CreateSpriteFromAssetsBundle("", iconname);
        if (sprite != null)
        {
            Icon.sprite = sprite;
        }
    }
    public void OnClickBtn()
    {
        switch (itemIconType)
        {
            case ItemIconType.ChangeEquipPanel:
                GameObject.Find("ChangeEquipPanel").SendMessage("UpdateSelectItem", Data);
                break;
            case ItemIconType.HeroPanel_EquipsSlot:
                ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItemId(Data.itemId);
                GameObject.Find("HeroScene").SendMessage("OnClickEquip", itemTableData.group);
                break;
            case ItemIconType.NULL:
                break;
            default:
                break;
        }
        
    }
}
