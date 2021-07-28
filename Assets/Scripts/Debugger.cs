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
        style.normal.textColor = new Color(200 / 255f, 180 / 255f, 150 / 255f); // 需要除以255，因为范围是0-1
        GUI.Box(new Rect(50, 350, 200, 30), "按键A-P分别获取0-15号遗物");    
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
