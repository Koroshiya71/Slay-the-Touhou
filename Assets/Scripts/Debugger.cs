using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            fontSize = 20,
        };
        style.normal.textColor = new Color(200 / 255f, 180 / 255f, 150 / 255f); // ��Ҫ����255����Ϊ��Χ��0-1
        GUI.Box(new Rect(50, 350, 200, 30), "����A-P�ֱ��ȡ0-15������");    
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
