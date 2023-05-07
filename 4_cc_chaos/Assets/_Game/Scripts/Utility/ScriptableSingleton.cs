//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Linq;
using UnityEngine;

namespace CaravanCrashChaos
{
	public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		protected static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					var type = typeof(T);
					var instances = Resources.LoadAll<T>(string.Empty);
					_instance = instances.FirstOrDefault();
					if (_instance == null)
					{
						Debug.LogErrorFormat("[ScriptableSingleton] No instance of {0} found!", type.ToString());
					}
					else if (instances.Length > 1)
					{
						Debug.LogErrorFormat("[ScriptableSingleton] Multiple instances of {0} found!", type.ToString());
					}
					else
					{
						Debug.LogFormat("[ScriptableSingleton] An instance of {0} was found!", type.ToString());
					}
				}
				return _instance;
			}
		}
	}
}


