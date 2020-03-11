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
    public Transform[] RandomHeroesPosArr;
    public List<GameObject> RandomHeroes;

    public GameObject heroInfoPanel;

    private int currentShowId = -1;

    // Start is called before the first frame update
    void Start()
    {
        CameraDefaultPos = Camera.main.transform.position;
        OnClickBack();

        OnFleshRandomHeroes();
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
                for (int i = 0; i < RandomHeroesPosArr.Length; i++)
                {
                    if(RandomHeroesPosArr[i].gameObject.name == obj.name)
                    {
                        if (Camera.main != null)
                        {
                            iTween.MoveTo(Camera.main.gameObject, new Vector3(obj.transform.position.x, obj.transform.position.y + 0.32f, 5.88f), 0.5f);
                            heroInfoPanel.gameObject.SetActive(true);
                        }
                        currentShowId = i;
                    }
                    else
                    {
                        RandomHeroesPosArr[i].gameObject.SetActive(false);
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
            for (int i = 0; i < RandomHeroesPosArr.Length; i++)
            {
                {
                    RandomHeroesPosArr[i].gameObject.SetActive(true);
                }
            }
            
        }
        currentShowId = -1;
    }
    
    public void ClearRandomHeroes()
    {
        for (int i = 0; i < RandomHeroes.Count; i++)
        {
            Destroy(RandomHeroes[i]);
        }
        RandomHeroes.Clear();
    }
    public void OnFleshRandomHeroes()
    {
        ClearRandomHeroes();
        for (int i = 0; i < RandomHeroesPosArr.Length; i++)
        {
            Hero hero = RandomHeroManager.instance.OnRandomOneHero();
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            string body_prefab_name = DataManager.instance.GetConfigValueToString(hero.heroJob.ToString() + "_body" + "_" + hero.heroSex.ToString());
            GameObject bodyPrefab = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", body_prefab_name);
            if (bodyPrefab != null)
            {
                Enity enity = bodyPrefab.GetComponent<Enity>();
                if (enity != null)
                {
                    /////初始化Enity////////////
                    enity.InitEnityByHero(hero);
                }
                RandomHeroes.Add(bodyPrefab);
                bodyPrefab.transform.SetParent(RandomHeroesPosArr[i].transform, false);
            }
            
        }
    }
    public void OnGetHero()
    {
        if(currentShowId >= 0 && RandomHeroes.Count > currentShowId)
        {
            GameObject bodyPrefab = RandomHeroes[currentShowId];
            if (bodyPrefab != null)
            {
                Enity enity = bodyPrefab.GetComponent<Enity>();
                if (enity != null)
                {
                    Vector3 worldPos = enity.dummyProp_Parent[(int)DummyProp.Head].TransformPoint(Vector3.zero);
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                    ScreenTool.Instance.CameraCapture(Camera.main, new Rect(0, 0, 120, 120), enity.hero.id.ToString() + ".png", (int)screenPos.x, (int)screenPos.y);
                    DataManager.instance.GetGameData().Heroes.Add(enity.hero.id,enity.hero);
                    DataManager.instance.SaveByBin();
                    Destroy(bodyPrefab);
                    RandomHeroes[currentShowId] = null;
                    
                    OnClickBack();
                }
            }
        }
    }

}
