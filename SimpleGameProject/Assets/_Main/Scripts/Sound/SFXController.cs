using UnityEngine;
using UnityEngine.UI;

public class SFXController : MonoBehaviour
{
    [Header("SFX 슬라이더")]
    public Slider sfx_Slider;
    private float init_SoundValue = 1f;

    private void Start()
    {
        sfx_Slider.onValueChanged.AddListener(HandleValueChange);

        // SFX 초기화
        sfx_Slider.value = init_SoundValue;
        SoundManager.Instance.SetSFXVolume(init_SoundValue);

        // Dialogue
        SoundManager.Instance.PlaySFX("Dialogue_Start");
    }

    private void HandleValueChange(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
}
