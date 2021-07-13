using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Relic : MonoBehaviour
{
    public enum RelicRare//遗物的稀有度
    {
        弱小的遗物,
        一般遗物,
        传说遗物
    }

    public Image relicImg;
    public RelicData relicData = new RelicData();
    void Start()
    {
    }

    public void InitRelic(RelicData data) //初始化遗物
    {
        relicData.relicID = data.relicID;
        relicData.relicDes = data.relicDes;
        relicData.relicName = data.relicName;
        relicData.relicRare = data.relicRare;
        relicImg.sprite = RelicManager.Instance.relicSpriteList[relicData.relicID];
        relicImg.enabled = true;
    }
    public void OnGetRelic() //获取遗物时触发的事件
    {
        switch (relicData.relicID)
        {
            case 0://神社的贡品
                List<CardData> choiceList = new List<CardData>();
                while (choiceList.Count < 3)
                {
                    CardData newData =
                        CardManager.Instance.CardDataList[
                            Random.Range(0, CardManager.Instance.CardDataList.Count)];
                    if (newData.rare == CardData.CardRare.Epic && !choiceList.Contains(newData) && newData.type != Card.CardType.符卡)
                    {
                        choiceList.Add(newData);
                    }
                }
                StartCoroutine(CardManager.Instance.AddCardToDesk(choiceList, 1));
                break;

            case 5://御守   
                //回复75%生命值
                Player.Instance.Recover((int)(0.75 * Player.Instance.maxHp));
                break;

            case 8://紫的阳伞
                //将回合数置0
                BattleManager.Instance.yokariPrasolCount = 0;
                break;
        }
    }
    void Update()
    {

    }

    public void OnPointerEnter()//当鼠标进入遗物时触发的事件
    {
        RelicManager.Instance.relicInfoText.enabled = true;
        RelicManager.Instance.relicInfoText.text = relicData.relicName + "\n" + relicData.relicDes;
        RelicManager.Instance.relicInfoText.transform.localPosition = transform.localPosition + new Vector3(20, -70, 0);
    }
    public void OnPointerExit()//当鼠标进入遗物时触发的事件
    {
        RelicManager.Instance.relicInfoText.enabled = false;
    }
}
