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
    public float posY;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CardManager.Instance.selectedCard=null;
        }
    }

    public Vector3 selectPos;
    public void OnPointerDown()
    {

        if (isShowCard)//�����չʾ�õĿ����򲻽��м��
            return;
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }

        if (CardManager.Instance.isChoosingFromHand)//������ڴ�����ѡ����
        {
            var localPosition = outLook.localPosition;
            posY = localPosition.y;
            if (CardManager.Instance.chosenCardList.Contains(this))
            {
                CardManager.Instance.chosenCardList.Remove(this);
                outLook.localPosition = new Vector3(localPosition.x, posY-15 );
                return;

            }
            CardManager.Instance.chosenCardList.Add(this);

            
            localPosition = new Vector3(localPosition.x, posY+15);
            outLook.localPosition = localPosition;
        }

        
        showGo.SetActive(false);//ȡ������չʾ
        CardManager.Instance.hasShow = false;
 
        CardManager.Instance.selectedCard = this;
    }

    public void OnPointerEnter()
    {

        if (CardManager.Instance.isChoosingFromHand)//�������ѡ�����򲻽��м��
        {
            return;
        }
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

        var localPosition = outLook.localPosition;
        posY = localPosition.y;
        localPosition = new Vector3(localPosition.x, posY+15);
        outLook.localPosition = localPosition;
    }

    public void OnPointerExit()
    {

        if (CardManager.Instance.isChoosingFromHand)//�������ѡ�����򲻽��м��
        {
            return;
        }
        if (MenuEventManager.Instance.isPreviewing)//������ڽ��п���Ԥ���򲻽��м��
        {
            return;
        }


        showGo.SetActive(false);
        CardManager.Instance.hasShow = false;
        var localPosition = outLook.localPosition;
        localPosition = new Vector3(localPosition.x, posY);
        outLook.localPosition = localPosition;

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

        if (CardManager.Instance.isChoosingFromHand)
        {
            return;
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)//�������ק���Ƶ�һ��λ�����ϲ�������˿��Ƶ�ʹ��
        {
            StartCoroutine(CardEffectManager.Instance.UseThisCard(CardManager.Instance.selectedCard));
        }
        else
        {
            CardManager.Instance.selectedCard = null;

        }
    }

    

    public void InitCard(CardData data)//����CardData��ʼ������
    {
        if (!cardData.keepChangeInBattle) //����Կ��Ƶ��޸Ĳ��ǿɳ�����
            foreach (var originalData in CardManager.Instance.CardDataList)
                if (cardData.cardID == originalData.cardID)
                    cardData=CardData.Clone(originalData);
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
                cardData.des = "���2�㡸��������������ϻغϴ��������ģ�����-1";
                return;
            case "0008"://�ϵ�һ��
                cardData.des = "������ս�У����á���ת��״̬����Ĳ���Ч����Ϊ���������������ǻغϽ���ʱ������\n�޺���";
                return;
            case "0009"://���罣��������Ѯ֮һ����
                cardData.des = "���һ������Ļغϡ��ڶ���Ļغϣ�ֻ��ʹ�������ơ�\n�޺���";
                return;
            case "0010"://ڤ��
                cardData.des = "�������ƣ�Ȼ��ѡ��������ϴ���ƿ⣬�������ƻ���޺���";
                return;
            case "0013"://�Ӻ�
                cardData.des += "�������Ϊ���غ�ʹ�õĵ�һ�������ƣ����"+valueDic[Value.ValueType.�˺�]+"���˺�";
                cardData.des += "\n���ģ����"+cardData.canXinList[0].CanXinValue.value+"���˺�";
                return;
            case "0018"://
                cardData.des = "��������ƻ���޺��У�����ĳ��ƶ�Ϊ��ʱ�����Ƹ�Ϊ������һ��������\n�޺���";

                return;
        }

        //������˺�KEY�����
        if (valueDic.ContainsKey(Value.ValueType.�˺�))
        {
            if (cardData.targetType==CardData.TargetType.�������)
            {
                cardData.des += "���";
            }
            if (cardData.targetType == CardData.TargetType.ȫ������)
            {
                cardData.des += "�����е���";
            }
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
            cardData.des += "���غϻ�á���������״̬";
        }
        //����г���Key�������
        if (valueDic.ContainsKey(Value.ValueType.����))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "��"+valueDic[Value.ValueType.����]+"����";
        }
        //����лط�Key�������
        if (valueDic.ContainsKey(Value.ValueType.�ط�))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "�ظ�" + valueDic[Value.ValueType.�ط�] + "������";
        }
        //����б�ˮһսKey�������
        if (valueDic.ContainsKey(Value.ValueType.��ˮһս))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "��á���ˮһս��������ս���޷��ٻ�û��ף�";
        }
        //���������Key�������
        if (valueDic.ContainsKey(Value.ValueType.����))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "���"+valueDic[Value.ValueType.����]+"�����ƣ�������նʱ��Ч�����ⴥ��һ�Σ�����һ�����ƣ�";
        }
        //����б�������Key�������
        if (valueDic.ContainsKey(Value.ValueType.��������))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "���غϽ���ʱ�����������";
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
                        cardData.des += "�����"+canXin.CanXinValue.value+"���˺�";
                        break;
                    case Value.ValueType.����:
                        cardData.des += "���" + canXin.CanXinValue.value + "�㻤��";
                        break;
                    case Value.ValueType.�ط�:
                        cardData.des += "���" + canXin.CanXinValue.value + "������";
                        break;
                    case Value.ValueType.��Ѫ:
                        cardData.des += "�ظ�" + canXin.CanXinValue.value + "������";
                        break;
                    case Value.ValueType.����:
                        cardData.des += "����" + canXin.CanXinValue.value + "�㾪��";
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
                        cardData.des += "�����" + combo.comboValue.value + "���˺�";
                        break;
                    case Value.ValueType.����:
                        cardData.des += "���" + combo.comboValue.value + "�㻤��";
                        break;
                    case Value.ValueType.�ط�:
                        cardData.des += "���" + combo.comboValue.value + "������";
                        break;
                    case Value.ValueType.��Ѫ:
                        cardData.des += "�ظ�" + combo.comboValue.value + "������";
                        break;
                }
            }
        }
        //������޺��д����������
        if (valueDic.ContainsKey(Value.ValueType.�޺���))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "�޺���";
        }

        if (valueDic.ContainsKey(Value.ValueType.��ɱ�ط�))
        {
            if (cardData.des != "")
            {
                cardData.des += "\n";
            }
            cardData.des += "��������λ�����ȥ����ظ�"+valueDic[Value.ValueType.��ɱ�ط�]+"������";
        }
    }
    private void Start()
    {
        showGo = CardManager.Instance.showCard;
    }

    
}
