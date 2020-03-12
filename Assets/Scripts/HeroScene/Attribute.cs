using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attribute : MonoBehaviour
{
    public Text attributeName;
    public Text attributeValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitAttribute(string attributename,string attributevalue)
    {
        attributeName.text = attributename;
        attributeValue.text = attributevalue;
    }
}
