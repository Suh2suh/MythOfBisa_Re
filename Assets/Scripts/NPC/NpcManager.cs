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

		List<GameObject> activeNpcs;
		public List<GameObject> ActiveNpcs
		{
			get
			{
				return activeNpcs;
			}
		}

		[SerializeField]
		float minDistance = 20.0f;
		[SerializeField]
		float maxDistance = 100.0f;
		[SerializeField]
		float nametagDistance = 35.0f;


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
					StartCoroutine(GetDofPlayerVNpc());
				}
			}
		}

		[SerializeField]
		NameTagManager NameTagManager;
		[SerializeField]
		MainCamMoving CamMover;
		[SerializeField]
		DialogueManager DialogueManager;

	private void Awake()
		{
			activeNpcs = new List<GameObject>();
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

		GameObject spawnedNpc = Instantiate(npcDictionary[currentQuestKey].npc, spawnPos, npcDictionary[currentQuestKey].spawnPos.rotation, npcDictionary[currentQuestKey].spawnPos.transform) as GameObject;
		ActiveNpcs.Add(spawnedNpc);

		IsNpcActivated = true;
	}
	#endregion


	#region Check Distance of Npc and Player : Enable Touch & Delete Npc

	IEnumerator GetDofPlayerVNpc()
	{
		float distance;
		Vector3 NpcPos;

		do
		{
			for (int i = 0; i < ActiveNpcs.Count; i++)
			{
				if(GameManager.currentGameMode == GameManager.GameMode.FieldMode)
				{
					NpcPos = ActiveNpcs[i].transform.position;
					distance = Vector3.Distance(playerPos.position, NpcPos);

					//최근 npc면 -> 대화 체크
					if (i == ActiveNpcs.Count - 1)
					{
						Npc recentActiveNpc = ActiveNpcs[i].GetComponent<Npc>();

						if (distance < minDistance)
						{
							if (!recentActiveNpc.IsTouchable)
								recentActiveNpc.IsTouchable = true;
						}
						else if (distance > minDistance)
						{
							if (recentActiveNpc.IsTouchable)
								recentActiveNpc.IsTouchable = false;
						}
					}
					//이미 쓰인 npc -> 멀어지면 삭제
					else
					{
						if (distance > maxDistance)
						{
							ActiveNpcs[i].SetActive(false);
							ActiveNpcs.RemoveAt(i);
						}
					}

					//공통 -> 명찰 see unsee
					if (distance < nametagDistance)
					{
						Debug.Log(ActiveNpcs[i].name + " 명찰 거리");

						var nameTagTransform = ActiveNpcs[i].transform.Find("NpcTagHead");
						Debug.Log(nameTagTransform);

						//TODO: Send npc name by switch block check (not gameobject name)
						NameTagManager.AddNpcHead(nameTagTransform, activeNpcs[i].name);
					}
					else
					{
						Debug.Log(ActiveNpcs[i].name + " 명찰 해제");

						var nameTagTransform = ActiveNpcs[i].transform.Find("NpcTagHead");

						NameTagManager.InactiveNametag(nameTagTransform);
					}
				}
				yield return new WaitForSecondsRealtime(0.5f);
			}

		} while (isNpcActivated);

		yield return null;
	}

	#endregion


	#region Start Dialogue of Npc
	public void StartDialogue(Transform npcHead, int questNum)
	{
		CamMover.WatchNpc(npcHead);
		DialogueManager.StartDialogue(questNum);
	}
	public void SpawnNextNpcNWatchPlayer()
	{
		IsNpcActivated = false;
		FindRightSpawnPosNActiveNpc();

		CamMover.CurrentMode = MainCamMoving.CameraMode.PlayerViewMode;
	}
	#endregion
}
