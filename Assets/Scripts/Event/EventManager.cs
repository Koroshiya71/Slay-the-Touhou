using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    public List<GameEvent> eventList = new List<GameEvent>(); //�¼��б�
    public List<GameObject> choiceButtonList = new List<GameObject>(); //�¼�ѡ��ť�б�
    public int choiceIndex; //ѡ���±�
    public Canvas eventCanvas; //�¼�����
    public Text desText; //�¼��������ı�
    public Text titleText;//�¼����Ƶ��ı�
    public static EventManager Instance;
    public GameObject returnButton;//���ذ�ť
    private void Awake()
    {
        Instance = this;
    }

    void InitButtonCallback() //��ʼ������ť�ص�
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
                titleText.text = "���������";
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
                    case 1://�ظ�ʮ��Ѫ���³�ս����ʼʱ���5�����
                        desText.text = evt.descriptionList[1];
                        Player.Instance.Recover(10);
                        BattleManager.Instance.actionsTurnStart.Add((() =>
                        {
                            StateManager.AddStateToPlayer(new Value(){type=Value.ValueType.����,value=5});
                        }));
                        break;
                    case 2:
                        desText.text = evt.descriptionList[2];

                        break;
                    case 3:
                        //TODO:�����������Ե�
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
                titleText.text = "��صĶ���";
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
                        //TODO:��������������

                        desText.text = evt.descriptionList[1];
                        break;
                    case 2:
                        desText.text = evt.descriptionList[2];
                        //TODO:ϡ��ѡ�ƻ���
                        List<CardData> choiceList=new List<CardData>();
                        while (choiceList.Count<3)
                        {
                            CardData newData =
                                CardManager.Instance.CardDataList[
                                    Random.Range(0, CardManager.Instance.CardDataList.Count)];
                            if (newData.rare==CardData.CardRare.Epic&&!choiceList.Contains(newData)&&newData.type!=Card.CardType.����)
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

    public void InitEvent()//��ʼ���¼�
    {
        choiceIndex = 0;
        eventCanvas.enabled = true;
        returnButton.SetActive(false);
    }
}