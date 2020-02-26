using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEquipPanel : MonoBehaviour
{
    public ScrollRect ItemsView;
    public RawImage ItemShow;
    public Text ItemName;
    public Text ItemInfo;
    public Camera heroSceneCamera;
    private GameObject ItemShow3D;
    private Item CurrentItem;
    private List<GameObject> Equips = new List<GameObject>();
    private Enity enity;
    private DummyProp dummyProp;
    public Button changeButton;
    public Button removeButton;
    private ItemIcon currentSelectItemIcon;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //if (ItemShow != null)
        //{
        //    ItemShow3D.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(ItemShow.transform.position.x, ItemShow.transform.position.y, 10.0f));
        //}
    }
    public void UpdateSelectItem(Item item)
    {
        Clear();
        CurrentItem = item;
        ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItem(item);
        ItemShow3D = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", itemTableData.itemPrefab);
        if(ItemShow3D != null && Camera.main != null)
        {
            ItemShow3D.AddComponent<RolateObject>();
            
            ItemShow3D.transform.SetParent(Camera.main.transform.parent,false);
            ItemShow3D.transform.localRotation = Quaternion.identity;
            ItemShow3D.transform.localScale = new Vector3(1, 1, 1);
            ItemShow3D.transform.localPosition = new Vector3(0, 0, 0);
            if (ItemShow != null)
            {
                ItemShow3D.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(ItemShow.transform.position.x,ItemShow.transform.position.y,10.0f));
            }
            
        }
        ItemName.text = itemTableData.name;
        ItemInfo.text = itemTableData.des;
        bool isSelfEquip = false;
        if(this.enity != null && this.enity.hero.dummyPropDic.ContainsKey(this.dummyProp) && this.enity.hero.dummyPropDic[this.dummyProp] == item.id)
        {
            isSelfEquip = true;
        }

        removeButton.gameObject.SetActive(isSelfEquip);
        changeButton.gameObject.SetActive(!isSelfEquip);
    }
    public void Clear()
    {
        if (ItemShow3D != null)
        {
            Destroy(ItemShow3D);
            ItemShow3D = null;
        }
    }
    public void InitData(DummyProp dummyProp,Enity enity)
    {
        this.enity = enity;
        this.dummyProp = dummyProp;
        bool isSetDefault = false;
        int enityitemid = 0;
        if (this.enity != null)
        {
            if (this.enity.hero.dummyPropDic.ContainsKey(dummyProp))
            {
                enityitemid = this.enity.hero.dummyPropDic[dummyProp];
                if (DataManager.GetInstance().GetGameData().Items.ContainsKey(enityitemid))
                {
                    Item item = DataManager.GetInstance().GetGameData().Items[enityitemid];
                    AddEquipToItemsView(item);
                    UpdateSelectItem(item);
                    isSetDefault = true;
                }
            }
        }
        foreach (KeyValuePair<int,Item> item in DataManager.GetInstance().GetGameData().Items)
        {
            ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItemId(item.Value.itemId);
            if(itemTableData.group == (int)dummyProp && enityitemid != item.Key)
            {
                AddEquipToItemsView(item.Value);
                if (!isSetDefault)
                {
                    UpdateSelectItem(item.Value);
                    isSetDefault = true;
                }
                
            }
        }
    }
    public void AddEquipToItemsView(Item item)
    {
        GameObject gameObject = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", "ItemIcon");
        if (gameObject != null)
        {
            gameObject.transform.SetParent(ItemsView.content, false);
            ItemIcon equip = gameObject.GetComponent<ItemIcon>();
            if (equip != null)
            {
                equip.InitData(item, ItemIconType.ChangeEquipPanel);
            }
            Equips.Add(gameObject);
        }
    }
    public void OnReturn()
    {
        for (int i = 0; i < Equips.Count; i++)
        {
            Destroy(Equips[i]);
        }
        Equips.Clear();
        Clear();
        GameObject.Find("HeroScene").SendMessage("ShowPanel",PanelType.HeroPanel);
    }
    public void ChangeEquip()
    {
        OnReturn();
        GameObject.Find("HeroScene").SendMessage("ChangeEquip", CurrentItem);
    }
    public void RemoveEquip()
    {
        OnReturn();
        GameObject.Find("HeroScene").SendMessage("RemoveEquip", CurrentItem);
    }
}
