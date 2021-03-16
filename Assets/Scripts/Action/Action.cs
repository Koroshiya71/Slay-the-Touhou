using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public ActionData data;//��ػ�������
    public Dictionary<Value.ValueType, int> valueDic=new Dictionary<Value.ValueType, int>();//Ч���ֵ�

    void Start()
    {
        
    }

    public void InitAction(ActionData data)
    {
        this.data = data;
        foreach (var value in data.valueList)//��ʼ���ֵ�
        {
            valueDic.Add(value.type, value.value);
        }
    }
    void Update()
    {
        
    }
}
