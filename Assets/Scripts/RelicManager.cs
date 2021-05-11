using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    public List<RelicData> relicDataList=new List<RelicData>();//存储所有遗物数据的列表
    public List<Relic> inGameRelicList = new List<Relic>();//游戏中玩家拥有的遗物列表
    public List<Sprite> relicSpriteList = new List<Sprite>();//遗物图片列表
    public GameObject relicPrefab;//遗物的预制体
    public GameObject inGameRelics;//游戏内遗物的总父物体
    private void Awake()
    {
        Instance = this;
    }

    public void GetRelic(int relicID)
    {
        GameObject relicGo=Instantiate(relicPrefab,inGameRelics.transform );
        relicGo.transform.localPosition = new Vector3(-900+70*inGameRelicList.Count, 410,0);
        Relic newRelic = relicGo.GetComponent<Relic>();
        inGameRelicList.Add(newRelic);
        foreach (var data in relicDataList)
        {
            if (data.relicID==relicID)
            {
                newRelic.InitRelic(data);
                break;
            }
        }
        newRelic.OnGetRelic();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetRelic(0);
        }
    }
}
