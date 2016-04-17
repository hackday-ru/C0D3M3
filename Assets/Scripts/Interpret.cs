using System;
using UnityEngine;
using UnityEngine.Networking;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;


public static class Interpret
{
    private static ScriptEngine engine;
    private static ScriptScope scope;
    private static ScriptSource source;

    
    public static void Init(MonoBehaviour self) {
        if (engine == null || scope == null) {
            engine = Python.CreateEngine();
            scope = engine.CreateScope();

            scope.SetVariable("self", self);
            PythonDictionary mem = new PythonDictionary();
            scope.SetVariable("mem", mem);
            scope.SetVariable("log", new Func<object, object>((x) => { Debug.Log(x); return null; }));
        }
    }

    public static void ExecInPython(this MonoBehaviour self, string code)
    {
        Init(self);

        try {
            source = engine.CreateScriptSourceFromString(code);
            source.Execute(scope);
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }
}