using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexBtn : MonoBehaviour
{
    public Image selectImg;
    public Image bossImg;
    public Text indexTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitByChapterId(int chapterid)
    {
        ChapterTableData chapterTableData = DataManager.GetInstance().GetChapterTableDataById(chapterid);
        if (chapterTableData != null)
        {
            int currentchapterId = DataManager.GetInstance().GetGameData().ChapterId;
            selectImg.gameObject.SetActive(currentchapterId == chapterTableData.id);
            bossImg.gameObject.SetActive(chapterTableData.isBoss == 1);
            indexTxt.text = chapterTableData.id.ToString();
        }

    }
    public void OnClickBtn()
    {

    }
}
