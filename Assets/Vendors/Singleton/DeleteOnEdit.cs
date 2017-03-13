using UnityEngine;
using System.Collections;

namespace MiupGames.Common.Singleton {

    [ExecuteInEditMode]
    public class DeleteOnEdit : MonoBehaviour {
        #if UNITY_EDITOR

        //Keep a reference to avoid an exception.
        private string gameObjectName;

        void Start() {
            gameObjectName = name;
        }

        void Update() {
            if (!Application.isPlaying) {
                Debug.LogWarning("GameObject still alive, killing: " + gameObjectName);
                DestroyImmediate(gameObject);
            }
        }
        #endif
    }
}
