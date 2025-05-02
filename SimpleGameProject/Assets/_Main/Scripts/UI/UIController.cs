using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public enum Mode
{
    Adventure,
    Combat
}

public class UIController : MonoBehaviour
{
    //-------------------------------------------------- 이벤트 스크립트 참조
    [Header("Event")]
    public Character character;
    public HealthSystem healthSystem;
    public KD_System kdSystem;

    //--------------------------------------------------
    [Header("UI")]
    [Header("체력")]
    public Image hp_Fill;                       // 체력바
    public TMP_Text hp_Text;                    // 체력 텍스트
    private StringBuilder hp_String;

    [Header("모드 / 팝업")]
    public TMP_Text currentMode_Text;           // 현재 모드 텍스트
    private StringBuilder mode_String;

    public GameObject popUp_Obj;                // 팝업
    public GameObject adventureUI_Obj;          // 모험 UI
    public GameObject combatUI_Obj;             // 전투 UI 

    public Button popUp_Button;                 // 팝업 버튼
    public TMP_Text popUp_Text;                 // 팝업 텍스트
    private StringBuilder popUp_String;
    public bool isPopupOpen = true;             // 팝업 열림 체크

    public Color AdventureColor = new Color(148f / 255f, 112f / 255f, 183f / 255f);
    public Color CombatColor = Color.red;

    [Header("킬 / 데스")]
    public TMP_Text killCount_Text;
    public TMP_Text deathCount_Text;

    // TODO : 상태 변경 ( 모험 모드 , 전투 모드 )
    //        +@ 하위 오브젝트 연동

    private void Awake()
    {
        // 초기화
        hp_String = new StringBuilder();
        mode_String = new StringBuilder();
        popUp_String = new StringBuilder();

        isPopupOpen = true;
    }

    private void Start()
    {
        ChangeMode(Mode.Adventure);
        ChangePopupState();

        // 버튼 이벤트
        popUp_Button.onClick.AddListener(() => {
            isPopupOpen = !isPopupOpen;
            ChangePopupState();
        });

        // 이벤트 구독
        character.OnModeChange += ChangeMode;
        healthSystem.OnHealthChange += ChangeHP;
        kdSystem.OnKillCountChange += ChangeKillCount;
        kdSystem.OnDeathCountChange += ChangeDeathCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPopupOpen = !isPopupOpen;
            ChangePopupState();
        }

    }

    /// <summary>
    /// hp 변경 함수
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="curHp"></param>
    public void ChangeHP(float maxHp, float curHp)
    {
        hp_Fill.fillAmount = curHp / maxHp;

        ChangeStringBuilder(hp_String, curHp + "/" + maxHp);
        hp_Text.text = hp_String.ToString();
    }

    /// <summary>
    /// 모드 변경 함수
    /// </summary>
    /// <param name="mode"></param>
    public void ChangeMode(Mode mode)
    {
        // 모험 모드
        if(mode.Equals(Mode.Adventure))
        {
            // 텍스트 변경
            ChangeStringBuilder(mode_String, "모험 모드");

            currentMode_Text.text = mode_String.ToString();
            currentMode_Text.color = AdventureColor;

            // 모험 조작키 UI 활성화
            adventureUI_Obj.SetActive(true);
            combatUI_Obj.SetActive(false);
        }

        // 전투 모드
        else if (mode.Equals(Mode.Combat))
        {
            // 텍스트 변경
            ChangeStringBuilder(mode_String, "전투 모드");

            currentMode_Text.text = mode_String.ToString();
            currentMode_Text.color = CombatColor;

            // 전투 조작키 UI 활성화
            combatUI_Obj.SetActive(true);
            adventureUI_Obj.SetActive(false);
        }
    }

    /// <summary>
    /// 팝업 상태 변환 [닫기] [열기]
    /// </summary>
    private void ChangePopupState()
    {
        popUp_Obj.SetActive(isPopupOpen);

        if (isPopupOpen) {
            ChangeStringBuilder(popUp_String, "조작키\n닫기");
        }
        else {
            ChangeStringBuilder(popUp_String, "조작키\n열기");
        }
        popUp_Text.text = popUp_String.ToString();

        PlayPopupSound();
    }

    /// <summary>
    /// 킬 Count 변경 함수
    /// </summary>
    /// <param name="count"></param>
    public void ChangeKillCount(int count)
    {
        killCount_Text.text = count.ToString();
    }

    /// <summary>
    /// 데스 count 변경 함수
    /// </summary>
    /// <param name="count"></param>
    public void ChangeDeathCount(int count)
    {
        deathCount_Text.text = count.ToString();
    }

    /// <summary>
    /// 스트링빌더의 텍스트를 변경하는 함수
    /// </summary>
    /// <param name="value"></param>
    private void ChangeStringBuilder(StringBuilder sb, string value)
    {
        sb.Clear();
        sb.Append(value);
    }

    private void PlayPopupSound()
    {
        if(isPopupOpen)
        {
            SoundManager.Instance.PlaySFX("Popup_Open");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Popup_Close");
        }
    }
}
