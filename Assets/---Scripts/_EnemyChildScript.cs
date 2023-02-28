using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class _EnemyChildScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            //Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            // Restart
            // Player dies
            //Debug.Log("Player Failed");
        }
    }
}
