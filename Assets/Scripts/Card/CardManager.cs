using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    public Card selectedCard;//ѡ�еĿ���
    public GameObject cardPrefab; //����Ԥ����
    public GameObject showCard;//����չʾ�Ŀ���
    #region �����ƿ�
    public List<CardData> discardList=new List<CardData>();//���ƶ�
    public List<CardData> drawCardList = new List<CardData>();//���ƶ�
    public List<string> cardDeskList=new List<string>();//�ƿ�
    public List<GameObject> handCardList = new List<GameObject>(); //�����б�
#endregion

    public GameObject BeginPos; //�������Ƶ��ʼ��λ
    private float rotateAngel; //���ƶ�����ת�ĽǶ�
    public List<Sprite> spriteList=new List<Sprite>();
    public List<CardData> cardDataList=new List<CardData>();
    public bool hasShow;//�Ƿ��Ѿ�չʾ�˿���
    public Text discardNumText;//���ƶѿ����������ı�
    public Text drawCardNumText;//���ƶѿ����������ı�
    private void Start()
    {
        showCard.SetActive(false);
        cardDataList = DataManager.Instance.LoadCardData();
    }
    
    public void InitDrawCardList()
    {
        if (discardList.Count>0)//����������ƶ��п��Ƶ�����³�ʼ�����ƶѣ����������ƶѵĿ��Ƽ�����ƶ�
        {
            foreach (var dCard in discardList)
            {
                drawCardList.Add(dCard);
            }

            discardList = new List<CardData>();//Ȼ���ʼ�����ƶ�
        }
        
        if (drawCardList.Count==0)//�����ʱ���ƶ���ȻΪ�գ���ô˵���ǵ�һ�δ��ƿ��ʼ�����ƶ�
        {
            foreach (var cardId in cardDeskList)//Ȼ������ƿ���ÿ���Ƶ�ID�Ų�������
            {
                foreach (var cardData in cardDataList)
                {
                    if (cardId==cardData.cardID)//���Ҷ�ӦID�Ŀ�������
                    {
                        drawCardList.Add(cardData);
                        break;
                    }

                }
                

            }
        }

        drawCardList=RandomSortList(drawCardList);//������ҳ��ƶ�˳��

    }
    public List<T> RandomSortList<T>(List<T> ListT)//���б�����������
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
    
    private void Update()
    {
        UpdateUIState();
    }

    public void DrawCard() //�ӳ��ƶѳ鿨
    {
        //TODO �����Ż������
        //��¡Ԥ��
        var handCardGo = Instantiate(cardPrefab) as GameObject;
        if (drawCardList.Count==0)//������ƿɳ飬���ʼ�����ƶ�
        {
            InitDrawCardList();
        }
        var newCard = handCardGo.GetComponent<Card>();
        newCard.InitCard(drawCardList[0]);//���ݳ��ƶѵĵ�һ�ſ�����������ʼ������
        drawCardList.Remove(drawCardList[0]);//�������Ƴ����ƶ�

        handCardGo.transform.position = BeginPos.transform.position;
        handCardGo.transform.SetParent(transform);

        //����������ӵ������б�
        handCardList.Add(handCardGo);

        //���㶯����Ҫ��ת�ĽǶ�
        RotateAngel();
        //���ų鿨����
        AddCardAnimations();
    }
    public void GetCards(int cardNo)//���ݿ��Ƶ���Ż�ȡָ������
    {
        //��¡Ԥ��
        var handCardGo = Instantiate(cardPrefab) as GameObject;
        Card newCard = handCardGo.GetComponent<Card>();
        newCard.InitCard(cardDataList[cardNo]);

        handCardGo.transform.position = BeginPos.transform.position;
        handCardGo.transform.SetParent(transform);

        //����������ӵ������б�
        handCardList.Add(handCardGo);
        //���㶯����Ҫ��ת�ĽǶ�
        RotateAngel();
    }

    public void UpdateUIState() //����UI���״̬
    {
        drawCardNumText.text = drawCardList.Count+"";
        discardNumText.text = discardList.Count + "";
    }
    //Ϊ������Ӷ���
    private void HandCardAnimation(GameObject GO, float Vec3_Z)
    {
        GO.transform.DORotate(new Vector3(0, 0, Vec3_Z), 0.3F, RotateMode.Fast);
    }

    //��������ʱ���ŵĶ���
    private void AddCardAnimations()
    {
        if (handCardList.Count == 1)
        {
            HandCardAnimation(handCardList[0], 0);
        }
        else
        {
            for (var i = 1; i < handCardList.Count; i++)
                HandCardAnimation(handCardList[i - 1], 30 - rotateAngel * (float) i * handCardList.Count + 2.5F);
            HandCardAnimation(handCardList[handCardList.Count - 1], -27.5F + rotateAngel);
        }
    }

    //ʹ������ʱ���ŵĶ���
    public void UseCard()
    {
        int index =handCardList.IndexOf(selectedCard.gameObject);

        if (handCardList[index]!=null)
        {
            Destroy(handCardList[index]);
            handCardList.Remove(handCardList[index]);
            RotateAngel();
            if (handCardList.Count == 1)
                HandCardAnimation(handCardList[0], 0);
            else if (handCardList.Count > 1)
                for (var i = 1; i < handCardList.Count + 1; i++)
                {
                    HandCardAnimation(handCardList[i - 1], 30 - rotateAngel * (float)i * handCardList.Count + 2.5F);
                    HandCardAnimation(handCardList[handCardList.Count - 1], -27.5F + rotateAngel);
                }
        }
        
    }

    public void DropAllCard() //������������
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            Discard(handCardList[i].GetComponent<Card>());
            Destroy(handCardList[i]);
            
            //TODO:�����Ͳ������ƶ���
        }

        handCardList = new List<GameObject>();


    }
    //������Ҫ��ת�ĽǶ�
    private void RotateAngel()
    {
        rotateAngel = 55F / (float) handCardList.Count / (float) handCardList.Count;
    }

    public void Discard(Card card) //����
    {
        discardList.Add(card.cardData);

    }
    private void Awake()
    {
        Instance = this;
    }
}