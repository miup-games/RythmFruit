/// --------------------------------------------------------------------
/// <summary>
/// Singleton
/// Intention:  Single GameObject as a Singleton inside Singleton Manager.
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
    /// Common Singleton.
    /// Attach a Component into a new GameObject inside SingletonManager.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T: class
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
                if (!IsInstantiated())
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
                Trace.WriteLine("Can't instantiate a Singleton of type Singleton<" + typeof(T) + ">: an instance already exists!", "Singleton:w");
                return;
            }

            Trace.WriteLine("Instantiating Singleton of type Singleton<" + typeof(T) + ">", "Singleton:i");
            _instance = SingletonManager.CreateGameObject("-" + typeof(T).Name).AddComponent(typeof(T)) as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
