using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public int yarnCount = 0; // Track the number of yarn balls collected
    public int fishpoleCount = 0; // Track the number of yarn balls collected
    public int magicShroomCount = 0; // Track the number of magic mushrooms collected
    public TMP_Text HpIncreaseText; // 🗨️ Reference to TextMeshPro 3D
    public AudioClip increase;

    private PlayerHealth playerHealth; // Reference to player's health system
    private SlashEffect slashEffect; // Ref to player's standard attack

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>(); // Get reference to PlayerHealth script

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
        Debug.Log("Magic Mushroom Collected! Total: " + magicShroomCount);

        Debug.Log("Attempting to show ATK power Increase text!");
        StartCoroutine(ShowHealthBonusText()); // Start fade animation

        // Increase Player's Max ATK power
        if (playerHealth != null)
        {
            slashEffect.IncreaseMaxATKpower(5);
            Debug.Log("Game data updated, ATK power increased!");
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

            if (GetComponent<PlayerHealth>() != null)
            {
                GameData.instance.maxHealth = GetComponent<PlayerHealth>().maxHealth;
            }

            if (GetComponent<SlashEffect>() != null)
            {
                GameData.instance.regularAttackPower = GetComponent<SlashEffect>().damage;
            }

            if (GetComponent<LightningEffect>() != null)
            {
                GameData.instance.specialAttackPower = GetComponent<LightningEffect>().damage;
            }

            Debug.Log($"📊 GameData Updated: HP {GameData.instance.maxHealth}, Attack {GameData.instance.regularAttackPower}, Special {GameData.instance.specialAttackPower}");
        }
    }
}
