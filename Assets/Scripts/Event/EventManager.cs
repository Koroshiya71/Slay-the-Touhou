using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    public List<GameEvent> eventList = new List<GameEvent>(); //事件列表
    public List<GameObject> choiceButtonList = new List<GameObject>(); //事件选择按钮列表
    public int choiceIndex; //选择下标
    public Canvas eventCanvas; //事件画布
    public Text desText; //事件描述的文本
    public Text titleText;//事件名称的文本
    public static EventManager Instance;
    public GameObject returnButton;//返回按钮
    private void Awake()
    {
        Instance = this;
    }

    void InitButtonCallback() //初始化各按钮回调
    {
        foreach (var btn in choiceButtonList)
        {
            btn.GetComponent<Button>().onClick.AddListener((() =>
            {
                choiceIndex = choiceButtonList.IndexOf(btn) + 1;
            }));
        }
        returnButton.GetComponent<Button>().onClick.AddListener((() =>
        {
            eventCanvas.enabled = false;
            
        }));
    }
    void Start()
    {
        eventCanvas.enabled = false;
        InitButtonCallback();
    }

    public void StartEvent(int evtID)
    {
        foreach (var evt in eventList)
        {
            if (evt.eventID==evtID)
            {
                StartCoroutine(GameEventStart(evt));

            }
        }
    }
    public IEnumerator GameEventStart(GameEvent evt)
    {
        InitEvent();
        switch (evt.eventID)
        {
            case 0:
                titleText.text = "诡异的幽灵";
                desText.text = evt.descriptionList[0];
                for (int i = 0; i < 3; i++)
                {
                    choiceButtonList[i].SetActive(true); 
                    choiceButtonList[i].GetComponentInChildren<Text>().text = evt.choiceList[i];
                }
                while (choiceIndex <= 0)
                {
                    yield return 0;
                }
                switch (choiceIndex)
                {
                    case 1://回复十滴血，下场战斗开始时获得5层焕发
                        desText.text = evt.descriptionList[1];
                        Player.Instance.Recover(10);
                        BattleManager.Instance.actionsTurnStart.Add((() =>
                        {
                            StateManager.AddStateToPlayer(new Value(){type=Value.ValueType.焕发,value=5});
                        }));
                        break;
                    case 2:
                        desText.text = evt.descriptionList[2];

                        break;
                    case 3:
                        //TODO:增加两点属性点
                        desText.text = evt.descriptionList[3];

                        break;
                }
                foreach (var btn in choiceButtonList)
                {
                    btn.SetActive(false);
                }
                returnButton.SetActive(true);
                yield break;
            case 1:
                titleText.text = "埋藏的东西";
                desText.text = evt.descriptionList[0];
                for (int i = 0; i < 3; i++)
                {
                    choiceButtonList[i].SetActive(true);
                    choiceButtonList[i].GetComponentInChildren<Text>().text = evt.choiceList[i];
                }
                while (choiceIndex <= 0)
                {
                    yield return 0;
                }
                switch (choiceIndex)
                {
                    case 1:
                        //TODO:获得遗物，减少力量

                        desText.text = evt.descriptionList[1];
                        break;
                    case 2:
                        desText.text = evt.descriptionList[2];
                        //TODO:稀有选牌机会
                        List<CardData> choiceList=new List<CardData>();
                        while (choiceList.Count<3)
                        {
                            CardData newData =
                                CardManager.Instance.CardDataList[
                                    Random.Range(0, CardManager.Instance.CardDataList.Count)];
                            if (newData.rare==CardData.CardRare.Epic&&!choiceList.Contains(newData)&&newData.type!=Card.CardType.符卡)
                            {
                                choiceList.Add(newData);

                            }

                        }
                        StartCoroutine(CardManager.Instance.AddCardToDesk(choiceList, 1));
                        break;
                    case 3:
                        desText.text = evt.descriptionList[3];
                        Player.Instance.Recover(22);
                        break;
                }
                foreach (var btn in choiceButtonList)
                {
                    btn.SetActive(false);
                }
                returnButton.SetActive(true);
                yield break;
        }
    }

    void Update()
    {

    }

    public void InitEvent()//初始化事件
    {
        choiceIndex = 0;
        eventCanvas.enabled = true;
        returnButton.SetActive(false);
    }
}