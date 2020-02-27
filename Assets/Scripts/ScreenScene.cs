using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject item;
    private GameObject ItemShow3D;
    public int ItemId = 16;
    private float dalayTime = 1f;
    private float leijiTime = 0.0f;
    private ItemTableData itemTableData;
    void Start()
    {
        //StartScreen();
    }

    // Update is called once per frame
    void Update()
    {
        leijiTime += Time.deltaTime;
        
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            leijiTime = 0.0f;
            if (ItemShow3D != null)
            {
                ScreenTool.Instance.CameraCapture(Camera.main, new Rect(0, 0, 1024, 1024), itemTableData.name + ".png");
                ItemShow3D.transform.parent = null;
                Destroy(ItemShow3D);
                ItemShow3D = null;

            }
            else
            {
                while (DataManager.GetInstance().ItemTableDataDic.ContainsKey(ItemId))
                {
                    itemTableData = DataManager.GetInstance().ItemTableDataDic[ItemId];
                    if(DataManager.GetInstance().GetItemTableDataByItemName(itemTableData.name) ==null)
                    {
                        ItemShow3D = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", itemTableData.itemPrefab);
                        if (ItemShow3D != null && Camera.main != null)
                        {
                            ItemShow3D.transform.SetParent(item.transform, false);
                            ParticleSystem[] particleSystems = ItemShow3D.GetComponentsInChildren<ParticleSystem>();
                            for (int i = 0; i < particleSystems.Length; i++)
                            {
                                particleSystems[i].gameObject.SetActive(false);
                            }
                            Quaternion quaternion = new Quaternion(ItemShow3D.transform.localRotation.x, ItemShow3D.transform.localRotation.y, ItemShow3D.transform.localRotation.z, ItemShow3D.transform.localRotation.w);
                            quaternion.x = 0;
                            quaternion.y = 145;
                            ItemShow3D.transform.localRotation = quaternion;
                            ItemShow3D.transform.localScale = new Vector3(1, 1, 1);
                            ItemShow3D.transform.localPosition = new Vector3(0, 0, 0);


                        }
                        break;
                    }
                    else
                    {
                        ItemId++;
                    }
                   
                }
                ItemId++;
            }
            
        }
        
    }
    public void StartScreen()
    {
        
        foreach (KeyValuePair<int, ItemTableData> pair in DataManager.GetInstance().ItemTableDataDic)
        {
            ItemTableData itemTableData = pair.Value;
            GameObject ItemShow3D = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("", itemTableData.itemPrefab);
            if (ItemShow3D != null && Camera.main != null)
            {
                ItemShow3D.transform.SetParent(item.transform, false);
                ItemShow3D.transform.localRotation = Quaternion.identity;
                ItemShow3D.transform.localScale = new Vector3(1, 1, 1);
                ItemShow3D.transform.localPosition = new Vector3(0, 0, 0);
                ScreenTool.Instance.CameraCapture(Camera.main, new Rect(0, 0, 1024, 1024), itemTableData.name + ".png");
                ItemShow3D.transform.SetParent(null);
                Destroy(ItemShow3D);
            }
        }
    }
}
