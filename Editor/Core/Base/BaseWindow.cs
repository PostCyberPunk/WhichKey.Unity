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
			WhichKeyManager.instance.OverrideWindowTimeout = instance.OverriderTimeout;
			instance.minSize = new(0, 0);
			instance.position = new Rect(0, 0, 0, 0);

			instance.OnActive();

			instance.ShowPopup();

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
			KeyHandler(e);
		}
		private void KeyHandler(Event e)
		{
			if (e.type == EventType.KeyUp)
			{
				keyReleased = true;
				return;
			}
			if (e.type == EventType.KeyDown)
			{
				e.Use();
				switch (e.keyCode)
				{
					case KeyCode.None:
						break;
					case KeyCode.Escape:
						Close();
						break;
					case KeyCode.LeftShift:
					case KeyCode.RightShift:
						break;
					default:
						if (e.keyCode != prevKey || keyReleased)
						{
							prevKey = e.keyCode;
							keyReleased = false;
							if (WhichKeyManager.instance.Input(e.keyCode, e.shift))
							{
								Close();
							}
							else
							{
								_changeUI = true;
								UpdateDelayTimer();
							}
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
		protected virtual void OnDisable()
		{
			Close();
		}
		private void OnLostFocus() => Close();
		public void OverriderTimeout(float timeout) => timeoutLen = timeout;


	}

}

