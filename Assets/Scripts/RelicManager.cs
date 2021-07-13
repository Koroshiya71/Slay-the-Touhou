using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    public List<RelicData> relicDataList = new List<RelicData>();//存储所有遗物数据的列表
    public List<Relic> inGameRelicList = new List<Relic>();//游戏中玩家拥有的遗物列表
    public List<Sprite> relicSpriteList = new List<Sprite>();//遗物图片列表
    public GameObject relicPrefab;//遗物的预制体
    public GameObject inGameRelics;//游戏内遗物的总父物体
    public Text relicInfoText;//遗物信息文本
    public bool isWuShu = false;//武术姿态是否触发
    private void Awake()
    {
        Instance = this;
    }

    public void GetRelic(int relicID)
    {
        GameObject relicGo = Instantiate(relicPrefab, inGameRelics.transform);
        relicGo.transform.localPosition = new Vector3(-900 + 70 * inGameRelicList.Count, 410, 0);
        Relic newRelic = relicGo.GetComponent<Relic>();
        inGameRelicList.Add(newRelic);
        foreach (var data in relicDataList)
        {
            if (data.relicID == relicID)
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
                case 4://植轮人偶
                    StateManager.AddStateToPlayer(new Value
                    {
                        type = Value.ValueType.魂体,
                        value = 2
                    });
                    StateManager.AddStateToPlayer(new Value
                    {
                        type = Value.ValueType.灵体,
                        value = 2
                    });
                    break;
            }
        }
    }
    public void RelicEffectOnTurnEnd()//在回合结束时触发的遗物效果
    {
        foreach (var relic in inGameRelicList)
        {
            switch (relic.relicData.relicID)
            {
                case 3://偷来的书
                    if (Player.Instance.energy > 0)
                    {
                        BattleManager.Instance.actionsTurnStart.Add((() =>
                        {
                            Player.Instance.GetEnergy(1);
                        }));
                    }

                    break;
            }
        }
    }

    void Start()
    {
        TestGetRelic();
    }
    //测试用遗物获取方法

    void TestGetRelic()
    {
        if (Input.GetKeyDown(KeyCode.A))//神社的贡品
        {
            GetRelic(0);
        }
        if (Input.GetKeyDown(KeyCode.B))//⑨的人偶
        {
            GetRelic(1);
        }
        if (Input.GetKeyDown(KeyCode.C))//上海人形
        {
            GetRelic(2);
        }
        if (Input.GetKeyDown(KeyCode.D))//偷来的书
        {
            GetRelic(3);
        }
        if (Input.GetKeyDown(KeyCode.E))//植轮人偶
        {
            GetRelic(4);
        }
        if (Input.GetKeyDown(KeyCode.F))//御守
        {
            GetRelic(5);
        }
        if (Input.GetKeyDown(KeyCode.G))//小阴阳玉
        {
            GetRelic(6);
        }
        if (Input.GetKeyDown(KeyCode.H))//武术心得
        {
            GetRelic(7);
        }
        if (Input.GetKeyDown(KeyCode.I))//紫的阳伞
        {
            GetRelic(8);
        }
        if (Input.GetKeyDown(KeyCode.J))//山童的钱包
        {
            GetRelic(9);
        }
        if (Input.GetKeyDown(KeyCode.K))//团子
        {
            GetRelic(10);
        }
        if (Input.GetKeyDown(KeyCode.L))//弱肉强食之证
        {
            GetRelic(11);
        }
    }
    void Update()
    {
        TestGetRelic();
       
    }
}
