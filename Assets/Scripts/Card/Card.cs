using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum CardType//��������
    {
        ����,//����
        ��Ļ,//��Ļ
        ����,//����
        ����,//����
        ����,//����
        ����//����
    }

    public Transform outLook;//��۵�λ��
    #region ��������
    //չʾ�ÿ��Ƶ���Ϸ����
    private GameObject showGo;
    //�Ƿ��Ѿ������������
    public bool canXin;
    //�Ƿ��Ѿ�ʹ�ù���
    public bool hasUsed;
    //�Ƿ�������չʾ�Ŀ���
    public bool isShowCard;
    //��������
    public CardData cardData;
    //Ч���ֵ�
    public Dictionary<Value.ValueType, int> valueDic = new Dictionary<Value.ValueType, int>();
    #endregion

    #region UI����
    //�����ı�
    public Text nameText;
    //���������ı�
    public Text desText;
    //���������ı�
    public Text costText;
    //��ͼ
    public Image img;
    #endregion

    public Vector3 selectPos;
    public void OnPointerDown()
    {

        if (CardManager.Instance.isChoosing)//�������ѡ����
        {
            CardManager.Instance.chosenCardList.Add(this.cardData);       
        }
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
            return;
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
        showGo.SetActive(false);//ȡ������չʾ
        CardManager.Instance.hasShow = false;
 
        CardManager.Instance.selectedCard = this;
    }

    public void OnPointerEnter()
    {
        Debug.Log("Enter" );
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
        {
            return;
        }
        showGo.SetActive(true);//չʾ����
        showGo.GetComponent<Card>().InitCard(cardData);
        CardManager.Instance.hasShow = true;
        outLook.transform.position = new Vector3(outLook.transform.position.x, outLook.transform.position.y + 0.5f);

    }

    public void OnPointerExit()
    {
        Debug.Log("exit");

        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }


        showGo.SetActive(false);
        CardManager.Instance.hasShow = false;
        outLook.transform.position = new Vector3(outLook.transform.position.x, outLook.transform.position.y - 0.5f);
    }

    public  void OnPointerUp()
    {
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }
        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
            return;
        if (CardManager.Instance.selectedCard==null)
        {
            return;
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)//�������ק���Ƶ�һ��λ�����ϲ�������˿��Ƶ�ʹ��
        {
            CardEffectManager.Instance.UseThisCard(CardManager.Instance.selectedCard);
        }
        else
        {
            CardManager.Instance.selectedCard = null;

        }
    }

    

    public void InitCard(CardData data)//����CardData��ʼ������
    {
        cardData = CardData.Clone(data);
        valueDic = new Dictionary<Value.ValueType, int>();
        canXin = false;
        hasUsed = false;
        cardData.keepChangeInBattle = false;
        costText.text = ""+cardData.cost;
        nameText.text = cardData.name;
        img.sprite = CardManager.Instance.spriteList[cardData.spriteID];

        foreach (var v in cardData.valueList) //��ʼ������Ч���ֵ�
        {
            valueDic.Add(v.type, v.value);
        }

        cardData.des = "";

        InitDes();//��ʼ���ı�����
        desText.text = cardData.des;

    }

    public void UpdateCardState() //���¿���״̬
    {
        switch (cardData.cardID)
        {
            case "0004"://�������ĵ�
                if (BattleManager.Instance.hasCanXin)
                {
                    cardData.cost -= 1;
                    InitCard(cardData);
                }
                break;
        }
    }
    public void InitDes()//���ݿ���Ч���ֵ��ʼ�������ı�
    {
        switch (cardData.cardID)
        {
            case "0004"://�������ĵ�
                cardData.des = "���һ��������ĵã�����ϻغϴ��������ģ�����-1";
                return;
            case "0008"://�ϵ�һ��
                cardData.des = "������ս�У�������ת״̬����Ĳ���Ч����Ϊ���������������ǻغϽ���ʱ������\n�޺���";
                return;
            case "0009"://���罣��������Ѯ֮һ����
                cardData.des = "���һ������Ļغϡ��ڶ���Ļغϣ�ֻ��ʹ�������ơ�\n�޺���";
                break;
        }
        
        //������˺�KEY�����
        if (valueDic.ContainsKey(Value.ValueType.�˺�))
        {
            cardData.des += "���"+valueDic[Value.ValueType.�˺�]+"���˺�";
            if (cardData.times>1)
            {
                cardData.des += cardData.times + "��";
            }
        }
        //����л���KEY�����
        if (valueDic.ContainsKey(Value.ValueType.����))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "���" + valueDic[Value.ValueType.����] + "�㻤��";
            if (cardData.times > 1)
            {
                cardData.des += cardData.times + "��";
            }
        }
        //����ж�����Key�������
        if (valueDic.ContainsKey(Value.ValueType.������))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "���غϻ�ö�����״̬";
        }
        //����в��ĵ������
        if (cardData.canXinList.Count>0)
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }

            cardData.des += "���ģ�";
            foreach (var canXin in cardData.canXinList)
            {
                if (!canXin.IsTurnEnd&& cardData.canXinList.IndexOf(canXin) == 0)
                {
                    cardData.des += "���¸��غϿ�ʼʱ,";
                }
                if (cardData.canXinList.IndexOf(canXin) > 0)
                {
                    cardData.des += ",";
                }
                switch (canXin.CanXinValue.type)
                {
                    case Value.ValueType.�˺�:
                        cardData.des += "���"+canXin.CanXinValue.value+"���˺�";
                        break;
                    case Value.ValueType.����:
                        cardData.des += "���" + canXin.CanXinValue.value + "�㻤��";
                        break;
                    case Value.ValueType.�ط�:
                        cardData.des += "���" + canXin.CanXinValue.value + "������";
                        break;
                }
            }
            
        }
        //�������ն�������
        if (cardData.comboList.Count > 0)
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }

            foreach (var combo in cardData.comboList)
            {
                cardData.des += "��ն"+combo.comboNum+"��";
                switch (combo.comboValue.type)
                {
                    case Value.ValueType.�˺�:
                        cardData.des += "���" + combo.comboValue.value + "���˺�";
                        break;
                    case Value.ValueType.����:
                        cardData.des += "���" + combo.comboValue.value + "�㻤��";
                        break;
                    case Value.ValueType.�ط�:
                        cardData.des += "���" + combo.comboValue.value + "������";
                        break;
                }
            }
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
