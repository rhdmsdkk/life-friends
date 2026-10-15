using UnityEngine;

public class WorkStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("working");
            player.SetMode(InteractMode.WORK);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("not working");
            player.SetMode(InteractMode.APPEASE);
        }
    }
}
