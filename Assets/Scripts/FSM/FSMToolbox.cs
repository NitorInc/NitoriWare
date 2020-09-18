using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace StageFSM
{
    public class FSMToolbox : MonoBehaviour
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


        private void Awake()
        {
            // TODO remove
            StageController.beatLength = 60f / 130f;
            StageController.beatLength = 0.46f;
            GetComponent<Animator>().speed = 1f / StageController.beatLength;
        }

        private void Update()
        {
            // TODO remove
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetTrigger("Advance");
            }

        }
        
    }
}
