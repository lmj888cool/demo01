using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class HeroManager
{
    private static HeroManager sHeroManager;
    public static HeroManager GetInstance()
    {
        if (sHeroManager == null)
        {
            sHeroManager = new HeroManager();
        }
        return sHeroManager;
    }
    public HeroManager()
    {

    }

}
