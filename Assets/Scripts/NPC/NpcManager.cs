using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
	//GameData 퀘스트 번호 변경됨 -> 새로운 퀘스트 인식
	//새로운 퀘스트 -> npc 서서히 사라짐

	#region Initialization

		#region Dictionary

		[UDictionary.Split(35, 65)]
		public UDictionary1 npcDictionary;
		[Serializable]
		public class UDictionary1 : UDictionary<Key, Value> {	}

		[Serializable]
		public class Key
		{
			public int questNum;

			public Key(int questNum)
			{
				this.questNum = questNum;
			}
		}

		[Serializable]
		public class Value
		{
			public Transform spawnPos;

			public GameObject npc;
		}

		#endregion

		#region Main

		public Transform playerPos;

		List<Key> CurrentActiveKeys;
		Key currentQuestKey;

		List<GameObject> ActiveNpcs;

		[SerializeField]
		float minDistance = 20.0f;
		[SerializeField]
		float maxDistance = 100.0f;


		bool isNpcActivated;
		public  bool IsNpcActivated
		{
			get
			{
				return isNpcActivated;
			}
			set
			{
				isNpcActivated = value;

				if(isNpcActivated)
				{
					StartCoroutine("GetDofPlayerVUsingNpc");

					if (CurrentActiveKeys.Count > 1)
						StartCoroutine("GetDofPlayervUselessNpc");
				}
			}
		}

		private void Awake()
		{
			ActiveNpcs = new List<GameObject>();
			CurrentActiveKeys = new List<Key>();
		}
	#endregion


	#endregion


	#region Spawn Npc  


	void Start()
    {
		FindRightSpawnPosNActiveNpc();
	}


	public void FindRightSpawnPosNActiveNpc()
	{
		int questNum = DataManager.Instance.questNum;

		if(questNum <= DataManager.maxQuestNum)
		{
			foreach (var key in npcDictionary.Keys)
			{
				if (key.questNum == questNum)
				{
					currentQuestKey = key;
					CurrentActiveKeys.Add(currentQuestKey);
				}
			}

			SpawnNpc();
		}
	}

	void SpawnNpc()
	{
		Vector3 spawnPos;
		string spawnPosName = npcDictionary[currentQuestKey].spawnPos.gameObject.name;

		if(spawnPosName == "NearPlayer")
		{
			Vector3 nearPlayerPos = playerPos.position + playerPos.forward * 10;
			spawnPos = new Vector3(nearPlayerPos.x, 10, nearPlayerPos.z);
		}
		else
		{
			spawnPos = npcDictionary[currentQuestKey].spawnPos.transform.position;
		}

		GameObject spawnedNpc = Instantiate(npcDictionary[currentQuestKey].npc, spawnPos, transform.rotation, npcDictionary[currentQuestKey].spawnPos.transform) as GameObject;
		ActiveNpcs.Add(spawnedNpc);

		IsNpcActivated = true;
	}
	#endregion


	#region Check Distance of Npc and Player : Enable Touch & Delete Npc

	IEnumerator GetDofPlayerVUsingNpc()
	{
		float distance;
		Vector3 NpcPos = ActiveNpcs[ActiveNpcs.Count-1].transform.position;
		Npc recentActiveNpc = ActiveNpcs[ActiveNpcs.Count - 1].GetComponent<Npc>();
		//Debug.Log(recentActiveNpc);

		do
		{
			distance = Vector3.Distance(playerPos.position, NpcPos);

			//Debug.Log(NpcPos + ": " + distance);

			if (distance < minDistance)
			{
				if(!recentActiveNpc.IsTouchable)
					recentActiveNpc.IsTouchable = true;
			}
			else if (distance > minDistance)
			{
				if(recentActiveNpc.IsTouchable)
					recentActiveNpc.IsTouchable = false;
			}

			yield return new WaitForSecondsRealtime(1.0f);

		} while (!recentActiveNpc.IsClicked);

		yield return null;
	}


	IEnumerator GetDofPlayervUselessNpc()
	{
		float distance;
		Vector3 NpcPos;

		do
		{
			for (int i = 0; i < ActiveNpcs.Count-1; i++)
			{
				NpcPos = ActiveNpcs[i].transform.position;
				distance = Vector3.Distance(playerPos.position, NpcPos);

				if (distance > maxDistance)
				{
					ActiveNpcs[i].SetActive(false);
					ActiveNpcs.RemoveAt(i);
				}
			}

			yield return new WaitForSecondsRealtime(1.0f);

		} while (ActiveNpcs.Count < 2);

		yield return null;
	}

	#endregion

}
