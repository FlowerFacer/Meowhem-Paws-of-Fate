using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int yarnCount = 0; // Track the number of yarn balls collected
    public int fishpoleCount = 0; // Track the number of yarn balls collected

    public void AddYarn(int amount)
    {
        yarnCount += amount;
        Debug.Log("Current Yarn Balls: " + yarnCount);
    }
    public void AddFishpole(int amount)
    {
        fishpoleCount += amount;
        Debug.Log("Current Yarn Balls: " + fishpoleCount);
    }
}
