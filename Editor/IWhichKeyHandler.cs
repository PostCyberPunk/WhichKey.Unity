using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWhichKeyHandler
{
	public bool ProcessKey(char key);
	public string[] GetLayerHints();
}
