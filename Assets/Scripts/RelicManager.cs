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
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
