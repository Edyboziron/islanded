using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurfaceScatterSpawner : MonoBehaviour
{
    [Header("Hedef Yüzey Ayarları")]
    public bool useTagSearch = false;
    public string surfaceTag = "Untagged";
    public Collider targetSurface;

    [Header("Spawn Edilecek Obje")]
    public GameObject prefabToSpawn;

    [Header("Dağıtım Ayarları")]
    [Tooltip("İşaretliyse her yüzeyde sadece 1 adet spawn yapılır.")]
    public bool spawnExactlyOnePerSurface = false;
    [Tooltip("Kaç deneme yapılacağı.")]
    public int attemptCount = 200;

    [Range(0f, 100f)] public float spawnChance = 60f;
    [Range(0f, 90f)] public float maxSlopeAngle = 45f;

    [Header("Boyut (Scale) Ayarları")]
    public float minScaleMultiplier = 0.8f;
    public float maxScaleMultiplier = 1.2f;

    [Header("Yerleşim Ayarları")]
    public float embedDepth = 0.1f;
    public bool alignYAxisUp = true;

    [Header("Optimizasyon & Hiyerarşi")]
    public bool isLowPriority = false;
    public float spawnDelay = 0.2f;

    // Oluşturulan objeleri takip etmek için liste
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // --- KRİTİK DÜZELTME: OYUN BAŞLADIĞINDA ÇALIŞTIR ---
    void Start()
    {
        // Unity'nin tag'leri ve collider'ları tam tanıması için 1 kare bekleyip öyle başlar
        StartCoroutine(InitialSpawnRoutine());
    }

    IEnumerator InitialSpawnRoutine()
    {
        yield return null; // 1 kare bekle
        RegenerateIsland();
    }

    // --- DIŞARIDAN (YATAKTAN) ÇAĞRILACAK FONKSİYON ---
    public void RegenerateIsland()
    {
        StopAllCoroutines(); // Hali hazırda çalışan bir spawn varsa durdur
        ClearOldObjects();   // Eskileri sil
        StartCoroutine(SpawnRoutine()); // Yenileri yarat
    }

    private void ClearOldObjects()
    {
        // Listedeki tüm objeleri yok et
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] != null)
            {
                Destroy(spawnedObjects[i]);
            }
        }
        spawnedObjects.Clear();
    }

    IEnumerator SpawnRoutine()
    {
        if (isLowPriority)
        {
            yield return new WaitForSeconds(spawnDelay);
        }

        List<Collider> targetColliders = new List<Collider>();

        // Yüzeyleri bul
        if (useTagSearch)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(surfaceTag);
            foreach (GameObject obj in taggedObjects)
            {
                Collider col = obj.GetComponent<Collider>();
                if (col != null) targetColliders.Add(col);
            }
        }
        else if (targetSurface != null)
        {
            targetColliders.Add(targetSurface);
        }

        if (targetColliders.Count == 0)
        {
            Debug.LogWarning(gameObject.name + ": Hedef yüzey bulunamadı!");
            yield break;
        }

        // Spawn işlemini başlat
        foreach (Collider currentSurface in targetColliders)
        {
            Bounds bounds = currentSurface.bounds;
            int iterations = spawnExactlyOnePerSurface ? 1 : attemptCount;

            for (int i = 0; i < iterations; i++)
            {
                if (Random.Range(0f, 100f) <= spawnChance)
                {
                    float randomX = Random.Range(bounds.min.x, bounds.max.x);
                    float randomZ = Random.Range(bounds.min.z, bounds.max.z);

                    // Yüzeyin üzerinden aşağı doğru ışın (ray) at
                    Vector3 rayStart = new Vector3(randomX, bounds.max.y + 5f, randomZ);
                    Ray ray = new Ray(rayStart, Vector3.down);
                    RaycastHit hit;

                    if (currentSurface.Raycast(ray, out hit, bounds.size.y + 10f))
                    {
                        // Eğim kontrolü
                        if (Vector3.Angle(Vector3.up, hit.normal) <= maxSlopeAngle)
                        {
                            // Rotasyon hesaplama
                            Quaternion finalRotation;
                            if (alignYAxisUp)
                            {
                                finalRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                            }
                            else
                            {
                                finalRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                            }

                            // Pozisyon ve Instantiate
                            Vector3 pos = hit.point - (hit.normal * embedDepth);
                            GameObject spawnedObj = Instantiate(prefabToSpawn, pos, finalRotation);

                            // Listeye ekle (temizlik için)
                            spawnedObjects.Add(spawnedObj);

                            // Boyutlandırma
                            float randomScale = Random.Range(minScaleMultiplier, maxScaleMultiplier);
                            spawnedObj.transform.localScale = prefabToSpawn.transform.localScale * randomScale;

                            if (spawnExactlyOnePerSurface) break;
                        }
                    }
                }
            }
            // Her yüzey işleminden sonra bir kare bekle (FPS düşüşünü engeller)
            yield return null;
        }
        Debug.Log(gameObject.name + ": Ada objeleri başarıyla yerleştirildi.");
    }
}