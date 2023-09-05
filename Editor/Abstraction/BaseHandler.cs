using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using PCP.Tools.WhichKey;
using UnityEditor;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
}
public interface IWKBasicHandler
{
    int TypeID { get; set; }
    int TypeName { get; set; }
    void ProcessKey(int key, bool shift);
}
public interface IWKBasicHandlerA
{
    int TypeID { get; set; }
    int TypeName { get; set; }
    bool ProcessKey(int key, bool shift);
}
public interface IWKTypeHandler
{
    Dictionary<int, string> TypeMap { get; set; }
    void ProcessType(int type);
    void ProcessKey(int key, bool shift);
}
public interface IWKTypeHandlerB
{
    Dictionary<int, string> TypeMap { get; set; }
    void ProcessType(int type);
    bool ProcessKey(int key, bool shift);
}
public interface IWKHintSource
{
    String[] GetHint();
}
public interface IWKCustomWindow
{
    WKHintsWindow win { get; set; }
    bool ProcessKey(int key, bool shift);
}
public abstract class WKHintsWindow : EditorWindow
{
}
public interface IWKArgHandler<T> where T : WKArgsBase
{
    void ProcessArg(T arg);
}
[Serializable]
public abstract class WKArgsBase
{
    public string args;
}
public class WKArgsInline : WKArgsBase { }
public class WKArgsFloating<T, U> : WKArgsBase where T : WKArgsWindow<U> where U : WKArgsBase
{
    public void ShowWindow()
    {
        T window = ScriptableObject.CreateInstance<T>();
        window.Active(this as U);
    }
}
public abstract class WKArgsWindow<T> : EditorWindow where T : WKArgsBase
{
    public T args;
    private void OnLostFocus() => Close();

    public void Active(T arg)
    {
        args = arg;
        Init();
        ShowPopup();
    }
    protected abstract void Init();
}
public class TestArgWindow : WKArgsWindow<TestArgs>
{
    protected override void Init()
    {
        Debug.Log(args.mInt);
    }
}
public class TestArgs : WKArgsFloating<TestArgWindow, TestArgs>
{
    public int mInt = 0;
    public TestArgs(int i)
    {
        mInt = i;
    }
}
[InitializeOnLoad]
public static class WKTest
{
    static WKTest()
    {
        var args = new TestArgs(12);
        args.ShowWindow();
    }
}