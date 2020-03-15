using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamListPanel : MonoBehaviour
{
    public HeroIcon[] teamListArr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitTeamList()
    {
        for (int i = 0; i < teamListArr.Length; i++)
        {
            teamListArr[i].Reset();
            teamListArr[i].SetTeamPosition(i);
        }
        Dictionary<long, Hero> heroes = DataManager.instance.GetGameData().Heroes;
        foreach (KeyValuePair<long,Hero> heropair in heroes)
        {
            int teampostion = heropair.Value.teamPosition;
            if (teampostion > -1 && teamListArr.Length > teampostion)
            {
                teamListArr[teampostion].InitData(heropair.Value,HeroIconType.FgihtScene);
            }
        }
    }
}
