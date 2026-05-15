using UnityEngine;

public class ProximityPanel : MonoBehaviour
{
    [Header("OTOMATÝK BULMA AYARLARI")]
    [Tooltip("Hiyerarþideki açýlacak panelin ADINI buraya yaz (Örn: MarangozPanel)")]
    public string targetPanelName;

    [Header("MESAFE AYARI")]
    public float distanceToOpen = 3f;

    // Statik yaparak tüm eþyalarýn ayný "E" panelini kullanmasýný saðlýyoruz
    private static GameObject globalPressEPanel;
    private GameObject specificPanel;
    private Transform playerTransform;
    private bool isPanelOpen = false;

    void Start()
    {
        // 1. "E'ye Bas" panelini isminden bul (Sürükleme gerektirmez)
        if (globalPressEPanel == null)
            globalPressEPanel = GameObject.Find("PressEPanel");

        // 2. Açýlacak olan ana paneli isminden bul
        if (!string.IsNullOrEmpty(targetPanelName))
            specificPanel = GameObject.Find(targetPanelName);

        // Baþlangýçta paneli gizle
        if (specificPanel != null) specificPanel.SetActive(false);
    }

    void Update()
    {
        // Karakteri bulana kadar aramaya devam et (Build hatasý korumasý)
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= distanceToOpen)
        {
            if (!isPanelOpen)
            {
                isPanelOpen = true;
                // Yaklaþýnca hem 'E' yazýsýný hem ana paneli açýyoruz
                if (specificPanel != null) specificPanel.SetActive(true);
                if (globalPressEPanel != null) globalPressEPanel.SetActive(true);
            }
        }
        else
        {
            if (isPanelOpen)
            {
                isPanelOpen = false;
                // Uzaklaþýnca her þeyi kapatýyoruz
                if (specificPanel != null) specificPanel.SetActive(false);
                if (globalPressEPanel != null && globalPressEPanel.activeSelf)
                    globalPressEPanel.SetActive(false);
            }
        }
    }
}