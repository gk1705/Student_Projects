using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnBehaviour : ScriptableObject
{
	public abstract void InvokeBehaviour(GameObject go, int objectId);
}
