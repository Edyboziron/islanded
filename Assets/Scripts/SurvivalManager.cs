using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.InputSystem;

public class SurvivalManager : MonoBehaviour
{
    public static SurvivalManager Instance;

    [Header("Isinlanma Ayarlari")]
    public Transform largeShipBoardingTarget;

    [Header("UI Elemanlari")]
    public TextMeshProUGUI timerText;
    public GameObject timerUIContainer;
    public GameObject pressEPanel;
    public Slider healthSlider;
    public Slider toklukSlider;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI toklukText;

    [Header("Oyun Sonu")]
    public GameObject gameOverPanel;
    private bool isDead = false;

    [Header("Hayatta Kalma Ayarlari")]
    public float maxTime = 30f;
    private float currentTime;
    private float currentHealth = 100f;

    [Header("Tokluk Ayarlari")]
    public float maxTokluk = 100f;
    public float toklukAzalmaHizi = 1f;
    private float currentTokluk;

    [Header("Ses Ayarlari")]
    public AudioSource survivalAudioSource;
    public AudioClip warningSound;
    public AudioClip damageSound;
    public float damageSoundCooldown = 1.5f;
    private float lastDamageSoundTime = 0f;
    private bool isWarningPlaying = false;

    private bool isInSafeZone = false;
    private bool isNearShip = false; // YENİ: Geminin yanındayken bunu true yapacağız
    private bool isTimeFrozen = false;
    private float freezeTimer = 0f;
    private float originalMoveSpeed;
    private float originalSprintSpeed;

    private CharacterController _characterController;
    private ThirdPersonController _tpController;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        currentTime = maxTime;
        currentTokluk = maxTokluk;
        isDead = false;

        _characterController = GetComponent<CharacterController>();
        _tpController = GetComponent<ThirdPersonController>();

        if (_tpController != null)
        {
            originalMoveSpeed = _tpController.MoveSpeed;
            originalSprintSpeed = _tpController.SprintSpeed;
        }

        if (survivalAudioSource == null) survivalAudioSource = GetComponent<AudioSource>();

        if (pressEPanel != null) pressEPanel.SetActive(false);

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void StartIslandSession()
    {
        if (isDead) return;

        isInSafeZone = false;
        isNearShip = false;
        currentTime = maxTime;
        isTimeFrozen = false;
        isWarningPlaying = false;
        if (survivalAudioSource != null) survivalAudioSource.Stop();
        ResetSpeed();

        if (timerUIContainer != null) timerUIContainer.SetActive(true);
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            TriggerGameOver();
            return;
        }

        if (isDead) return;

