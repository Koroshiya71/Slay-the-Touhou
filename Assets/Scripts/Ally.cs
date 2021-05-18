using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ally : MonoBehaviour
{
    public AllyData allyData;//该友方单位的数据
    public Image allyImg;//该单位的外观图片
    public Text allyInfoText;//说明该单位信息的文本
    public int currentHp;//友方单位当前的生命值
    public Text curHpText;//当前生命值文本
    void Start()
    {
        allyImg = GetComponent<Image>();
    }

    public void InitAlly(AllyData data) //初始化
    {
        allyData = data;
        if(allyImg!=null)
            allyImg.sprite = AllyManager.Instance.allySpriteList[data.allyID];
        currentHp = data.allyHp;
    }

    public void OnTurnEnd()//当前友方单位回合结束时的行动
    {
        switch (allyData.allyID)
        {
            case 0://上海
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
