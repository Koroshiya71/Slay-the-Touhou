using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ally : MonoBehaviour
{
    public AllyData allyData;//���ѷ���λ������
    public Image allyImg;//�õ�λ�����ͼƬ
    public Text allyInfoText;//˵���õ�λ��Ϣ���ı�
    public int currentHp;//�ѷ���λ��ǰ������ֵ
    public Text curHpText;//��ǰ����ֵ�ı�
    void Start()
    {
        allyImg = GetComponent<Image>();
    }

    public void InitAlly(AllyData data) //��ʼ��
    {
        allyData = data;
        if(allyImg!=null)
            allyImg.sprite = AllyManager.Instance.allySpriteList[data.allyID];
        currentHp = data.allyHp;
    }

    public void OnTurnEnd()//��ǰ�ѷ���λ�غϽ���ʱ���ж�
    {
        switch (allyData.allyID)
        {
            case 0://�Ϻ�
                if (EnemyManager.Instance.InGameEnemyList.Count>0)
                {
                    int minHp = EnemyManager.Instance.InGameEnemyList[0].hp;
                    int index = 0;
                    for (int i = 1; i < EnemyManager.Instance.InGameEnemyList.Count; i++)
                    {

                        if (minHp < EnemyManager.Instance.InGameEnemyList[i].hp)
                        {
                            index = i;
                        }
                    }
                    EnemyManager.Instance.InGameEnemyList[index].TakeDamage(5);
                }
                
                break;
        }
    }
    void Update()
    {
        curHpText.text = currentHp.ToString();
    }
}
