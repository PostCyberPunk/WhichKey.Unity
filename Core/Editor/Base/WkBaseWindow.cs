using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Core;

namespace PCP.WhichKey.Types
{
	public abstract class WkBaseWindow : EditorWindow
	{
		//FIXME
		private static WhichKeyPreferences pref => WhichKeyPreferences.instance;
		private bool keyReleased = true;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;
		private bool _changeUI;
		private bool needClose;
		protected float DefaultTimeoutLen => pref.Timeout;
		protected float WinWidth;
		protected float WinHeight;
		protected float TimeoutLen;
		public virtual void OnActive()
		{
			TimeoutLen = DefaultTimeoutLen;
		}

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
			{
				KeyHandler(e);
			}
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
							WhichKeyManager.instance.ProcesRawKey(e.keyCode, e.shift);
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

		public void UpdateHints()
		{
			_changeUI = true;
			UpdateDelayTimer();
		}

		public void UpdateDelayTimer()
		{
			if (!showHint)
				hideTill = Time.realtimeSinceStartup + TimeoutLen;
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
		protected void SetWindowPosition()
		{
			if (pref.WindowFollowMouse)
			{
				Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				position = new Rect(mousePos.x - WinWidth / 2, mousePos.y - WinHeight / 2, WinWidth, WinHeight);
			}
			else
			{
				position = new Rect(pref.FixedPosition.x, pref.FixedPosition.y, WinWidth, WinHeight);
			}
		}

		private void OnLostFocus() => base.Close();
		public new void Close() => needClose = true;
		public void ForceClose() => base.Close();
	}
}