using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour
{
    // Start is called before the first frame update
    public int DetonateDamage = 100;//自爆造成的范围伤害
    public int DamageCount = 1;//对单个物体造成伤害次数
    private Dictionary<string,int> DamagedGameObjects;//伤害过的物体，不能重复伤害
    void Start()
    {
        DamagedGameObjects = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsCanDemage(string gameObejctName)
    {
        return !(DamagedGameObjects.ContainsKey(gameObejctName) && DamagedGameObjects[gameObejctName] >= DamageCount);
    }
    public void AddDamagedGameObject(string gameObejctName)
    {
        if (DamagedGameObjects.ContainsKey(gameObejctName))
        {
            DamagedGameObjects[gameObejctName]++;
        }
        else
        {
            DamagedGameObjects.Add(gameObejctName,1);
        }
    }
}
