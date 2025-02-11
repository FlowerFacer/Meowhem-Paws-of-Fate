using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public int yarnCount = 0; // Track the number of yarn balls collected
    public int fishpoleCount = 0; // Track the number of yarn balls collected
    public int magicShroomCount = 0; // Track the number of magic mushrooms collected
    public int tutuPlushieCount = 0;
    public TMP_Text HpIncreaseText; // 🗨️ Reference to TextMeshPro 3D
    public AudioClip increase;
    private PlayerController playerController;
    private PlayerHealth playerHealth; // Reference to player's health system
    private SlashEffect slashEffect; // Ref to player's standard attack

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>(); // Get reference to PlayerHealth script
        playerController = GetComponent<PlayerController>();

        if (GameData.instance != null)
        {
            fishpoleCount = GameData.instance.fishpoleCount; // ✅ Load Attack Power
            Debug.Log("📥 Loaded Fishpole Count: " + fishpoleCount);

            yarnCount = GameData.instance.yarnCount; // ✅ Load Attack Power
            Debug.Log("📥 Loaded Yarn Count: " + yarnCount);

            magicShroomCount = GameData.instance.magicShroomCount; // ✅ Load Attack Power
            Debug.Log("📥 Loaded Mushroom Count: " + magicShroomCount);

            tutuPlushieCount = GameData.instance.tutuPlushieCount; // ✅ Load Attack Power
            Debug.Log("📥 Loaded Plush Count: " + tutuPlushieCount);
        }

        if (HpIncreaseText != null)
        {
            // **Get the child text**
            TMP_Text childText = HpIncreaseText.transform.GetChild(0).GetComponent<TMP_Text>();

            Color transparentMain = HpIncreaseText.color;
            transparentMain.a = 0f; // Set transparency to 0
            HpIncreaseText.color = transparentMain;

            if (childText != null)
            {
                Color transparentChild = childText.color;
                transparentChild.a = 0f;
                childText.color = transparentChild;
            }
        }
    }

    public void AddYarn(int amount)
    {
        yarnCount += amount;
        UpdateGameData();
        Debug.Log("Current Yarn Balls: " + yarnCount);
    }
    public void AddFishpole(int amount)
    {
        fishpoleCount += amount;
        UpdateGameData();
        Debug.Log("Current Fishpoles: " + fishpoleCount);
    }

    public void AddMagicShroom(int amount)
    {
        magicShroomCount += amount;
        Debug.Log("Magic Mushroom Collected! Total: " + magicShroomCount);

        Debug.Log("Attempting to show HP Increase text!");
        StartCoroutine(ShowHealthBonusText()); // Start fade animation

        // Increase Player's Max Health
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(5);
            Debug.Log("Game data updated, max health increased!");
        }
    }

    public void AddPowerMagicShroom(int amount)
    {
        magicShroomCount += amount;
        Debug.Log($"🍄 Magic Mushroom Collected! Total: {magicShroomCount}");

        StartCoroutine(ShowHealthBonusText()); // Start fade animation

        if (playerController != null)
        {
            playerController.IncreaseMaxATKpower(5);
            Debug.Log("✅ ATK Power increased via PlayerController!");
        }
        else
        {
            Debug.LogError("❌ PlayerController not found!");
        }
    }

    public void AddTutuPlushie(int amount)
    {
        tutuPlushieCount += amount;
        Debug.Log($" plushie Collected! Total: {tutuPlushieCount}");

        StartCoroutine(ShowHealthBonusText()); // Start fade animation

        if (playerController != null)
        {
            playerController.IncreaseMaxSPATKpower(5);
            Debug.Log("✅ SP ATK Power increased via PlayerController!");
        }
        else
        {
            Debug.LogError("❌ PlayerController not found!");
        }
    }

    IEnumerator ShowHealthBonusText()
    {
        if (HpIncreaseText != null)
        {
            // **Get the child text**
            TMP_Text childText = HpIncreaseText.transform.GetChild(0).GetComponent<TMP_Text>();

            Debug.Log("✅ HP Texts Found! Starting fade-in...");

            Color mainTextColor = HpIncreaseText.color;
            Color childTextColor = childText != null ? childText.color : mainTextColor;

            // **Fade In Effect**
            float fadeDuration = 0.5f; // Speed of fade-in
            float timer = 0f;

            if (increase != null)
            {
                AudioManager.instance.PlaySound(increase);
            }

            while (timer < fadeDuration)
            {
                mainTextColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                HpIncreaseText.color = mainTextColor;

                if (childText != null)
                {
                    childTextColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                    childText.color = childTextColor;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            mainTextColor.a = 1f;
            HpIncreaseText.color = mainTextColor;

            if (childText != null)
            {
                childTextColor.a = 1f;
                childText.color = childTextColor;
            }

            yield return new WaitForSeconds(3f); // Keep visible for 2 seconds

            Debug.Log("⏳ Starting fade-out...");

            // **Fade Out Effect**
            timer = 0f;
            while (timer < fadeDuration)
            {
                mainTextColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                HpIncreaseText.color = mainTextColor;

                if (childText != null)
                {
                    childTextColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                    childText.color = childTextColor;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            mainTextColor.a = 0f;
            HpIncreaseText.color = mainTextColor;

            if (childText != null)
            {
                childTextColor.a = 0f;
                childText.color = childTextColor;
            }

            Debug.Log("🛑 HP Text Hidden.");
        }
    }

    public void UpdateGameData()
    {
        if (GameData.instance != null)
        {
            GameData.instance.yarnCount = yarnCount;
            GameData.instance.fishpoleCount = fishpoleCount;
            GameData.instance.magicShroomCount = magicShroomCount;
            GameData.instance.tutuPlushieCount = tutuPlushieCount;

            PlayerController playerController = GetComponent<PlayerController>();


            if (playerController != null)
            {
                GameData.instance.regularAttackPower = playerController.attackPower; // ✅ Update Attack Power
                GameData.instance.specialAttackPower = playerController.specialAttackPower;
            }

            if (GetComponent<PlayerHealth>() != null)
            {
                GameData.instance.maxHealth = GetComponent<PlayerHealth>().maxHealth;
            }

            Debug.Log($"📊 GameData Updated: HP {GameData.instance.maxHealth}, Attack {GameData.instance.regularAttackPower}, Special {GameData.instance.specialAttackPower}");
        }
    }
}
