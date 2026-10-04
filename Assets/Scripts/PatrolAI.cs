using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PatrolAI : MonoBehaviour
{
    [Header("Devriye Ayarları")]
    [Tooltip("Karakterin sırayla gideceği noktalar.")]
    public Transform[] waypoints;

    [Tooltip("Karakterin yürüme hızı.")]
    public float moveSpeed = 3f;

    [Tooltip("Karakterin kendi ekseninde dönüş hızı (Örn: 300).")]
    public float turnSpeed = 300f;

    [Header("Bekleme ve Rastgelelik Ayarları")]
    [Tooltip("Noktaya ulaştığında bekleme kararı alırsa kaç saniye bekleyecek?")]
    public float waitTimeAtWaypoint = 2f;

    [Tooltip("Karakterin bir noktaya ulaştığında bekleme yapma ihtimali (%0 hiç beklemez, %100 her noktada bekler).")]
    [Range(0f, 100f)]
    public float waitChance = 50f;

    [Header("Animasyon Ayarları")]
    [Tooltip("Animator içindeki yürüme parametresinin (Bool) tam adı.")]
    public string isWalkingParam = "IsWalking";

    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("PatrolAI: Waypoint dizisi boş!");
        }
    }

    void Update()
    {
        // Eğer waypoint yoksa veya bekleme durumundaysa hareket kodunu çalıştırma
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

            // Şu anki açımız ile hedefin açısı arasındaki farkı ölç
            float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);

            // 1. AŞAMA: Eğer hedefe tam dönmemişsek (örneğin 5 dereceden fazla fark varsa) SADECE DÖN
            if (angleToTarget > 5f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                // Sadece döndüğü için yürüme animasyonunu durdur (Idle'da kalarak dönsün)
                animator.SetBool(isWalkingParam, false);

                // Return diyerek aşağıdaki yürüme kodunun çalışmasını engelliyoruz
                return;
            }
        }

        // --- 2. AŞAMA: BURADAN AŞAĞISI SADECE KARAKTER YÜZÜNÜ HEDEFE DÖNDÜYSE ÇALIŞIR ---

        // Hedefe Doğru İlerleme
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // Karakter adım atıyor, yürüme animasyonunu tetikle
        animator.SetBool(isWalkingParam, true);

        // Hedefe Ulaşıldı mı Kontrolü
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