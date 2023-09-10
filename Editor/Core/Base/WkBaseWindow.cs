using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PCP.Tools.WhichKey
{

	public abstract class WkBaseWindow : EditorWindow
	{
		// public static T instance;
		private bool keyReleased = true;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;
		private bool _changeUI;
		private float timeoutLen;
		protected float mWidth;
		protected float mHeight;
		private bool needClose;
		public static void Active()
		{
			// if (instance == null)
			// {
			// 	instance = ScriptableObject.CreateInstance<T>();
			// }
			// instance.UpdateDelayTimer();


			// instance.OnActive();

			// instance.ShowPopup();
			// instance.minSize = new(0, 0);
			// instance.position = new Rect(0, 0, 0, 0);

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
			if (!needClose)
				KeyHandler(e);
			else if (e.type == EventType.KeyUp)
			{
				e.Use();
				base.Close();
				return;
			}
			else if (showHint && !_changeUI)
			{
				_changeUI = true;
				showHint = false;
			}
			e.Use();
		}
		private void OnLostFocus() => base.Close();
		protected virtual void Update()
		{
			CheckDelayTimer();
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
				switch (e.keyCode)
				{
					case KeyCode.None:
						break;
					case KeyCode.Escape:
						base.Close();
						break;
					case KeyCode.LeftShift:
					case KeyCode.RightShift:
						break;
					default:
						if (e.keyCode != prevKey || keyReleased)
						{
							prevKey = e.keyCode;
							keyReleased = false;
							WhichKeyManager.instance.Input(e.keyCode, e.shift);
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
			if (needClose || showHint) return;
			showHint = Time.realtimeSinceStartup > hideTill;
			if (showHint)
			{
				//OPT cant get mouse position here,need to find a way to get mouse position
				_changeUI = true;
				Repaint();
			}
		}
		public new void Close() => needClose = true;
		public void ForceClose() => base.Close();
		public void OverriderTimeout(float timeout) => timeoutLen = timeout;
	}

}

