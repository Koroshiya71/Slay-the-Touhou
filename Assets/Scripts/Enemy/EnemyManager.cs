using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;//����
    public List<EnemyData> enemyDataList=new List<EnemyData>();//���������б�
    public List<Enemy> InGameEnemyList = new List<Enemy>();//ս���еĵ����б�
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