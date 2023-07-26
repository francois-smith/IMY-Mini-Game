/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using Runtime.Managers;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LooseLife();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
