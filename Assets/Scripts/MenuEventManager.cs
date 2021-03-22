using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventManager : MonoBehaviour//��������һϵ��UI�¼�
{
    public static MenuEventManager Instance;
    public List<Card> showCardList;//������ʾ�Ŀ����б�
    public GameObject cardDisplayCanvas;//����Ԥ���ƶ�����Ļ���
    public RectTransform displayContent;//�����˵������ݷ�Χ
    public bool isPreviewing;//�Ƿ�������ʾ����

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitShowCardCanvas();
    }

    void InitShowCardCanvas() //�Կ���Ԥ��������г�ʼ��
    {
        cardDisplayCanvas.SetActive(false);//ȡ����ʾ
        Card[] showCards = cardDisplayCanvas.GetComponentsInChildren<Card>();
        foreach (var card in showCards)
        {
            showCardList.Add(card);//������Ԥ�ƿ��Ƽ�������б�
        }

    }

    
    public void ChooseCardFromDesk()//��������ķ�Χ���п���ѡ��
    {
        CardManager.Instance.isChoosing = true;
        cardDisplayCanvas.SetActive(true);
        List<CardData> optionalList= CardManager.Instance.optionalCardList;
        for (int i = 0; i < optionalList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//����ͬ�ڳ��ƶ�������չʾ���Ƴ�ʼ������ʾ����
            showCardList[i].InitCard(optionalList[i]);
        }
        for (int i = optionalList.Count; i < showCardList.Count; i++)//����������������ʾ
        {
            showCardList[i].gameObject.SetActive(false);
        }
    }
    public void ExitDisplayButtonDown() //ȡ��Ԥ�����ư�ť�Ļص��¼�
    {
        isPreviewing = false;
        cardDisplayCanvas.SetActive(false);
    }
    public void ShowDeskButtonDown() //�鿴�ƿⰴť���µĻص��¼�
    {
        isPreviewing = true;

        List<string> deskList = CardManager.Instance.cardDeskList;
        cardDisplayCanvas.SetActive(true);
        displayContent.sizeDelta = new Vector2(1835, 939 + (deskList.Count / 5 - 1) * 420);
        for (int i = 0; i < deskList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//����ͬ�����ƶ�������չʾ���Ƴ�ʼ������ʾ����
            foreach (var data in CardManager.Instance.cardDataList)
            {
                if (data.cardID == deskList[i])
                {
                    showCardList[i].InitCard(data);
                    break;
                }
            }
        }

        for (int i = deskList.Count; i < showCardList.Count; i++)//����������������ʾ
        {
            showCardList[i].gameObject.SetActive(false);
        }

    }
    public void ShowDrawCardButtonDown() //�鿴���ƶѰ�ť���µĻص��¼�
    {
        isPreviewing = true;

        List<CardData> drawCardList = CardManager.Instance.RandomSortList(CardManager.Instance.drawCardList);
        cardDisplayCanvas.SetActive(true);
        displayContent.sizeDelta = new Vector2(1835, 939 + (drawCardList.Count / 5 - 2) * 420);
        for (int i = 0; i < drawCardList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//����ͬ�ڳ��ƶ�������չʾ���Ƴ�ʼ������ʾ����
            showCardList[i].InitCard(drawCardList[i]);
        }

        for (int i = drawCardList.Count; i < showCardList.Count; i++)//����������������ʾ
        {
            showCardList[i].gameObject.SetActive(false);
        }
        
    }
    public void ShowDiscardButtonDown() //�鿴���ƶѶѰ�ť���µĻص��¼�
    {
        isPreviewing = true;

        List<CardData> discardList = CardManager.Instance.discardList;
        cardDisplayCanvas.SetActive(true);
        displayContent.sizeDelta=new Vector2(1835, 939 + (discardList.Count / 5 - 2) * 420);
       
        for (int i = 0; i < discardList.Count; i++)
        {
            showCardList[i].gameObject.SetActive(true);//����ͬ�����ƶ�������չʾ���Ƴ�ʼ������ʾ����
            showCardList[i].InitCard(discardList[i]);
        }

        for (int i = discardList.Count; i < showCardList.Count; i++)//����������������ʾ
        {
            showCardList[i].gameObject.SetActive(false);
        }

    }

    public void NotShowButtonDown() //ȡ������Ԥ����ť���µĻص��¼�
    {

    }

    void Update()
    {
        
    }
}
