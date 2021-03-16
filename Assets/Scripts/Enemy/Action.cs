using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public ActionData data;//相关基本数据
    public Dictionary<Value.ValueType, int> valueDic=new Dictionary<Value.ValueType, int>();//效果字典

    void Start()
    {
        
    }

    public void InitAction(ActionData data)
    {
        this.data = data;
        foreach (var value in data.valueList)//初始化字典
        {
            valueDic.Add(value.type, value.value);
        }
    }
    void Update()
    {
        
    }
}
