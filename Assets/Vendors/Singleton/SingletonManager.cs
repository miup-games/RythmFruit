/// --------------------------------------------------------------------
/// <summary>
/// SingletonManager
/// Intention: 	Manage All Singletons Created From this Tool.
/// Author: 	Juan Silva
/// Date: 		Dec 15, 2014
/// Copyright:  (C) 2014 MiupGames Studios Canada Ltd. All rights reserved.
/// </summary>
/// --------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiupGames.Common.Singleton {
	/// <summary>
	/// Manage the instance of a Singleton
	/// </summary>
	public class SingletonManager {

		#region Main Singleton
		/// <summary>
		/// The main game object.
		/// </summary>
		private static GameObject _mainGameObject = null;

		/// <summary>
		/// Gets the main game object.
		/// </summary>
		/// <value>The main game object.</value>
		public static GameObject mainGameObject {
			get {
				if(_mainGameObject == null) {
					_mainGameObject = new GameObject("-Singleton");
					UnityEngine.Object.DontDestroyOnLoad(_mainGameObject);
                    			_mainGameObject.AddComponent<DeleteOnEdit>();
				}
				return _mainGameObject;
			}
		}
		#endregion

        private static List<WeakReference> _singletons = new List<WeakReference>();

		/// <summary>
		/// Creates the game object.
		/// </summary>
		/// <returns>The game object.</returns>
		/// <param name="name">Name.</param>
		public static GameObject CreateGameObject(string name){
			GameObject newGameObject = CreateSingleGameObject(name);
			// Add to Main Object
			newGameObject.transform.parent = mainGameObject.transform;

            _singletons.Add(new WeakReference(newGameObject));

			return newGameObject;
		}

		/// <summary>
		/// Creates the single game object.
		/// </summary>
		/// <returns>The single game object.</returns>
		/// <param name="name">Name.</param>
		public static GameObject CreateSingleGameObject(string name){
			GameObject newGameObject = new GameObject(name);
			UnityEngine.Object.DontDestroyOnLoad(newGameObject);
                        newGameObject.AddComponent<DeleteOnEdit>();
			
			// Add to Main Object
			newGameObject.transform.localScale 		= Vector3.one;
			newGameObject.transform.localPosition 	= Vector3.zero;

            _singletons.Add(new WeakReference(newGameObject));
			
			return newGameObject;
		}

        public static void DestroyAll()
        {
            foreach (var singleton in _singletons.AsEnumerable().Reverse())
            {
                if (singleton.IsAlive && singleton.Target != null)
                {
                    GameObject.Destroy((GameObject) singleton.Target);
                }
            }

            _singletons.Clear();
        }
	}
}
