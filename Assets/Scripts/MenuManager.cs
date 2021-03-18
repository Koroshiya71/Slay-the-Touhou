using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour//��������һϵ��UI�¼�
{
    public static MenuManager Instance;
    public List<Card> showCardList;//������ʾ�Ŀ����б�
    public GameObject cardDisplayCanvas;//����Ԥ���ƶ�����Ļ���
    public RectTransform displayContent;//�����˵������ݷ�Χ
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
        Card[] showCards = GetComponentsInChildren<Card>();
        foreach (var card in showCards)
        {
            showCardList.Add(card);//������Ԥ�ƿ��Ƽ�������б�
        }
    }
    public void ShowDeskButtonDown() //�鿴�ƿⰴť���µĻص��¼�
    {
        cardDisplayCanvas.SetActive(true);

    }
    public void ShowDrawCardButtonDown() //�鿴���ƶѰ�ť���µĻص��¼�
    {
        cardDisplayCanvas.SetActive(true);
        displayContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,//���ݳ��ƶѵĿ���������̬����content��С
            340+(CardManager.Instance.drawCardList.Count%5-2)*160);
        
    }
    public void ShowDiscardButtonDown() //�鿴���ƶѶѰ�ť���µĻص��¼�
    {
        cardDisplayCanvas.SetActive(true);

    }

    public void NotShowButtonDown() //ȡ������Ԥ����ť���µĻص��¼�
    {

    }

    void Update()
    {
        
    }
}
