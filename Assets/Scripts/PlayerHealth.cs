using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Elemanlarý")]
    public Slider healthSlider;        // Canvas'taki Slider'ý buraya atayacaðýz
    public TextMeshProUGUI healthText; // Ýsteðe baðlý sayýsal gösterge

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // UI Baþlangýç ayarlarý
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
        UpdateUI();
    }

    // Hasar alma fonksiyonu (Bakteriler bu fonksiyonu çaðýracak)
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Canýn 0-100 arasýnda kalmasýný saðlar

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Can doldurma fonksiyonu (Ýlerde lazým olur)
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (healthText != null) healthText.text = "Can: %" + currentHealth.ToString("F0");
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Robot parçalandý! Oyun bitti.");
        // Buraya ölüm animasyonu veya yeniden baþlatma ekraný gelebilir
    }
}