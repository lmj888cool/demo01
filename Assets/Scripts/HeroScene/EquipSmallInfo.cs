using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSmallInfo : MonoBehaviour
{
    public Image Icon;
    public Image bg;
    private Item Data;
    private ItemIconType itemIconType;
    public Text[] equipTextsArr;
    public Text[] notEquipTextsArr;
    public Attribute[] attributesArr;
    // Start is called before the first frame update
    void Start()
    {
        //InitData();
        //SetState();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitData(Item item, ItemIconType itemIconType)
    {
        SetState(true);
        Data = item;
        ItemTableData itemTableData = DataManager.instance.GetItemTableDataByItemId(item.itemId);
        equipTextsArr[0].text = itemTableData.name;
        equipTextsArr[1].text = item.itemLevel.ToString();
        this.itemIconType = itemIconType;
        AddIcon(DataManager.GetInstance().GetItemTableDataByItem(item).icon);

    }
    public void SetState(bool isHaveEquip = false)
    {
        for (int i = 0; i < notEquipTextsArr.Length; i++)
        {
            notEquipTextsArr[i].gameObject.SetActive(!isHaveEquip);
        }
        for (int i = 0; i < equipTextsArr.Length; i++)
        {
            equipTextsArr[i].gameObject.SetActive(isHaveEquip);
        }
        for (int i = 0; i < attributesArr.Length; i++)
        {
            attributesArr[i].gameObject.SetActive(isHaveEquip);
        }
        Icon.sprite = null;
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
