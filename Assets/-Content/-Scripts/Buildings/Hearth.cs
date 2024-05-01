using UnityEngine;

public class Hearth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Heart _rsoHeart;

    public void UpdateHealth(int amount)
    {
        _rsoHeart.value += amount;
    }
}