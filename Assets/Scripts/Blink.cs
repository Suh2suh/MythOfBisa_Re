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
        float MinimumVarianceLevel = TimeForVariance * 10f;                    //�ڿ��������� ���� ��ȭ �ð��� 10�����ŭ �ܰ踦 �����ؾ� ��. EX) 1�� �� 10�ܰ迡 ���� Alpha���� 1 -> 0���� ��ȭ

        LevelForVariance = MinimumVarianceLevel * SoftVariationLevel;   //�� �� ��ȭ�� ������ ��ġ�� ���İ� ��ȭ �ܰ� (�������� �ڿ���������)


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
