using UnityEngine;

public class SceneChange_Shop : MonoBehaviour
{
    private Player player;
    private bool playerInRangeFlag = false;

    void Update()
    {
        if (playerInRangeFlag && Input.GetKeyDown(KeyCode.E))
        {
            SceneFadeManager.Instance.ChangeSceneWithFade("Shop");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("入った");
            player = other.GetComponent<Player>();
            playerInRangeFlag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("出た");
        player = null;
        playerInRangeFlag = false;
    }
}
