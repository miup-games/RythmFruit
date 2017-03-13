using System.Collections.Generic;
using MiupGames.Common.Singleton;
using UnityEngine;

public class GameObjectPool : Singleton<GameObjectPool>
{
    Dictionary<GameObject, Stack<GameObject>> _prefabObjectPools = new Dictionary<GameObject, Stack<GameObject>>();
    Dictionary<GameObject, GameObject> prefabsFromObjects = new Dictionary<GameObject, GameObject>();
    Dictionary<string, GameObject> _prefabsFromNames = new Dictionary<string, GameObject>();

    private GameObject PositionAndReturnObject(GameObject newObject, Vector3 position, Quaternion rotation, Transform parent)
    {
        newObject.SetActive(true);
        Transform objectTransform = newObject.transform;
        objectTransform.position = position;
        objectTransform.rotation = rotation;
        objectTransform.parent = parent;

        return newObject;
    }

    private GameObject GetGameObjectFromName(string name)
    {
        return Resources.Load(name) as GameObject;
    }

    private GameObject InstantiateFromPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity, parent) as GameObject;
        this.prefabsFromObjects[newObject] = prefab;
        return newObject;
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Transform parent)
    {
        Stack<GameObject> pool;

        if (this._prefabObjectPools.TryGetValue(prefab, out pool))
        {
            if (pool.Count > 0)
            {
                return this.PositionAndReturnObject(pool.Pop(), position, Quaternion.identity, parent);
            }
        }
        else
        {
            this._prefabObjectPools[prefab] = new Stack<GameObject>();
        }

        return this.InstantiateFromPrefab(prefab, position, Quaternion.identity, parent);
    }

    public T GetObject<T>(GameObject prefab, Vector3 position, Transform parent) where T : MonoBehaviour
    {
        return this.GetObject(prefab, position, parent).GetComponent<T>();
    }

    public GameObject GetObject(string prefabName, Vector3 position, Transform parent)
    {
        GameObject prefab;
        if(!this._prefabsFromNames.TryGetValue(prefabName, out prefab))
        {
            this._prefabsFromNames[prefabName] = this.GetGameObjectFromName(name);
        }

        return this.GetObject(prefab, position, parent);
    }

    public T GetObject<T>(string prefabName, Vector3 position, Transform parent) where T : MonoBehaviour
    {
        return this.GetObject(prefabName, position, parent).GetComponent<T>();
    }

    public void ReturnObject(GameObject returnedObject)
    {
        GameObject prefab;
        if (this.prefabsFromObjects.TryGetValue(returnedObject, out prefab))
        {
            this._prefabObjectPools[prefab].Push(returnedObject);
        }

        this.HideReturnedObject(returnedObject);
    }

    private void HideReturnedObject(GameObject returnedObject)
    {
        returnedObject.SetActive(false);
        returnedObject.transform.parent = this.transform;
    }
}