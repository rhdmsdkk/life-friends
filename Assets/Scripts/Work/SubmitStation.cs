using UnityEngine;

public class SubmitStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.IsBlue())
            {
                player.SetMode(InteractMode.SUBMIT);
            }
            else
            {
                player.SetMode(InteractMode.PICK_UP);
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
