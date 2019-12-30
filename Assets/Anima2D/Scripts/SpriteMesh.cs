using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	public class SpriteMesh : ScriptableObject
	{
		public const int api_version = 4;

		[SerializeField][HideInInspector]
		private int m_ApiVersion;
		[SerializeField]
		private Sprite m_Sprite;
		[SerializeField]
		private Mesh m_SharedMesh;

		public Sprite sprite
		{
			get { return m_Sprite; }
			private set { m_Sprite = value; }
		}

		public Mesh sharedMesh
		{
			get { return m_SharedMesh; }
			private set { m_SharedMesh = value; }
		}

		//Needed for upgrade 4
		[SerializeField, HideInInspector] Material[] m_SharedMaterials;
		
	}
}
