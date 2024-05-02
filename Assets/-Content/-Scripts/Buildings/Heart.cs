using UnityEngine;

public class Heart : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Heart _rsoHeart;

    public void UpdateHealth(int amount)
    {
        _rsoHeart.value += amount;
    }
}