using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
	public int questNum;
    public List<GameObject> NpcList;

	Vector3 spawnPos;

	private void Awake()
	{
		spawnPos = transform.position;
	}

	public Vector3 GetSpawnPos()
	{
		return spawnPos;
	}

	public void SpawnNpc()
	{
		//회전값 조정 필요

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
}
