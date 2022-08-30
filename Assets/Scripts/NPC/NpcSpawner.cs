using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
	#region Initialization

	public int questNum;
    public List<GameObject> NpcList;

	Vector3 spawnPos;

	private void Awake()
	{
		spawnPos = transform.position;
	}

	#endregion

	#region Spawn/Delete Npc
	public Vector3 GetSpawnPos()
	{
		spawnPos = transform.position;

		return spawnPos;
	}

	public void SpawnNpc()
	{
		//ȸ���� ���� �ʿ�

		foreach(GameObject npc in NpcList)
		{
			Instantiate(npc, transform.position, transform.rotation, transform);
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
			Debug.Log(questNum + "�� ����Ʈ ��ȭ ����");
			outliner.OutlineMode = Outline.Mode.OutlineHidden;
			//npc quest number pass
			//��ȭ ����
		}
	}

	void ProcessConversationEnd()
	{

	}


	#endregion
}
