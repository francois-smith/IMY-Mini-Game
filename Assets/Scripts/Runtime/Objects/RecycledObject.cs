/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using Runtime.Managers;
using UnityEngine;

namespace Runtime.Objects
{
    public class RecycledObject : MonoBehaviour
    {
        public float distanceToRecycle = 5f;

        private void Update()
        {
            if (!CheckRecycleCondition(GetComponent<Collider2D>().bounds)) return;
            Destroy(gameObject);
        }

        private bool CheckRecycleCondition(Bounds objectBounds)
        {
            var tmpRecycleBounds = GameManager.Instance.LevelManager.levelBounds;
            tmpRecycleBounds.extents += Vector3.one * distanceToRecycle;
            return !objectBounds.Intersects(tmpRecycleBounds);
        }
    }
}
