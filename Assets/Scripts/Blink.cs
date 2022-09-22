using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    Image myImage;
    static float imageAlpha = 1f;

    [SerializeField]
    public float TimeForSingleBlink = 1f;
    [SerializeField]
    public float SoftVariationLevel = 1f;

    float TimeForVariance;
    float LevelForVariance;

    float SecPerOneLevel;
    float AlphaPerOneLevel;

    private void Awake()
    {
        InitBlink();
    }

    private void OnEnable()
    {
        StartBlink();
    }


    private void InitBlink()
    {
        myImage = gameObject.GetComponent<Image>();

        if (SoftVariationLevel > 2)
        {
            SoftVariationLevel = 2;
        }

        TimeForVariance = TimeForSingleBlink / 2;
        float MinimumVarianceLevel = TimeForVariance * 10f;                    //자연스러움을 위해 변화 시간의 10배수만큼 단계를 설정해야 함. EX) 1초 당 10단계에 걸쳐 Alpha값이 1 -> 0으로 변화

        LevelForVariance = MinimumVarianceLevel * SoftVariationLevel;   //한 번 변화할 때마다 거치는 알파값 변화 단계 (높을수록 자연스러워짐)


        AlphaPerOneLevel = 1f / LevelForVariance;
        SecPerOneLevel = TimeForVariance / LevelForVariance;
    }

    private void StartBlink()
    {
        if (imageAlpha > 0)
        {
            StartCoroutine(makeItInvisible());
        }
        else
        {
            StartCoroutine(makeItVisible());
        }
    }

    private IEnumerator makeItInvisible()
    {
        while (imageAlpha > 0)
        {
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, imageAlpha);

            imageAlpha -= AlphaPerOneLevel;

            yield return new WaitForSeconds(SecPerOneLevel);
        }

        StartCoroutine(makeItVisible());
    }

    private IEnumerator makeItVisible()
    {
        while (imageAlpha < 1)
        {
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, imageAlpha);

            imageAlpha += AlphaPerOneLevel;

            yield return new WaitForSeconds(SecPerOneLevel);
        }

        StartCoroutine(makeItInvisible());
    }
}
