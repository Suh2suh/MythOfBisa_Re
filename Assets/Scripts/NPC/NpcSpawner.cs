using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
	#region Initialization

	public int questNum;
    public List<GameObject> NpcList;

	Vector3 spawnPos;
	public Vector3 SpawnPos
	{
		get
		{
			return spawnPos;
		}
		set
		{
			spawnPos = value;
		}
	}

	bool isTouchable = false;

	// Make Npc Untouchable when the conversation is over
	public bool IsTouchable
	{
		get
		{
			return isTouchable;
		}
		set
		{
			isTouchable = value;
			Debug.Log(transform.name + " Touchable: " + isTouchable);

			if(isTouchable)
			{
				TouchOnScreen.isTouchDetectNeeded = true;

				for (int i = 0; i < NpcList.Count; i++)
				{
					NpcList[0].tag = "Touchable";
				}
			}
			else
			{
				TouchOnScreen.isTouchDetectNeeded = false;

				for (int i = 0; i < NpcList.Count; i++)
				{
					NpcList[0].tag = "Untouchable";
				}
			}
		}
	}


	private void Awake()
	{
		spawnPos = transform.position;
	}

	#endregion


	#region Spawn/Delete Npc

	public void SpawnNpc()
	{
		//회전값 조정 필요

		foreach(GameObject npc in NpcList)
		{
			Instantiate(npc, spawnPos, transform.rotation, transform);
		}
	}

	public void DeleteNpc()
	{
		for(int i = 0; i < NpcList.Count; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}
	#endregion

	#region Npc: ClickCheck / ActiveConversation


	bool isClicked = false;

	Outline outliner;

	public void CheckClickAndConversationOn()
	{
		if(!isClicked)
		{
			isClicked = true;

			for (int i = 0; i < NpcList.Count; i++)
			{
				outliner = transform.GetChild(i).gameObject.GetComponent<Outline>();
				outliner.OutlineMode = Outline.Mode.OutlineVisible;
			}
		}
		else
		{
			IsTouchable = false;

			Debug.Log(questNum + "번 퀘스트 대화 시작");
			outliner.OutlineMode = Outline.Mode.OutlineHidden;
			//npc quest number pass
			//대화 시작, 대화 끝나면 questnum ++;

			//<Test>
			GameData.questNum++;
			transform.parent.GetComponent<NpcManager>().FindRightSpawnPosNActiveNpc();
		}
	}

	void ProcessConversationEnd()
	{

	}


	#endregion
}
