using UnityEngine;
using UnityEngine.Events;

public class EnemyGroundChecker : MonoBehaviour
{
    public UnityAction OnDirectionChange;

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnDirectionChange?.Invoke();
    }
}
