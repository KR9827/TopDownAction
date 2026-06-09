using UnityEngine;

public class Potion : MonoBehaviour
{
    private bool flag = false;
    void Start()
    {

    }

    void Update()
    {
        if (flag)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ItemManager.Instance.AddPotion();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("enter");
            flag = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("exit");
            flag = false;
        }
    }
}
