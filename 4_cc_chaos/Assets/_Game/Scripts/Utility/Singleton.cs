//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Singleton Template
	/// Can be used in a threaded environment.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static bool singletonUnloaded = false;
		private static object threadLock = new object();
		private static T instance;

		/// <summary>
		/// Access singleton instance.
		/// If not created, we first try to find an object of the same type.
		/// Otherwise we create a game object and outfit said with the templated monobehaviour.
		/// The singleton then persists throughout the application, albeit it can't be
		/// accessed on the process application shutdown anymore.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (singletonUnloaded)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					                 "' already destroyed. Returning null.");
					return null;
				}

				lock (threadLock)
				{
					if (instance == null)
					{
						instance = (T)FindObjectOfType(typeof(T));

						if (instance == null)
						{
							var singletonObject = new GameObject();
							instance = singletonObject.AddComponent<T>();
							singletonObject.name = typeof(T).ToString() + " (Singleton)";

							DontDestroyOnLoad(singletonObject);
						}
					}

					return instance;
				}
			}
		}

		private void OnApplicationQuit()
		{
			singletonUnloaded = true;
		}


		private void OnDestroy()
		{
			singletonUnloaded = true;
		}
	}
}
