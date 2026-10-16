using UnityEngine;

public class WorkStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.SetMode(InteractMode.WORK);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.SetMode(InteractMode.APPEASE);
        }
    }
}
