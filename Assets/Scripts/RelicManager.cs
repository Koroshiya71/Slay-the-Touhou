using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    public List<RelicData> relicDataList=new List<RelicData>();//存储所有遗物数据的列表
    public List<Relic> inGameRelicList = new List<Relic>();//游戏中玩家拥有的遗物列表
    public List<Sprite> relicSpriteList = new List<Sprite>();//遗物图片列表
    public GameObject relicPrefab;//遗物的预制体
    public GameObject inGameRelics;//游戏内遗物的总父物体
    public Text relicInfoText;//遗物信息文本
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

    public bool CheckRelic(int ID) //查找是否拥有该遗物
    {
        foreach (var relic in inGameRelicList)
        {
            if (relic.relicData.relicID == ID)
                return true;
        }
        return false;
    }

    public void RelicEffectOnBattleStart()//在战斗开始时触发的遗物效果
    {
        foreach (var relic in inGameRelicList)
        {
            switch (relic.relicData.relicID)
            {
                case 1://⑨的人偶
                    BattleManager.Instance.actionsTurnStart.Add((() =>
                    {
                        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                        {
                            enemy.TakeDamage(9);
                        }
                    }));
                    break;
                case 2://上海人形
                    AllyManager.Instance.CreateAlly(0);
                    break;
            }
        }
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetRelic(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetRelic(2);
        }
    }
}
