using UnityEngine;
using UnityEngine.UI;

public class MarangozManager : MonoBehaviour
{
    public static MarangozManager Instance;

    [Header("Görsel Referanslar")]
    public GameObject govdeTahtalariVisual; // Geminin yanýndaki ekstra tahtalar
    public GameObject pruvaZirhiVisual;     // Geminin önündeki metal zýrh

    [Header("Power-up Durumlarý")]
    public bool level1_SaglamGovde = false;
    public bool level2_ZirhliPruva = false;

    [Header("Buton Referanslarý")]
    public Button saglamGovdeButonu;
    public Button zirhliPruvaButonu;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (govdeTahtalariVisual != null) govdeTahtalariVisual.SetActive(false);
        if (pruvaZirhiVisual != null) pruvaZirhiVisual.SetActive(false);
    }

    public void Buy_SaglamGovde()
    {
        if (GlobalInventory.Instance.SpendItem("Tahta", 10))
        {
            level1_SaglamGovde = true;
            if (ShipManager.Instance != null) ShipManager.Instance.leakFrequency = 4;

            // GÖRSELÝ AKTÝF ET
            if (govdeTahtalariVisual != null) govdeTahtalariVisual.SetActive(true);

            if (saglamGovdeButonu != null) saglamGovdeButonu.interactable = false;
        }
    }

    public void Buy_ZirhliPruva()
    {
        if (GlobalInventory.Instance.SpendItem("Civi", 5))
        {
            level2_ZirhliPruva = true;
            if (ShipManager.Instance != null)
            {
                ShipManager.Instance.maxLeaks = 4;
                ShipManager.Instance.UpdateShipUI();
            }

            // GÖRSELÝ AKTÝF ET
            if (pruvaZirhiVisual != null) pruvaZirhiVisual.SetActive(true);

            if (zirhliPruvaButonu != null) zirhliPruvaButonu.interactable = false;
        }
    }
}