using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static void AddStateToPlayer(Value state)//�������Ӷ�Ӧ���ͣ�������״̬
    {
        foreach (var s in Player.Instance.stateList)//����������е�״̬�б�
        {
            if (s.type==state.type)//����������״̬����ô�������Ĳ���
            {
                s.value += state.value;
                return;
            }
        }
        //����ͽ����״̬�����б�
        Value newValue = new Value();
        newValue.type = state.type;
        newValue.value = state.value;
        Player.Instance.stateList.Add(newValue);
    }

    public static void CleanState() //���״̬�б��е���Ч״̬
    {
        List<Value> emptyList = new List<Value>();
        foreach (var state in Player.Instance.stateList)
        {
            if (state.value==0)
            {
                emptyList.Add(state);
            }
        }

        foreach (var emptyState in emptyList)
        {
            Player.Instance.stateList.Remove(emptyState);
        }
    }
}
