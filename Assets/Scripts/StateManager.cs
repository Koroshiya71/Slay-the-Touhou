using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    public List<StateImgData> imgList;
    public Dictionary<Value.ValueType, Sprite> stateImgDic=new Dictionary<Value.ValueType, Sprite>();//

    private void Awake()
    {
        Instance = this;
        foreach (var i in imgList)
        {
            stateImgDic.Add(i.type,i.sprite);
        }
    }

    public void InitPlayerState() //初始化玩家状态
    {
        Player.Instance.stateList = new List<Value>();
    }
    public static void AddStateToPlayer(Value state)//给玩家添加对应类型，层数的状态
    {
        foreach (var s in Player.Instance.stateList)//查找玩家现有的状态列表
        {
            if (s.type==state.type)//如果已有这个状态，那么增加它的层数
            {
                s.value += state.value;
                return;
            }
        }
        //否则就将这个状态加入列表
        Value newValue = new Value();
        newValue.type = state.type;
        newValue.value = state.value;
        Player.Instance.stateList.Add(newValue);
        if (newValue.type!=Value.ValueType.额外回合)
        {
            Player.Instance.newStateList.Add(newValue);

        }

    }
    public static void AddStateToEnemy(Value state,Enemy target)//给玩家添加对应类型，层数的状态
    {
        foreach (var s in target.stateList)//查找玩家现有的状态列表
        {
            if (s.type == state.type)//如果已有这个状态，那么增加它的层数
            {
                s.value += state.value;
                return;
            }
        }
        //否则就将这个状态加入列表
        Value newValue = new Value();
        newValue.type = state.type;
        newValue.value = state.value;
        target.stateList.Add(newValue);
        target.newStateList.Add(newValue);

    }
    public static void UpdatePlayerState() //清除状态列表中的无效状态
    {
        List<Value> emptyList = new List<Value>();
        foreach (var state in Player.Instance.stateList)
        {
            if (Player.Instance.newStateList.Contains(state))//如果这个状态是新添加的状态，则先不清除
            {
                continue;
            }
            switch (state.type)
            {
                case Value.ValueType.二刀流:
                case Value.ValueType.额外回合:
                case Value.ValueType.体术以外禁止:
                case Value.ValueType.体术限制:

                case Value.ValueType.技能限制:
                case Value.ValueType.法术限制:
                case Value.ValueType.防御限制:
                case Value.ValueType.弹幕限制:
                case Value.ValueType.惊吓:
                case Value.ValueType.保留手牌:
                case Value.ValueType.重伤:
                case Value.ValueType.抽牌减1:
                    state.value--;
                    break;

            }
        }
        for(int i=0;i<Player.Instance.stateList.Count;i++)
        {
            if (Player.Instance.stateList[i].value==0)
            {
                emptyList.Add(Player.Instance.stateList[i]);
                Player.Instance.stateStackTextList[i].enabled = false;
            }
        }

        foreach (var emptyState in emptyList)
        {
            Player.Instance.stateList.Remove(emptyState);
        }
    }
    public static void UpdateEnemiesState() //清除所有敌人状态列表中的无效状态
    {
        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
        {
            List<Value> emptyList = new List<Value>();
            foreach (var state in enemy.stateList)
            {
                if (enemy.newStateList.Contains(state))//如果这个状态是新添加的状态，则先不清除
                {
                    continue;
                }
                switch (state.type)
                {
                    case Value.ValueType.二刀流:
                    case Value.ValueType.额外回合:
                    case Value.ValueType.体术以外禁止:
                    case Value.ValueType.惊吓:
                    case Value.ValueType.灵体:
                    case Value.ValueType.保留护甲:
                    case Value.ValueType.增幅:
                        state.value--;
                        break;

                }
            }
            for (int i = 0; i < enemy.stateList.Count; i++)
            {
                if (enemy.stateList[i].value == 0)
                {
                    emptyList.Add(Player.Instance.stateList[i]);
                    enemy.stateStackTextList[i].enabled = false;
                }
            }

            foreach (var emptyState in emptyList)
            {
                enemy.stateList.Remove(emptyState);
            }
        }
    }
        
}
