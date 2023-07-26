/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using UnityEngine;

namespace Runtime.Objects
{
    // Spawns objects based on the distance travelled by the player
    public sealed class FloorSpawner : ObjectSpawner
    {
	    private GameObject _lastSpawned;
		private float _nextSpawnDistance;

		private void Update() 
		{
			//Initial check
			if (_lastSpawned == null)
			{
				FloorSpawn(transform.position);	
				return;
			}
			
			if(gameObject.transform.position.x - _lastSpawned.transform.position.x >= _nextSpawnDistance)
			{
				FloorSpawn(transform.position);
			}
		}

	    private void FloorSpawn(Vector3 spawnPosition)
		{
			_lastSpawned = Spawn(spawnPosition);

			if (_lastSpawned != null)
			{
				_lastSpawned.transform.position = transform.position;
			}
            else
            {
	            return;
            }
			
			var size = _lastSpawned.GetComponent<Collider2D>().bounds.center;
			_nextSpawnDistance = size.x /2;
		}
    }
}
