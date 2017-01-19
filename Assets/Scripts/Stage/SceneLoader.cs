using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SceneLoader : MonoBehaviour
{

	//abandon hope for this script

	public bool loadResourcesAsync;

	private List<MicrogameLoadOperation> operationQueue;

	private class MicrogameLoadOperation
	{
		public StageController.Microgame microgame;
		public List<QueuedResource> resourceQueue;
		public List<ResourceRequest> requests;
		public bool finished;

		public MicrogameLoadOperation(StageController.Microgame microgame)
		{
			this.microgame = microgame;
			resourceQueue = new List<QueuedResource>();
			requests = new List<ResourceRequest>();
			finished = false;
		}
	}

	private enum ResourceType
	{
		None,
		Texture2D
	}

	private struct QueuedResource
	{
		public string path;
		public ResourceType type;

		public QueuedResource(string path, ResourceType type)
		{
			this.path = path;
			this.type = type;
		}
	}

	private System.DateTime lastTime;
	private bool doneQueueing;

	void Awake()
	{
		operationQueue = new List<MicrogameLoadOperation>();
		Application.backgroundLoadingPriority = ThreadPriority.Low;
	}

	public void queueMicrogame(StageController.Microgame microgame)
	{
		MicrogameLoadOperation newOperation = new MicrogameLoadOperation(microgame);
		operationQueue.Add(newOperation);

		for (int i = 0; i < operationQueue.Count - 1; i++)
		{
			if (!operationQueue[i].finished)
				return;
		}

		if (loadResourcesAsync)
			StartCoroutine(loadScene(newOperation));
		else
			StartCoroutine(loadMicrogameAsync(newOperation));

	}

	public void removeMicrogame(StageController.Microgame microgame)
	{
		for (int i = 0; i < operationQueue.Count; i++)
		{
			if (operationQueue[i].microgame.name == microgame.name && operationQueue[i].finished)
			{
				operationQueue.RemoveAt(i);
				return;
			}
		}
	}

	IEnumerator loadScene(MicrogameLoadOperation operation)
	{

		string resourcePath = "", path = "";
			//Application.dataPath + "/Resources/",
			//path = "Microgames/" + (operation.microgame.unfinished ? "" : "_Finished/") + operation.microgame.name + "/";///Resources/";
																														 ///
		//int offset = ("Assets/Resources/").Length;
		//path = path.Substring(offset, path.Length - offset);

		lastTime = System.DateTime.Now;
		doneQueueing = false;
		StartCoroutine(queueResources(operation.resourceQueue, resourcePath, path, true));

		while(!doneQueueing)
		{
			yield return null;
		}

		StartCoroutine(loadAssetsAsync(operation));
	}

	IEnumerator queueResources(List<QueuedResource> queue, string resourcePath, string path, bool isTop)
	{
		Debug.Log("Now we're in " + path);

		if ((System.DateTime.Now - lastTime).Milliseconds >= 2)
		{
			yield return null;
			lastTime = System.DateTime.Now;
		}

		DirectoryInfo levelPath = new DirectoryInfo(resourcePath + path);
		FileInfo[] fileInfos = levelPath.GetFiles("*", SearchOption.TopDirectoryOnly);

		foreach (FileInfo file in fileInfos)
		{
			attemptQueueResource(queue, path, file.Name);
		}

		string[] directories = Directory.GetDirectories(resourcePath + path);

		Debug.Log(directories.Length);

		for (int i = 0; i < directories.Length; i++)
		{
			string[] newPath = directories[i].Split('/');
			queueResources(queue, resourcePath, path + newPath[newPath.Length - 1] + "/", false);
			//fix this later maybe

			
		}

		if (isTop)
		{
			doneQueueing = true;
		}

	}

	void Update()
	{
		//Debug.Log("UPDATE");
	}

	IEnumerator loadAssetsAsync(MicrogameLoadOperation operation)
	{
		Debug.Log(operation.resourceQueue.Count);
		yield return null;

		ResourceRequest request = null;
		for (int i = 0; i < operation.resourceQueue.Count; i++)
		{
			switch (operation.resourceQueue[i].type)
			{
				case (ResourceType.Texture2D):
					request = Resources.LoadAsync<Texture2D>(operation.resourceQueue[i].path);
					break;
				default:
					break;

			}

			while (!request.isDone)
			{
				Debug.Log((lastTime - System.DateTime.Now).Milliseconds);
				yield return null;
			}

			Debug.Log("Done: " + (lastTime - System.DateTime.Now).Milliseconds);

			yield return null;

			operation.requests.Add(request);

		}

		StartCoroutine(loadMicrogameAsync(operation));
		yield return null;

	}
	
	IEnumerator loadMicrogameAsync(MicrogameLoadOperation operation)
	{

		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(operation.microgame.name, LoadSceneMode.Additive);

		while (!asyncOperation.isDone)
		{
			yield return null;
		}

		operation.finished = true;

		
		for (int i = 0; i < operationQueue.Count; i++)
		{
			if (!operationQueue[i].finished)
			{
				yield return null;
				if (loadResourcesAsync)
					StartCoroutine(loadScene(operationQueue[i]));
				else
					StartCoroutine(loadMicrogameAsync(operationQueue[i]));
				i = operationQueue.Count;
			}
		}

		yield return null;
	}

	private bool attemptQueueResource(List<QueuedResource> queue, string path, string name)
	{
		ResourceType type = getFileResourceType(name);

		if (type != ResourceType.None)
		{
			string resourcePath = getResourcePath(path, name);
			queue.Add(new QueuedResource(resourcePath, type));
			return true;
		}
		return false;
	}


	//private IEnumerator loadResourceAsync(string path, )
	//{
		

	//	while (!microgamePool[index].asyncOperation.isDone)
	//	{
	//		Debug.Log(microgamePool[index].asyncOperation.progress);
	//		yield return null;
	//	}
	//	Debug.Log("DONE: " + microgamePool[index].asyncOperation.progress);
	//}

	private string getResourcePath(string path, string name)
	{
		string[] nameSections = name.Split('.');
		name = "";
		for (int i = 0; i < nameSections.Length - 1; i++)
		{
			name += nameSections[i];
		}
		return path + name;
	}


	private ResourceType getFileResourceType(string path)
	{
		string[] names = path.Split('.');
		string extension = names[names.Length - 1];

		if (extension.Equals("png", System.StringComparison.OrdinalIgnoreCase))
		{
			//Debug.Log("SPRITE: " + path);
			return ResourceType.Texture2D;
		}

		return ResourceType.None;

	}




	//IEnumerator loadMicrogameAsync()
	//{
	//	microgamePool[index].asyncOperation = SceneManager.LoadSceneAsync(microgamePool[index].name, LoadSceneMode.Additive);
	//	microgamePool[index].asyncOperation.priority = 999 - index;

	//	while (!microgamePool[index].asyncOperation.isDone)
	//	{
	//		Debug.Log(microgamePool[index].asyncOperation.progress);
	//		yield return null;
	//	}
	//	Debug.Log("DONE: " + microgamePool[index].asyncOperation.progress);
	//}

}
