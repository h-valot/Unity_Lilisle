using UnityEngine;

public class Hearth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_HeartHealth _rsoHeartHealth;

    public void UpdateHealth(int amount)
    {
        _rsoHeartHealth.value += amount;
        print("HEARTH: health value changed");
    }
}