using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    // Start is called before the first frame update
    //public RawImage Bg;
    public RawImage Hp;
    public Text txt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetProgress(float progress)
    {
        if(Hp != null)
        {
            Hp.transform.localScale = new Vector3(progress,1,1);
            Vector3 scale = new Vector3(3.0f,3.0f,3.0f);
            if(txt != null)
            {
                Text tempTxt = Instantiate(txt, txt.transform.parent);
                tempTxt.transform.localScale = scale;
                tempTxt.transform.position = new Vector3(tempTxt.transform.position.x + Random.Range(0.0f, 20.0f), tempTxt.transform.position.y + Random.Range(0.0f, 20.0f), 0);
                //iTween.MoveTo(tempTxt.gameObject, iTween.Hash("y", tempTxt.transform.position.y +2.0f, "time", 0.2f, "delay", 0.0f));
                iTween.ScaleTo(tempTxt.gameObject, iTween.Hash("scale", Vector3.one, "time", 0.3f, "delay", 0.0f));
                Destroy(tempTxt, 0.4f);
            }
            
        }
    }
}
