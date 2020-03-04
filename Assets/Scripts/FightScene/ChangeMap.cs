using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMap : MonoBehaviour
{
    public Image changeMapBG;
    public Text mapNameTxt;
    public Text mapIdTxt;
    public float speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeTextAlpha(mapNameTxt);
        ChangeTextAlpha(mapIdTxt);
        ChangeImageAlpha(changeMapBG);
    }
    public void ChangeTextAlpha(Text text)
    {
        Color color = text.color;
        color.a -= speed;
        if (color.a < 0.0f)
        {
            color.a = 0.0f;
        }
        text.color = color;
    }
    public void ChangeImageAlpha(Image image)
    {
        Color color = image.color;
        color.a -= speed;
        if (color.a < 0.0f)
        {
            color.a = 0.0f;
        }
        image.color = color;
    }
    public void Init(string name,string id, string bg="")
    {
        gameObject.SetActive(true);
        mapNameTxt.text = name;
        mapIdTxt.text = id;
        if(bg != null)
        {

        }
        //iTween.FadeTo(gameObject,1.0f, 1.0f);
        //iTween.FadeTo(mapNameTxt.gameObject, iTween.Hash("alpha", 0.0f, "namedcolorvalue", iTween.NamedValueColor._Color, "time", 1.0f, "delay", 1.0f));
    }
}
