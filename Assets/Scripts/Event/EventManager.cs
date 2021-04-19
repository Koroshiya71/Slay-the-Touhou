using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<GameEvent> eventList = new List<GameEvent>();//事件列表
    public Canvas eventCanvas;//事件画布
    void Start()
    {
        eventCanvas.enabled = false;
    }

    void Update()
    {
        
    }
}
