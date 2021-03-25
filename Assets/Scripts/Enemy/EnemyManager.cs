using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;//单例
    public List<EnemyData> enemyDataList=new List<EnemyData>();//敌人数据列表
    public List<Enemy> InGameEnemyList = new List<Enemy>();//战斗中的敌人列表
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
