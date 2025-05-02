using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    [Header("BGM 슬라이더")]
    public Slider bgm_Slider;
    private float init_SoundValue = 0.2f;

    private void Start()
    {
        bgm_Slider.onValueChanged.AddListener(HandleValueChange);

        // BGM 초기화
        bgm_Slider.value = init_SoundValue;
        SoundManager.Instance.SetMusicVolume(init_SoundValue);

        // BGM 실행
        SoundManager.Instance.PlayMusic("Ascend to the Heavens");
    }

    private void HandleValueChange(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }
}
