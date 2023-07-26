/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime.Objects
{
	[Serializable]
	public class SpawnableObject
	{
		public GameObject gameObject;
		public float spawnWeight;
	}
	
    public class ObjectSpawner : MonoBehaviour
    {
	    [FormerlySerializedAs("Spawning")]
		[Space(10)]	
		[Header("Spawning")]
		public bool active = true;
		public float initialDelay;
		
		[Space(10)]
		[Header("Objects Pool")]
		public List<SpawnableObject> objectsToPool;

		public float startTime;
		
		public GameObject Spawn(Vector3 spawnPosition)
		{
			if (Time.time - startTime < initialDelay || !active)
	        {
	        	return null;
	        }

			var totalWeight = 0f;
			foreach (var spawnableObject in objectsToPool)
			{
				totalWeight += spawnableObject.spawnWeight;
			}
			
			var randomWeight = Random.Range(0, totalWeight);
			var currentWeight = 0f;
			SpawnableObject nextGameObject = null;
			foreach (var spawnableObject in objectsToPool)
			{
				currentWeight += spawnableObject.spawnWeight;
				if (randomWeight <= currentWeight)
				{
					nextGameObject = spawnableObject;
					break;
				}
			}
			
			if (nextGameObject == null)
            {
                return null;
            }
			
			nextGameObject.gameObject.transform.position = spawnPosition;
			return Instantiate(nextGameObject.gameObject);
	    }
    }
}
