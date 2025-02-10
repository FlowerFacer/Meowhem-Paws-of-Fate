using UnityEngine;
using System.Collections;

public class LightningEffectController : MonoBehaviour
{
    [Header("Lightning Settings")]
    public GameObject lightningAttackPrefab1;
    public GameObject lightningAttackPrefab2;
    public Transform lightningSpawnPoint1;
    public Transform lightningSpawnPoint2;
    public AudioClip RedLightning;

    public BossTutu3 bossTutu; // Reference to the boss script
    public GameObject tutuBoss; // Reference to the actual boss GameObject

    private bool isLightningActive = false;
    private bool canLightning1 = true;
    private bool canLightning2 = true;

    void Start()
    {
        // Find the boss if not assigned
        if (bossTutu == null)
        {
            bossTutu = FindFirstObjectByType<BossTutu3>();
        }
    }

    void Update()
    {
        // **Start lightning when boss transforms!**
        if (bossTutu != null && bossTutu.isBigMouse && !isLightningActive)
        {
            isLightningActive = true;
            StartCoroutine(LightningLoop());
        }

        // **Stop lightning when boss is defeated**
        if (tutuBoss == null && isLightningActive)
        {
            isLightningActive = false;
            StopAllCoroutines();
        }
    }

    IEnumerator LightningLoop()
    {
        while (isLightningActive)
        {
            if (canLightning1) StartCoroutine(PerformLightning1());
            if (canLightning2) StartCoroutine(PerformLightning2());

            yield return new WaitForSeconds(1.5f); // Adjust as needed for delay between strikes
        }
    }

    IEnumerator PerformLightning1()
    {
        canLightning1 = false;

        if (lightningAttackPrefab1 != null && lightningSpawnPoint1 != null)
        {
            if (RedLightning != null)
            {
                AudioManager.instance.PlaySound(RedLightning);
            }

            GameObject attackInstance = Instantiate(
                lightningAttackPrefab1,
                new Vector3(lightningSpawnPoint1.position.x, lightningSpawnPoint1.position.y, lightningSpawnPoint1.position.z),
                Quaternion.identity
            );
            Destroy(attackInstance, 2f);
        }

        yield return new WaitForSeconds(2f);
        canLightning1 = true;
    }

    IEnumerator PerformLightning2()
    {
        canLightning2 = false;

        if (lightningAttackPrefab2 != null && lightningSpawnPoint2 != null)
        {
            if (RedLightning != null)
            {
                AudioManager.instance.PlaySound(RedLightning);
            }

            GameObject attackInstance = Instantiate(
                lightningAttackPrefab2,
                new Vector3(lightningSpawnPoint2.position.x, lightningSpawnPoint2.position.y, lightningSpawnPoint2.position.z),
                Quaternion.identity
            );
            Destroy(attackInstance, 2f);
        }

        yield return new WaitForSeconds(3f);
        canLightning2 = true;
    }
}
