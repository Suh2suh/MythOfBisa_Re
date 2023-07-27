using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniPRight : MonoBehaviour
{
    public bool m_IsButtonDowning;

    public GameObject miniPlayer;
    RectTransform miniPlayerRect;
    public float moveSpeed = 6.0f;

    private void Start()
    {
        miniPlayerRect = miniPlayer.transform.GetComponent<RectTransform>();
    }

	private void FixedUpdate()
	{
        if (m_IsButtonDowning && miniPlayerRect.anchoredPosition.x < 360f)
        {
            miniPlayerRect.Translate(new Vector3(moveSpeed, 0));
        }
    }

	public void PointerDown()
    {
        m_IsButtonDowning = true;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
    }
}
