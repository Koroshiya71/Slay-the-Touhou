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
