using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Relic : MonoBehaviour
{
    public enum RelicRare//�����ϡ�ж�
    {
        ��С������,
        һ������,
        ��˵����
    }

    public Image relicImg;
    public RelicData relicData=new RelicData();
    void Start()
    {
    }

    public void InitRelic(RelicData data) //��ʼ������
    {
        relicData.relicID = data.relicID;
        relicData.relicDes = data.relicDes;
        relicData.relicName = data.relicName;
        relicData.relicRare = data.relicRare;
        relicImg.sprite = RelicManager.Instance.relicSpriteList[relicData.relicID];
        relicImg.enabled = true;
    }
    public void OnGetRelic() //��ȡ����ʱ�������¼�
    {
        switch (relicData.relicID)
        {
            case 0://����Ĺ�Ʒ
                List<CardData> choiceList = new List<CardData>();
                while (choiceList.Count < 3)
                {
                    CardData newData =
                        CardManager.Instance.CardDataList[
                            Random.Range(0, CardManager.Instance.CardDataList.Count)];
                    if (newData.rare == CardData.CardRare.Epic && !choiceList.Contains(newData) && newData.type != Card.CardType.����)
                    {
                        choiceList.Add(newData);
                    }
                }
                StartCoroutine(CardManager.Instance.AddCardToDesk(choiceList, 1));
                break;
        }
    }
    void Update()
    {
        
    }
}
