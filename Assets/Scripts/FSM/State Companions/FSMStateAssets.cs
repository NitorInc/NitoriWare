using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace StageFSM
{
    public class FSMStateAssets : MonoBehaviour
    {
        [SerializeField]
        private StateAssetGroup[] stateAssets;
        public StateAssetGroup[] StateAssets => stateAssets;

        [System.Serializable]
        public class StateAssetGroup
        {
            [SerializeField]
            private string stateName;
            public string StateName => stateName;
            [SerializeField]
            private Object[] objects;
            public Object[] Objects => objects;

            public T GetAsset<T>() where T : Object
            {
                return objects
                    .FirstOrDefault(a => a.GetType() == typeof(T) || a.GetType().IsSubclassOf(typeof(T))) as T;
            }

            public T[] GetAssets<T>() where T : Object
            {
                return objects
                    .Where(a => a.GetType() == typeof(T) || a.GetType().IsSubclassOf(typeof(T)))
                    .ToArray() as T[];
            }
        }

        public StateAssetGroup GetAssetGroupForState(string stateName)
        {
            return stateAssets
                .FirstOrDefault(a => a.StateName.Equals(stateName));
        }
    }
}