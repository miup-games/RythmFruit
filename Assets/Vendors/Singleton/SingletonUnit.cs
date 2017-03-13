/// --------------------------------------------------------------------
/// <summary>
/// SingletonUnit
/// Intention:  Single GameObject as a Singleton.
/// Author:     Juan Silva
/// Date:       Dec 15, 2014
/// Copyright:  (C) 2014 MiupGames Studios Canada Ltd. All rights reserved.
/// </summary>
/// --------------------------------------------------------------------
using System.Diagnostics;
using UnityEngine;

namespace MiupGames.Common.Singleton
{
    /// <summary>
    /// Create a New Gameobject and attach the given component.
    /// This object will not be destroyed on Load level.
    /// Usefull when the Singleton handles heavy objects during the entire game.
    /// </summary>
    public class SingletonUnit<T> : MonoBehaviour where T: class
    {
        // Instance
        private static T _instance = null;

        /// <summary>
        /// Gets the unique instance of this singleton.
        /// </summary>
        /// <value>The instance.</value>
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    Instantiate();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Determines if the unique instance is instantiated.
        /// </summary>
        /// <returns><c>true</c> if is instantiated; otherwise, <c>false</c>.</returns>
        public static bool IsInstantiated()
        {
            return _instance != null;
        }

        /// <summary>
        /// Force to instantiate.
        /// </summary>
        public static void Instantiate()
        {
            if (IsInstantiated())
            {
                Trace.WriteLine("Can't instantiate a SingletonUnit of type SingletonUnit<" + typeof(T) + ">: an instance already exists!", "Singleton:w");
                return;
            }

            Trace.WriteLine("Instantiating SingletonUnit of type SingletonUnit<" + typeof(T) + ">", "Singleton:i");
            _instance = SingletonManager.CreateSingleGameObject("-" + typeof(T).Name).AddComponent(typeof(T)) as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
