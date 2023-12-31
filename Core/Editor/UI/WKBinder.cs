using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using PCP.WhichKey.Types;
using PCP.WhichKey.UI;

namespace PCP.WhichKey.Core.UI
{
	[CustomPropertyDrawer(typeof(WkKeySeq))]
	public class WkBinder : PropertyDrawer
	{
		private int mDepth = 1;
		private string mTitle = "WhichKey Binding";

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = UILoader.instance.WkBinder.Instantiate(property.propertyPath);
			var btn = root.Q<Button>("Bind");
			btn.clickable = new Clickable(() =>
			{
				if (root.parent.userData != null)
				{
					var setting = (WkBinderSetting)root.parent.userData;
					mDepth = setting.Depth;
					mTitle = setting.Title;
				}
				BindingWindow.ShowWindow((ks) =>
				{

					WkKeySeq wkKey = ks;
					var array = property.FindPropertyRelative("_keySeq");
					array.arraySize = wkKey.KeySeq.Length;
					for (int i = 0; i < wkKey.KeySeq.Length; i++)
					{
						array.GetArrayElementAtIndex(i).intValue = wkKey.KeySeq[i];
					}

					property.FindPropertyRelative("_keyLabel").stringValue = wkKey.KeyLabel;
					property.serializedObject.ApplyModifiedProperties();
				}, mDepth, mTitle);
			});
			return root;
		}
	}
}

namespace PCP.WhichKey.UI
{
	internal class WkBinderSetting
	{
		public int Depth;
		public string Title;

		/// <summary>
		/// Create a which key binder darower for WkKeySeq
		/// </summary>
		/// <param name="depth">Max count for your key sequence</param>
		public WkBinderSetting(int depth)
		{
			Depth = depth;
			Title = "WhichKey Binding";
		}

		/// <summary>
		/// Create a which key binder darower for WkKeySeq
		/// </summary>
		/// <param name="depth">Max count for your key sequence</param>
		/// <param name="title">Tilte for biding window</param>
		public WkBinderSetting(int depth, string title)
		{
			Depth = depth;
			Title = title;
		}
	}
	public static class WkBinderUtil
	{
		/// <summary>
		/// Use a visual tree asset to create a which key binder darower for WkKeySeq,amke sure your property field name is  "WkBinder"
		/// </summary>
		/// <param name="depth">Max keys count for binding</param>
		/// <param name="vts">VisualTreeAsset that contians a Propery field for wkkeyseq</param>
		/// <returns></returns> 
		public static VisualElement SetBinder(int depth, VisualTreeAsset vts)
		{
			var e = vts.CloneTree();
			e.Q<PropertyField>("WkBinder").userData = new WkBinderSetting(depth);
			return e;
		}
		/// <summary>
		/// Use a visual tree asset to create a which key binder darower for WkKeySeq,amke sure your property field name is  "WkBinder"
		/// </summary>
		/// <param name="depth">Max keys count for binding</param>
		/// <param name="vts">VisualTreeAsset that contians a Propery field for wkkeyseq</param>
		/// <param name="title">Tilte for biding window</param>
		/// <returns></returns> 
		public static VisualElement SetBinder(int depth, string title, VisualTreeAsset vts)
		{
			var e = vts.CloneTree();
			e.Q<PropertyField>("WkBinder").userData = new WkBinderSetting(depth, title);
			return e;
		}
	}
}