        // --- IŞINLANMA BUG'ININ ÇÖZÜMÜ ---
        // Tuş kontrolünü Update içine aldık ki asla kaçırmasın!
        if (isNearShip && !isInSafeZone)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                BoardShip();
            }
        }

        if (!isInSafeZone)
        {
            HandleSurvivalLogic();
            HandleKasifSkills();
        }
        else
        {
            if (isWarningPlaying) StopWarning();
        }

        UpdateUI();
    }

    public void TriggerGameOver()
    {
        isDead = true;

        StopWarning();

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (_tpController != null) _tpController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HandleSurvivalLogic()
    {
        if (isTimeFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0) isTimeFrozen = false;
            if (isWarningPlaying) StopWarning();
        }
        else
        {
            if (currentTime > 0) currentTime -= Time.deltaTime;

            if (currentTime <= 10f && currentTime > 0 && !isWarningPlaying)
            {
                PlayWarning();
            }
        }

        if (currentTokluk > 0)
        {
            float carpan = 1f;
            if (HungerManager.Instance != null && HungerManager.Instance.level1_ToklukHissi)
            {
                carpan = 0.5f;
            }
            currentTokluk -= Time.deltaTime * toklukAzalmaHizi * carpan;
        }

        if (currentTime <= 0)
        {
            currentHealth -= Time.deltaTime * 5f;
            PlayDamageSound();
            if (isWarningPlaying) StopWarning();
        }

        if (currentTokluk <= 0)
        {
            currentHealth -= Time.deltaTime * 2f;
            PlayDamageSound();
        }
    }

    void PlayDamageSound()
    {
        if (survivalAudioSource != null && damageSound != null)
        {
            if (Time.time - lastDamageSoundTime >= damageSoundCooldown)
            {
                survivalAudioSource.PlayOneShot(damageSound);
                lastDamageSoundTime = Time.time;
            }
        }
    }

    void PlayWarning()
    {
        if (survivalAudioSource != null && warningSound != null)
        {
            isWarningPlaying = true;
            survivalAudioSource.clip = warningSound;
            survivalAudioSource.loop = true;
            survivalAudioSource.Play();
        }
    }

    void StopWarning()
    {
        isWarningPlaying = false;
        if (survivalAudioSource != null) survivalAudioSource.Stop();
    }

    public void AddTokluk(float amount)
    {
        if (isDead) return;
        currentTokluk = Mathf.Clamp(currentTokluk + amount, 0, maxTokluk);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        PlayDamageSound();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            TriggerGameOver();
        }
    }

    public void SyncHealthWithTokluk()
    {
        if (isDead) return;

        currentHealth = currentTokluk;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            TriggerGameOver();
        }
    }

    void HandleKasifSkills()
    {
        if (KasifManager.Instance == null) return;

        if (KasifManager.Instance.skill_ZamanBukucu && !isTimeFrozen)
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                isTimeFrozen = true;
                freezeTimer = 15f;
            }
        }

        if (KasifManager.Instance.skill_SonGaz && currentTime <= 5f && currentTime > 0 && !isTimeFrozen)
        {
            _tpController.MoveSpeed = originalMoveSpeed * 1.5f;
            _tpController.SprintSpeed = originalSprintSpeed * 1.5f;
        }
        else if (!isTimeFrozen)
        {
            ResetSpeed();
        }
    }

    void ResetSpeed()
    {
        if (_tpController != null)
        {
            _tpController.MoveSpeed = originalMoveSpeed;
            _tpController.SprintSpeed = originalSprintSpeed;
        }
    }

    // --- YENİ EKLENEN TETİKLEYİCİLER (Artık OnTriggerStay kullanmıyoruz) ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone") && !isDead)
        {
            isNearShip = true;
            if (pressEPanel != null && !isInSafeZone) pressEPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isNearShip = false;
            if (pressEPanel != null) pressEPanel.SetActive(false);
        }
    }

    void BoardShip()
    {
        if (largeShipBoardingTarget == null) return;
        StopWarning();
        if (_characterController != null) _characterController.enabled = false;
        transform.position = largeShipBoardingTarget.position;
        transform.rotation = largeShipBoardingTarget.rotation;
        if (_characterController != null) _characterController.enabled = true;

        isInSafeZone = true;
        isNearShip = false; // İçeri girdiğimiz için bunu kapatıyoruz

        if (pressEPanel != null) pressEPanel.SetActive(false);
        if (ShipManager.Instance != null) ShipManager.Instance.NextDay();
    }

    void UpdateUI()
    {
        if (timerText != null)
        {
            string status = isTimeFrozen ? "DURDURULDU" : Mathf.Ceil(Mathf.Max(0, currentTime)).ToString() + "s";
            timerText.text = "Oksijen: " + status;
            timerText.color = (currentTime <= 10f && !isTimeFrozen && currentTime > 0) ? Color.red : Color.white;
        }

        if (healthSlider != null) healthSlider.value = currentHealth;
        if (toklukSlider != null) toklukSlider.value = currentTokluk;

        if (healthText != null) healthText.text = Mathf.Ceil(currentHealth).ToString();
        if (toklukText != null) toklukText.text = Mathf.Ceil(currentTokluk).ToString();
    }
}