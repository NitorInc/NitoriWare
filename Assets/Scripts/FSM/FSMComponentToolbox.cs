using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace StageFSM
{
    public class FSMComponentToolbox : MonoBehaviour
    {
        [SerializeField]
        private Component[] tools;
        public Component[] Tools => tools;

        public T GetTool<T>() where T : Component
        {
            return tools
                .FirstOrDefault(a => a.GetType() == typeof(T) || a.GetType().IsSubclassOf(typeof(T))) as T;
        }

        public T[] GetTools<T>() where T : Component
        {
            return tools
                .Where(a => a.GetType() == typeof(T) || a.GetType().IsSubclassOf(typeof(T)))
                .ToArray() as T[];
        }
    }
}
