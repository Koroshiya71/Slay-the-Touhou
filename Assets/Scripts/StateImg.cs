using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateImg : MonoBehaviour
{
    private int index; //在图标列表的下标
    private Text stateExplanationText;
    public bool isEnemy; //是否是敌人的状态
    private Enemy thisEnemy;//管理这个状态图标的敌人对象
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

    public void OnStateImgPointerEnter() //当鼠标放到状态图标上的回调方法
    {
        stateExplanationText.enabled = true;
        stateExplanationText.text = "";
        stateExplanationText.transform.position = transform.position - new Vector3(0f, 0.5f, 0.0f);
        if (!isEnemy)
            switch (Player.Instance.stateList[index].type)
            {
                case Value.ValueType.二刀流:
                    stateExplanationText.text += "体术牌的伤害减半但将释放两次";
                    break;
                case Value.ValueType.流转:
                    stateExplanationText.text += "残心的效果立刻触发";
                    break;
                case Value.ValueType.额外回合:
                    stateExplanationText.text += "回合结束后保留你的手牌和护甲，开始一个新的回合";
                    break;
                case Value.ValueType.体术以外禁止:
                    stateExplanationText.text += "只能使用体术牌";
                    break;
                case Value.ValueType.体术限制:
                    stateExplanationText.text += "无法使用体术牌";
                    break;
                case Value.ValueType.弹幕限制:
                    stateExplanationText.text += "无法使用弹幕牌";
                    break;
                case Value.ValueType.技能限制:
                    stateExplanationText.text += "无法使用技能牌";
                    break;
                case Value.ValueType.法术限制:
                    stateExplanationText.text += "无法使用法术牌";
                    break;
                case Value.ValueType.防御限制:
                    stateExplanationText.text += "无法使用防御牌";
                    break;
                case Value.ValueType.惊吓:
                    stateExplanationText.text += "造成的伤害减少30%";
                    break;
                case Value.ValueType.背水一战:
                    stateExplanationText.text += "无法获得护甲";
                    break;
                case Value.ValueType.起势:
                    stateExplanationText.text += "连斩效果触发两次";
                    break;
                case Value.ValueType.保留手牌:
                    stateExplanationText.text += "回合结束时不丢弃手牌";
                    break;
                case Value.ValueType.六根清净:
                    stateExplanationText.text += "所有卡牌获得无何有，抽牌堆为0时，抽牌改为随机获得一张妖梦牌";
                    break;
                case Value.ValueType.增幅:
                    stateExplanationText.text += "造成的伤害增加30%";
                    break;
                case Value.ValueType.重伤:
                    stateExplanationText.text += "获取的护甲减少30%";
                    break;
            }
        else
            switch (thisEnemy.stateList[index].type)
            {
                
                case Value.ValueType.惊吓:
                    stateExplanationText.text += "造成的伤害减少30%";
                    break;
                case Value.ValueType.灵体:
                    stateExplanationText.text += "受到的体术伤害减少30%";
                    break;
                case Value.ValueType.魂体:
                    stateExplanationText.text += "受到的弹幕伤害减少30%";
                    break;
            }
    }

    public void OnStateImgPointerExit() //当鼠标移开状态图标上的回调方法
    {
        stateExplanationText.enabled = false;
    }
}