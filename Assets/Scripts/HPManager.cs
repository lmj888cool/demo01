using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    // Start is called before the first frame update
    public HP HPPrefab;
    private Dictionary<string, HP> EnityHPMap = new Dictionary<string, HP>();//保存场景中所有物体的血量体
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<string> destroyList = new List<string>();
        foreach (KeyValuePair< string, HP > item in EnityHPMap)
        {
            if(item.Value.Hp.transform.localScale.x < 0.00001f)
            {
                Destroy(item.Value.gameObject);
                destroyList.Add(item.Key);
            }
        }
        if (destroyList.Count > 0)
        {
            for (int i = 0; i < destroyList.Count; i++)
            {
                EnityHPMap.Remove(destroyList[i]);
            }
        }
    }
    public bool IsCreateHP(string name)
    {
        return EnityHPMap.ContainsKey(name);
    }
    public void UpdateEnityHPByEnityName(string name,Vector3 pos,float progress,float scale)
    {
        if (!EnityHPMap.ContainsKey(name))
        {
            //HP gameObject = Instantiate(HPPrefab, transform);
            EnityHPMap.Add(name, Instantiate(HPPrefab, transform));        
        }
        EnityHPMap[name].SetProgress(progress);
        //float scale = (1136 - pos.y) / 1136;
        //gameObject.transform.localScale = new Vector3(scale, scale, scale);
        EnityHPMap[name].transform.position = pos;
    }
}
