using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMap : MonoBehaviour
{
    public Image changeMapBG;
    public Text mapNameTxt;
    public Text mapIdTxt;
    public float speed = 0.005f;
    public float alpha = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        alpha -= 0.01f;
        if (alpha < 0.0f)
        {
            alpha = 1.0f;
            gameObject.SetActive(false);
        }
        else
        {
            ChangeAlpha();
        }
        

    }
    public void ChangeAlpha()
    {
        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            Color color = texts[i].color;
            color.a = alpha;
            texts[i].color = color;
        }

        Image[] images = gameObject.GetComponentsInChildren<Image>();
        for (int j = 0; j < images.Length; j++)
        {
            Color color = images[j].color;
            color.a = alpha;
            images[j].color = color;
        }

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
