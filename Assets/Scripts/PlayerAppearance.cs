using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; //sprite du perso
    public Sprite defaultSprite; //apparence de base
    public Sprite passionRingSprite; //apparence avec ring passion
    public Sprite bravoureRingSprite; //apparence avec ring bravoure

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    public void ChangeAppearance(string ringType)
    {

        switch (ringType)
        {
            case "PassionRing":
                spriteRenderer.sprite = passionRingSprite;
                break;
            case "BravoureRing":
                spriteRenderer.sprite = bravoureRingSprite;
                break;
            default:
                spriteRenderer.sprite = defaultSprite;
                break;
             
        }
    }
}
