using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using System.Linq;
using System.Text;
using PCP.Tools.WhichKey;
public class SceneHandler : ScriptableObject
{
    private SceneData? sceneData;
    public void Init()
    {
        if (WhichkeyProjectSettings.instance == null)
        {
            WhichKeyManager.LogError("Cant find project settings");
            return;
        }
        sceneData = WhichkeyProjectSettings.instance.CurrentSceneData;
    }
}
