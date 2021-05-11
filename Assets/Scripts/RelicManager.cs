using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    public List<RelicData> relicDataList=new List<RelicData>();//�洢�����������ݵ��б�
    public List<Relic> inGameRelicList = new List<Relic>();//��Ϸ�����ӵ�е������б�
    public List<Sprite> relicSpriteList = new List<Sprite>();//����ͼƬ�б�
    public GameObject relicPrefab;//�����Ԥ����
    public GameObject inGameRelics;//��Ϸ��������ܸ�����
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
