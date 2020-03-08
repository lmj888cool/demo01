using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TavernScene : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    GameObject obj;
    private Vector3 CameraDefaultPos;//摄像机初始的位置
    public GameObject[] RandomHeroes;

    public GameObject heroInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        CameraDefaultPos = Camera.main.transform.position;
        OnClickBack();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("点击鼠标左键");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                obj = hit.collider.gameObject;
                //通过名字
                if (obj.name.Equals("BeiJiuChuan"))
                {
                    Debug.Log("点中" + obj.name);
                }
                //通过标签
                if (obj.tag == "move")
                {
                    Debug.Log("点中" + obj.name);
                }
                for (int i = 0; i < RandomHeroes.Length; i++)
                {
                    if(RandomHeroes[i].gameObject.name == obj.name)
                    {
                        if (Camera.main != null)
                        {
                            iTween.MoveTo(Camera.main.gameObject, new Vector3(obj.transform.position.x, obj.transform.position.y + 0.12f, 7f), 0.5f);
                            heroInfoPanel.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        RandomHeroes[i].gameObject.SetActive(false);
                    }
                }
                
            }
        }

    }
    public void OnClickBack()
    {
        if (Camera.main != null)
        {
            iTween.MoveTo(Camera.main.gameObject, CameraDefaultPos, 0.5f);
            heroInfoPanel.gameObject.SetActive(false);
            for (int i = 0; i < RandomHeroes.Length; i++)
            {
                {
                    RandomHeroes[i].gameObject.SetActive(true);
                }
            }
            
        }
    }
    public void OnRandomHeroes()
    {

    }
    public Enity OnRandomOneHero()
    {
        Enity enity = null;
        HeroJob heroJob = (HeroJob)Random.Range((int)HeroJob.Archer, (int)HeroJob.NULL);
        int sex = Random.Range(0, 100) > 50 ? 1 : 0;
        List<DIYTableData> dIYTableDatas = DataManager.GetInstance().GetDIYTableDatasByHeroJobAndSex(heroJob,sex);

        ///////获取身体/////////////////////////////////////////////
        int bodyIndex = Random.Range(0,dIYTableDatas.Count);
        DIYTableData body = dIYTableDatas[bodyIndex];
        GameObject prefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", body.prefab);
        if(prefab != null)
        {
            enity = prefab.AddComponent<Enity>();
            if (enity != null)
            {

            }
        }

        ////////////////////////////////////////////////////////////
        int part = Random.Range(0, 100) > 50 ? 1 : 0;
        return enity;
    }

}
