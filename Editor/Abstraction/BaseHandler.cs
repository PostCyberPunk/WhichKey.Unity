using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

public interface HandlerBase
{
    Dictionary<int, string> TypeMap { get; set; }
}
public interface IWKBasicHandlerA : HandlerBase
{
    void ProcessKey(int key);
}
public interface IWKBasicHandlerB : HandlerBase
{
    bool ProcessKey(int key);
}
public interface IWKTypeHandlerA : HandlerBase
{
    void ProcessType(int type);
    void ProcessKey(int key);
}
public interface IWKTypeHandlerB : HandlerBase
{
    void ProcessType(int type);
    bool ProcessKey(int key);
}
public interface IWKHintSource
{
    String[] GetHint();
}
public class WKCoreManager
{
    private Dictionary<int, HandlerBase> mHandlers;

    void procesKey(int key, bool shift)
    {

    }
}
public struct WKKey
{
    public int key;
    public bool shift;
    public WKKey(int key, bool shift)
    {
        this.key = key;
        this.shift = shift;
    }
}