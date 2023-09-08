using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PCP.Tools.WhichKey
{

	public abstract class BaseWKWindow<T> : EditorWindow where T : BaseWKWindow<T>
	{
		public static T instance;
		private bool keyReleased = true;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint = false;
		private bool _changeUI;
		private float timeoutLen;
		protected float mWidth;
		protected float mHeight;
		public static void Active()
		{
			if (instance == null)
			{
				instance = ScriptableObject.CreateInstance<T>();
			}
			instance.UpdateDelayTimer();


			instance.OnActive();

			instance.ShowPopup();
			instance.minSize = new(0, 0);
			instance.position = new Rect(0, 0, 0, 0);

		}
		protected abstract void OnActive();
		protected virtual void DummyWindow()
		{
			position = new Rect(0, 0, 0, 0);
		}
		protected abstract void ShowHints();
		protected virtual void OnGUI()
		{
			Event e = Event.current;
			if (e == null) return;
			CheckUI();
			if (!e.isKey)
				return;
			KeyCode keyCode = e.keyCode;
			bool shift = e.shift;
			bool isUp = e.type == EventType.KeyUp;
			e.Use();
			KeyHandler(keyCode, shift, isUp);
		}
		private void KeyHandler(KeyCode keyCode, bool shift, bool isUp)
		{
			if (isUp)
			{
				keyReleased = true;
				return;
			}
			else
			{
				switch (keyCode)
				{
					case KeyCode.None:
						break;
					case KeyCode.Escape:
						Close();
						break;
					case KeyCode.LeftShift:
					case KeyCode.RightShift:
					case KeyCode.LeftControl:
					case KeyCode.RightControl:
					case KeyCode.LeftAlt:
					case KeyCode.RightAlt:
					case KeyCode.LeftCommand:
					case KeyCode.RightCommand:
					case KeyCode.LeftWindows:
					case KeyCode.RightWindows:
						break;
					default:
						if (keyCode != prevKey || keyReleased)
						{
							prevKey = keyCode;
							keyReleased = false;
							WhichKeyManager.instance.Input(keyCode, shift);
						}
						break;
				}
			}
		}
		private void CheckUI()
		{
			if (_changeUI)
			{
				_changeUI = false;
				if (showHint)
					ShowHints();
				else
					DummyWindow();
			}
		}
		protected virtual void Update()
		{
			CheckDelayTimer();
		}
		protected void UpdateHints()
		{
			_changeUI = true;
			UpdateDelayTimer();
		}
		private void UpdateDelayTimer()
		{
			if (!showHint)
				hideTill = Time.realtimeSinceStartup + timeoutLen;
		}
		private void CheckDelayTimer()
		{
			if (showHint) return;
			showHint = Time.realtimeSinceStartup > hideTill;
			if (showHint)
			{
				//OPT cant get mouse position here,need to find a way to get mouse position
				_changeUI = true;
				Repaint();
			}
		}
		// protected virtual void OnDisable()
		// {
		// 	Close();
		// }
		private void OnLostFocus() => Close();
		public void OverriderTimeout(float timeout) => timeoutLen = timeout;


	}

}

