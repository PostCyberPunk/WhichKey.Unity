using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using PCP.WhichKey.Log;

namespace PCP.WhichKey.Extra
{
	[FilePath("ProjectSettings/Whichkey/ExtraData", FilePathAttribute.Location.ProjectFolder)]
	public class WkExtraManager : ScriptableSingleton<WkExtraManager>
	{
		//Extra
		#region Manager
		public VisualTreeAsset SceneNav { private set; get; }
		public VisualTreeAsset NavSet { private set; get; }
		public float WinTimeout;

		[InitializeOnLoadMethod]
		public static void Init()
		{
			EditorSceneManager.sceneOpened += instance.OnSceneOpened;
			var c_scene = SceneManager.GetActiveScene();
			instance.SetSceneData(c_scene);

			if (SessionState.GetBool("WhichKeyExtraOnce", false))
				RefreshUI();
			else
			{
				SessionState.SetBool("WhichKeyExtraOnce", true);
				EditorApplication.update += RunOnce;
			}
		}
		public static void Save()
		{
			Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey Extra Data");
			instance.Save(true);
		}
		internal SerializedObject GetSerializedObject()
		{
			return new SerializedObject(this);
		}
		private void OnEnable()
		{
			hideFlags &= ~HideFlags.NotEditable;
		}
		public static void RunOnce()
		{
			RefreshUI();
			EditorApplication.update -= RunOnce;
		}
		public static void RefreshUI()
		{
			instance.SceneNav = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/SceneNav");
			instance.NavSet = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/NavSet");
		}
		#endregion

		#region SceneNav
		[SerializeField] private List<SceneNavData> savedSceneDatas = new();
		public SceneNavData CurrentSceneData;
		public void SaveSceneData()
		{
			CurrentSceneData?.SetupKeyHints();
			Save(true);
		}
		private void OnSceneOpened(Scene scene, OpenSceneMode mode)
		{
			SetSceneData(scene);
		}
		private void SetSceneData(Scene scene)
		{
			//FIXME:new scene not working
			if (!scene.IsValid())
				// WkLogger.LogError($"WhichKey: SetCurrentSceneData: Invalid Scene");
				return;
			else if (scene.path == "")
				WkLogger.LogInfo($"WhichKey:Save and reopen the scene to use WhichKey");
			else if (!FindScenedata(scene))
			{
				CurrentSceneData = new SceneNavData(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path));
				savedSceneDatas.Add(CurrentSceneData);
				Save();
			}
		}
		private bool FindScenedata(Scene scene)
		{
			SceneAsset c_scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
			CurrentSceneData = savedSceneDatas.Find(x => x.Scene == c_scene);
			return CurrentSceneData != null;
		}

		#endregion

		#region AssetNav
		public AssetsNavData[] NavAssetsDatas = new AssetsNavData[0];

		#endregion
	}
}
