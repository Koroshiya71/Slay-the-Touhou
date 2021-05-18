using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    public static AllyManager Instance;
    public List<AllyData> allyDataList;//�ѷ���λ�����б�
    public List<Sprite> allySpriteList;//�ѷ���λ�����б�

    public GameObject allyPrefab;//�ѷ���λԤ����
    public GameObject inGameAllies;//��Ϸ�е��ѷ���λ���ܸ�����
    public List<Ally> inGameAlliesList = new List<Ally>();//��Ϸ�е������ѷ���λ
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
