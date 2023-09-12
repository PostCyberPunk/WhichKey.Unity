using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Core;

namespace PCP.WhichKey.Types
{
	public abstract class WkBaseWindow : EditorWindow
	{
		//FIXME
		protected float DefaultTimeoutLen => WhichKeyPreferences.instance.Timeout;
		private bool keyReleased = true;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;
		private bool _changeUI;
		private bool needClose;
		protected float mWidth;
		protected float mHeight;
		private float timeoutLen;
		protected int mDepth = -1;
		public virtual void OnActive()
		{
			timeoutLen = DefaultTimeoutLen;
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
				if (mDepth < 0)
					KeyHandler(e);
				else if (mDepth == 0)
					needClose = true;
				else
					mDepth--;
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
	}
}