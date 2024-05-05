using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private Slider _slider;

	[Header("External references")]
	[SerializeField] private RSO_Volume _rsoVolume;

    public void SetVolume(Slider slider)
    {
        _rsoVolume.value = slider.value;
    }

    public void ToggleVolume()
    {
        if (_rsoVolume.value == 0f)
        {
            _rsoVolume.value = 1f;
        }
        else
        {
            _rsoVolume.value = 0f;
        }

        _slider.value = _rsoVolume.value;
    }
}