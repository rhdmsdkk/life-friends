using UnityEngine;

public class WorkStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.IsRed())
            {
                player.SetMode(InteractMode.WORK);
            }
            else
            {
                player.SetMode(InteractMode.WORK_PICK_UP);
            }
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
