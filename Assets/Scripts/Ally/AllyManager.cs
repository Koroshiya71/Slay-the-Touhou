using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    public static AllyManager Instance;
    public List<AllyData> allyDataList;//友方单位数据列表
    public List<Sprite> allySpriteList;//友方单位数据列表

    public GameObject allyPrefab;//友方单位预制体
    public GameObject inGameAllies;//游戏中的友方单位的总父物体
    public List<Ally> inGameAlliesList = new List<Ally>();//游戏中的所有友方单位
    private int createCount = 0;
    private void Awake()
    {
        Instance = this;
    }

    public void CreateAlly(int id)
    {
        GameObject allyGo = Instantiate(allyPrefab,inGameAllies.transform);
        allyGo.transform.localPosition = new Vector3(-850+100*createCount, 75, 0);  
        Ally newAlly = allyGo.GetComponent<Ally>();
        foreach (var data in allyDataList)
        {
            if (data.allyID==id)
            {
                newAlly.InitAlly(data);
                break;
            }
        }
        inGameAlliesList.Add(newAlly);
        createCount++;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CreateAlly(0);
        }
    }
}
