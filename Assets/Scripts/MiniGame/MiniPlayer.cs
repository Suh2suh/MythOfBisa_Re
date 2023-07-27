using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlayer : MonoBehaviour
{
	public MiniGameManager miniGameManager;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		switch(collision.GetComponent<Item>().ItemType)
		{
			case 0:
				miniGameManager.LeftLife -= 1;
				break;
			case 1:
				miniGameManager.Score += 100;
				break;
		}

		collision.transform.gameObject.SetActive(false);
	}
}
