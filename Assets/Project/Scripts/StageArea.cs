using UnityEngine;
using System.Collections.Generic;

public class StageArea : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> spawners;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ToggleSpawners(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleSpawners(false);
        }
    }

    private void ToggleSpawners(bool enabled)
    {
        foreach (var sp in spawners)
            sp.enabled = enabled;
    }

}
