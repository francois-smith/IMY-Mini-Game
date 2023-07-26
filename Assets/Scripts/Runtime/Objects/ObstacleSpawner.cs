/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using Runtime.Managers;
using UnityEngine;

namespace Runtime.Objects
{
    // Spawns objects based on the distance travelled by the player
    public sealed class ObstacleSpawner : ObjectSpawner
    {
	    public Vector2 spawnDistanceRange = new(250f, 600f);
	    public Vector2 spawnDelayRange = new(1f, 2f);

	    private GameObject _lastSpawned;
	    private float _nextSpawnDistance;
	    private float _lastSpawnDistance;

	    private float _timePassed;
	    private float _nextSpawnDelay;

	    private void Update() 
		{
			if(GameManager.Instance.gameStarted == false) return;
			
			//Initial check
			if (_lastSpawned == null)
			{
				ObstacleSpawn(transform.position);
				return;
			}
			
			//Check if the player has moved far enough to spawn a new obstacle
			if (GameManager.Instance.LevelManager.DistanceTraveled - _lastSpawnDistance >= _nextSpawnDistance)
			{
				_timePassed += Time.deltaTime;
				if(_timePassed >= _nextSpawnDelay) ObstacleSpawn(transform.position);
			}
	    }

	    private void ObstacleSpawn(Vector3 spawnPosition)
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
			
			_lastSpawnDistance = GameManager.Instance.LevelManager.DistanceTraveled;
			_nextSpawnDistance = Random.Range(spawnDistanceRange.x, spawnDistanceRange.y);
			
			_nextSpawnDelay = Random.Range(spawnDelayRange.x, spawnDelayRange.y);
			_timePassed = 0;
		}
    }
}
