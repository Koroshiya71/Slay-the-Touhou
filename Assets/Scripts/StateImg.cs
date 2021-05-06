using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateImg : MonoBehaviour
{
    private int index; //��ͼ���б���±�
    private Text stateExplanationText;
    public bool isEnemy; //�Ƿ��ǵ��˵�״̬
    private Enemy thisEnemy;//�������״̬ͼ��ĵ��˶���
    private void Start()
    {
        if (!isEnemy)
            index = Player.Instance.stateImageList.IndexOf(GetComponent<Image>());
        else
        {
            thisEnemy = GetComponentInParent<Enemy>();
            index = thisEnemy.stateImageList.IndexOf(GetComponent<Image>());
        }

        stateExplanationText = MenuEventManager.Instance.stateExplanationText;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnStateImgPointerEnter() //�����ŵ�״̬ͼ���ϵĻص�����
    {
        stateExplanationText.enabled = true;
        stateExplanationText.text = "";
        stateExplanationText.transform.position = transform.position - new Vector3(0f, 0.5f, 0.0f);
        if (!isEnemy)
            switch (Player.Instance.stateList[index].type)
            {
                case Value.ValueType.������:
                    stateExplanationText.text += "�����Ƶ��˺����뵫���ͷ�����";
                    break;
                case Value.ValueType.��ת:
                    stateExplanationText.text += "���ĵ�Ч�����̴���";
                    break;
                case Value.ValueType.����غ�:
                    stateExplanationText.text += "�غϽ�������������ƺͻ��ף���ʼһ���µĻغ�";
                    break;
                case Value.ValueType.���������ֹ:
                    stateExplanationText.text += "ֻ��ʹ��������";
                    break;
                case Value.ValueType.��������:
                    stateExplanationText.text += "�޷�ʹ��������";
                    break;
                case Value.ValueType.��Ļ����:
                    stateExplanationText.text += "�޷�ʹ�õ�Ļ��";
                    break;
                case Value.ValueType.��������:
                    stateExplanationText.text += "�޷�ʹ�ü�����";
                    break;
                case Value.ValueType.��������:
                    stateExplanationText.text += "�޷�ʹ�÷�����";
                    break;
                case Value.ValueType.��������:
                    stateExplanationText.text += "�޷�ʹ�÷�����";
                    break;
                case Value.ValueType.����:
                    stateExplanationText.text += "��ɵ��˺�����30%";
                    break;
                case Value.ValueType.��ˮһս:
                    stateExplanationText.text += "�޷���û���";
                    break;
                case Value.ValueType.����:
                    stateExplanationText.text += "��նЧ����������";
                    break;
                case Value.ValueType.��������:
                    stateExplanationText.text += "�غϽ���ʱ����������";
                    break;
                case Value.ValueType.�����徻:
                    stateExplanationText.text += "���п��ƻ���޺��У����ƶ�Ϊ0ʱ�����Ƹ�Ϊ������һ��������";
                    break;
                case Value.ValueType.����:
                    stateExplanationText.text += "��ɵ��˺�����30%";
                    break;
                case Value.ValueType.����:
                    stateExplanationText.text += "��ȡ�Ļ��׼���30%";
                    break;
            }
        else
            switch (thisEnemy.stateList[index].type)
            {
                
                case Value.ValueType.����:
                    stateExplanationText.text += "��ɵ��˺�����30%";
                    break;
                case Value.ValueType.����:
                    stateExplanationText.text += "�ܵ��������˺�����30%";
                    break;
                case Value.ValueType.����:
                    stateExplanationText.text += "�ܵ��ĵ�Ļ�˺�����30%";
                    break;
            }
    }

    public void OnStateImgPointerExit() //������ƿ�״̬ͼ���ϵĻص�����
    {
        stateExplanationText.enabled = false;
    }
}