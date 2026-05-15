using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

// IPointerClickHandler: Tiklama olayini yakalamak icin eklendi
public class ButtonInfoHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum UpgradeType { None, ZamanBukucu, GenisHeybe, ToklukHissi, BesleyiciOgun, ZirhliPruva, SaglamGovde }

    [Header("Gelistirme Ayari")]
    public UpgradeType hangiGelistirme;

    [Header("Info Panel Ayarlari")]
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

    [Header("Buton Aciklamasi")]
    [TextArea(3, 10)]
    public string aciklama;

    [Header("Ses Ayarlari")]
    public AudioClip clickSound;  // Tiklama sesi
    public AudioClip hoverSound;  // Uzerine gelme sesi (Opsiyonel)
    private AudioSource audioSource;

    private Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();

        // AudioSource kontrolü: Yoksa otomatik ekler
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
        CheckAndRefresh();
    }

    void OnEnable()
    {
        CheckAndRefresh();
    }

    void OnDisable()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }

    // --- BUTONA TIKLANDIÐINDA ÇALIÞIR ---
    public void OnPointerClick(PointerEventData eventData)
    {
        // Buton etkilesime aciksa ve ses atanmissa cal
        if (myButton != null && myButton.interactable && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void CheckAndRefresh()
    {
        if (myButton == null) return;
        bool isBought = false;

        switch (hangiGelistirme)
        {
            case UpgradeType.ZamanBukucu:
                if (KasifManager.Instance != null) isBought = KasifManager.Instance.skill_ZamanBukucu;
                break;
            case UpgradeType.GenisHeybe:
                if (KasifManager.Instance != null) isBought = KasifManager.Instance.level2_GenisHeybe;
                break;
            case UpgradeType.ToklukHissi:
                if (HungerManager.Instance != null) isBought = HungerManager.Instance.level1_ToklukHissi;
                break;
            case UpgradeType.BesleyiciOgun:
                if (HungerManager.Instance != null) isBought = HungerManager.Instance.level2_BesleyiciOgun;
                break;
        }

        if (isBought) myButton.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hover sesini cal (Eger atanmissa)
        if (myButton != null && myButton.interactable && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        if (infoPanel != null && infoText != null)
        {
            if (myButton != null && !myButton.interactable && hangiGelistirme != UpgradeType.None)
            {
                infoText.text = "<color=green>BU GELISTIRME ZATEN ALINDI</color>";
            }
            else if (!string.IsNullOrEmpty(aciklama))
            {
                infoText.text = aciklama;
            }
            else
            {
                var tmpText = GetComponentInChildren<TextMeshProUGUI>();
                if (tmpText != null) infoText.text = tmpText.text;
            }
            infoPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }
}