using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;


public interface IEventInfo { }

public class EventInfo<T>:IEventInfo
{
    public Action<T> Action;
}
public class EventInfo : IEventInfo
{
    public Action Action;
}
public class EventManager 
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null) instance = new EventManager();
            return instance;
        }
    }

    public Dictionary<string, IEventInfo> actionDic = new Dictionary<string, IEventInfo>();

    //添加事件
    //无参
    public void AddEventListener(string name, Action action)
    {
        if (actionDic.ContainsKey(name))
        {
         (  actionDic[name] as EventInfo).Action += action;
        }
        else
        {
            actionDic.Add(name, new EventInfo() { Action = action});
        }
    }
    //添加事件
    //无参
    public void AddEventListener<T>(string name, Action<T> action)
    {

        if (actionDic.ContainsKey(name))
        {
            (actionDic[name] as EventInfo<T>).Action += action;
        }
        else
        {
            actionDic.Add(name, new EventInfo<T>() { Action = action });
        }
    }

    //触发事件
    public void TriggerEvent(string name)
    {

        if (actionDic.ContainsKey(name))
        {
            (actionDic[name] as EventInfo).Action?.Invoke();
        }
    }  
    //触发事件
    public void TriggerEvent<T>(string name,T Param)
    {

        if (actionDic.ContainsKey(name))
        {
            (actionDic[name] as EventInfo<T>).Action?.Invoke(Param);
        }
    }
    //移出事件
    public void RemoveEvent(string name, Action action)
    {

        if (actionDic.ContainsKey(name))
        {
            (actionDic[name] as EventInfo).Action -= action;
        }
    }
    //移出事件
    public void RemoveEvent<T>(string name, Action<T> action)
    {

        if (actionDic.ContainsKey(name))
        {
            (actionDic[name] as EventInfo<T>).Action -= action;
        }
    }
    //清空事件
    public void ClearEvent()
    {
        actionDic.Clear();
    }
}
