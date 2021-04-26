using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionImage : MonoBehaviour
{
    private Enemy thisEnemy;//�������״̬ͼ��ĵ��˶���
    private Text actionExplanationText;

    void Start()
    {
        thisEnemy = GetComponentInParent<Enemy>();
        actionExplanationText = MenuEventManager.Instance.actionExplanationText;
    }

    public void OnActionImgPointerEnter() //�����ŵ��ж�ͼ���ϵĻص�����
    {
        actionExplanationText.enabled = true;
        actionExplanationText.text = "";
        actionExplanationText.transform.position = transform.position - new Vector3(0f, -0.5f, 0.0f);

        switch (thisEnemy.currentEnemyAction.data.Type)
        {
            case ActionController.ActionType.Attack:
                actionExplanationText.text = "�õ��˽����������" + thisEnemy.actualValue + "���˺�";
                break;
            case ActionController.ActionType.Defend:
                actionExplanationText.text = "�õ��˽����" + thisEnemy.actualValue + "�㻤��";
                if (thisEnemy.enemyData.ID==6)//������Ⱥ�����
                {
                    actionExplanationText.text = "�õ��˽�ʹ���е��˻��" + thisEnemy.actualValue + "�㻤��";
                }
                break;
            case ActionController.ActionType.Buff:
                actionExplanationText.text = "�õ��˽���û������������ǿ��Ч��" ;
                break;
            case ActionController.ActionType.DeBuff:
                actionExplanationText.text = "�õ��˽������ʩ�Ӹ���Ч��";
                break;
            case ActionController.ActionType.Special:
                actionExplanationText.text = "�õ��˽�����������ж�";
                break;
            case ActionController.ActionType.Unknown:
                actionExplanationText.text = "�õ��˵��ж�δ֪";
                break;
        }
    }

    public void OnActionImgPointerExit() //������뿪�ж�ͼ���ϵĻص�����
    {
        actionExplanationText.enabled = false;
    }
    void Update()
    {
        
    }
}
