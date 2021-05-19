using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    public List<RelicData> relicDataList=new List<RelicData>();//�洢�����������ݵ��б�
    public List<Relic> inGameRelicList = new List<Relic>();//��Ϸ�����ӵ�е������б�
    public List<Sprite> relicSpriteList = new List<Sprite>();//����ͼƬ�б�
    public GameObject relicPrefab;//�����Ԥ����
    public GameObject inGameRelics;//��Ϸ��������ܸ�����
    public Text relicInfoText;//������Ϣ�ı�
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

    public bool CheckRelic(int ID) //�����Ƿ�ӵ�и�����
    {
        foreach (var relic in inGameRelicList)
        {
            if (relic.relicData.relicID == ID)
                return true;
        }
        return false;
    }

    public void RelicEffectOnBattleStart()//��ս����ʼʱ����������Ч��
    {
        foreach (var relic in inGameRelicList)
        {
            switch (relic.relicData.relicID)
            {
                case 1://�����ż
                    BattleManager.Instance.actionsTurnStart.Add((() =>
                    {
                        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                        {
                            enemy.TakeDamage(9);
                        }
                    }));
                    break;
                case 2://�Ϻ�����
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
