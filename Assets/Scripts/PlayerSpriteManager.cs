using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerSpriteManager : MonoBehaviour
{
    public SpriteResolver spriteResolver;

    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
    }

    public void SetHeadExpression(string spriteName)
    {
        spriteResolver.SetCategoryAndLabel("Head", spriteName);
    }
}
