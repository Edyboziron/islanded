using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurfaceScatterSpawner : MonoBehaviour
{
    [Header("Hedef Yüzey Ayarlarý")]
    public bool useTagSearch = false;
    public string surfaceTag = "Untagged";
    public Collider targetSurface;

    [Header("Spawn Edilecek Obje")]
    public GameObject prefabToSpawn;

    [Header("Daðýtým Ayarlarý")]
    [Tooltip("Ýþaretliyse her yüzeyde sadece 1 adet spawn yapýlýr.")]
    public bool spawnExactlyOnePerSurface = false;
    [Tooltip("Kaç deneme yapýlacaðý.")]
    public int attemptCount = 200;

    [Range(0f, 100f)] public float spawnChance = 60f;
    [Range(0f, 90f)] public float maxSlopeAngle = 45f;

    [Header("Boyut (Scale) Ayarlarý")]
    public float minScaleMultiplier = 0.8f;
    public float maxScaleMultiplier = 1.2f;

    [Header("Yerleþim Ayarlarý")]
    public float embedDepth = 0.1f;
    public bool alignYAxisUp = true;

    [Header("Optimizasyon & Hiyerarþi")]
    public bool isLowPriority = false;
    public float spawnDelay = 0.2f;

    // Oluþturulan objeleri takip etmek için liste
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // --- KRÝTÝK DÜZELTME: OYUN BAÞLADIÐINDA ÇALIÞTIR ---
    void Start()
    {
        // Unity'nin tag'leri ve collider'larý tam tanýmasý için 1 kare bekleyip öyle baþlar
        StartCoroutine(InitialSpawnRoutine());
    }

    IEnumerator InitialSpawnRoutine()
    {
        yield return null; // 1 kare bekle
        RegenerateIsland();
    }

    // --- DIÞARIDAN (YATAKTAN) ÇAÐRILACAK FONKSÝYON ---
    public void RegenerateIsland()
    {
        StopAllCoroutines(); // Hali hazýrda çalýþan bir spawn varsa durdur
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
            Debug.LogWarning(gameObject.name + ": Hedef yüzey bulunamadý!");
            yield break;
        }

        // Spawn iþlemini baþlat
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

                    // Yüzeyin üzerinden aþaðý doðru ýþýn (ray) at
                    Vector3 rayStart = new Vector3(randomX, bounds.max.y + 5f, randomZ);
                    Ray ray = new Ray(rayStart, Vector3.down);
                    RaycastHit hit;

                    if (currentSurface.Raycast(ray, out hit, bounds.size.y + 10f))
                    {
                        // Eðim kontrolü
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

                            // Boyutlandýrma
                            float randomScale = Random.Range(minScaleMultiplier, maxScaleMultiplier);
                            spawnedObj.transform.localScale = prefabToSpawn.transform.localScale * randomScale;

                            if (spawnExactlyOnePerSurface) break;
                        }
                    }
                }
            }
            // Her yüzey iþleminden sonra bir kare bekle (FPS düþüþünü engeller)
            yield return null;
        }
        Debug.Log(gameObject.name + ": Ada objeleri baþarýyla yerleþtirildi.");
    }
}