using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PatrolAI : MonoBehaviour
{
    [Header("Devriye Ayarlarý")]
    [Tooltip("Karakterin sýrayla gideceði noktalar.")]
    public Transform[] waypoints;

    [Tooltip("Karakterin yürüme hýzý.")]
    public float moveSpeed = 3f;

    [Tooltip("Karakterin kendi ekseninde dönüþ hýzý (Örn: 300).")]
    public float turnSpeed = 300f;

    [Header("Bekleme ve Rastgelelik Ayarlarý")]
    [Tooltip("Noktaya ulaþtýðýnda bekleme kararý alýrsa kaç saniye bekleyecek?")]
    public float waitTimeAtWaypoint = 2f;

    [Tooltip("Karakterin bir noktaya ulaþtýðýnda bekleme yapma ihtimali (%0 hiç beklemez, %100 her noktada bekler).")]
    [Range(0f, 100f)]
    public float waitChance = 50f;

    [Header("Animasyon Ayarlarý")]
    [Tooltip("Animator içindeki yürüme parametresinin (Bool) tam adý.")]
    public string isWalkingParam = "IsWalking";

    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("PatrolAI: Waypoint dizisi boþ!");
        }
    }

    void Update()
    {
        // Eðer waypoint yoksa veya bekleme durumundaysa hareket kodunu çalýþtýrma
        if (waypoints.Length == 0 || isWaiting) return;

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            // Hedef rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Þu anki açýmýz ile hedefin açýsý arasýndaki farký ölç
            float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);

            // 1. AÞAMA: Eðer hedefe tam dönmemiþsek (örneðin 5 dereceden fazla fark varsa) SADECE DÖN
            if (angleToTarget > 5f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                // Sadece döndüðü için yürüme animasyonunu durdur (Idle'da kalarak dönsün)
                animator.SetBool(isWalkingParam, false);

                // Return diyerek aþaðýdaki yürüme kodunun çalýþmasýný engelliyoruz
                return;
            }
        }

        // --- 2. AÞAMA: BURADAN AÞAÐISI SADECE KARAKTER YÜZÜNÜ HEDEFE DÖNDÜYSE ÇALIÞIR ---

        // Hedefe Doðru Ýlerleme
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // Karakter adým atýyor, yürüme animasyonunu tetikle
        animator.SetBool(isWalkingParam, true);

        // Hedefe Ulaþýldý mý Kontrolü
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            float randomValue = Random.Range(0f, 100f);

            if (randomValue <= waitChance)
            {
                StartCoroutine(WaitRoutine());
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    private IEnumerator WaitRoutine()
    {
        isWaiting = true;

        animator.SetBool(isWalkingParam, false);

        yield return new WaitForSeconds(waitTimeAtWaypoint);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        isWaiting = false;
    }
}