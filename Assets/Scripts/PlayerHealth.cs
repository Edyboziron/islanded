using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Elemanları")]
    public Slider healthSlider;        // Canvas'taki Slider'ı buraya atayacağız
    public TextMeshProUGUI healthText; // İsteğe bağlı sayısal gösterge

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // UI Başlangıç ayarları
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
        UpdateUI();
    }

    // Hasar alma fonksiyonu (Bakteriler bu fonksiyonu çağıracak)
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Canın 0-100 arasında kalmasını sağlar

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Can doldurma fonksiyonu (İlerde lazım olur)
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
        Debug.Log("Robot parçalandı! Oyun bitti.");
        // Buraya ölüm animasyonu veya yeniden başlatma ekranı gelebilir
    }
}