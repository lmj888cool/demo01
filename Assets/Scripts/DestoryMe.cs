using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryMe : MonoBehaviour
{
    public GameObject me;
    private bool HaveTriggerEnter = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (HaveTriggerEnter)
            return;
        if (other.gameObject.tag == EnityType.Hero.ToString())
        {
            HaveTriggerEnter = true;
            if (me != null)
            {
                Destroy(me, 5);
            }

        }

    }
}
