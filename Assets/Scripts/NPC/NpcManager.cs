using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			public Transform npcSpawnPos;

			public GameObject npc;
		}

		#endregion

		#region Main

		public Transform playerPos;

		//List<Key> CurrentActiveKeys;
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
		float npcTouchableDistance = 20.0f;
		[SerializeField]
		float npcDeleteDistance = 100.0f;
		[SerializeField]
		float nameTagOnDistance = 35.0f;


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
			//CurrentActiveKeys = new List<Key>();
		}
	#endregion


	#endregion


	#region Spawn Npc  


	#region FindRightSpawnPosNActiveNpc()를 씬이 로딩될 때마다 호출

	/// <summary>
	/// 과거에는 씬마다 호출되는 함수를 관리하기 위해 OnSceneLoaded()를 사용했었지만,
	/// 최근에는 OnSceneLoaded Delegate Chain을 통해 관리하는 것을 권장
	/// </summary>
	/// 
	void OnEnable()
	{
		// 씬 매니저의 sceneLoaded에 체인을 건다.
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// 체인을 걸어서 이 함수는 매 씬마다 호출된다.
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("OnSceneLoaded: " + scene.name);
		FindRightSpawnPosNActiveNpc();
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	#endregion

	public void FindRightSpawnPosNActiveNpc()
	{
		int questNum = DataManager.Instance.questNum;

		//TODO: 굳이 if/else 없어도 됨 -> DataManager에서 Save 시 예외처리 되어있음
		//            삭제해도 될 지 체크
		if(questNum <= DataManager.maxQuestNum)
		{
			foreach (var key in npcDictionary.Keys)
			{
				if (key.questNum == questNum)
				{
					currentQuestKey = key;
					//CurrentActiveKeys.Add(currentQuestKey);
				}
			}

			SpawnNpc();
		}
		else
		{
			Debug.Log("퀘스트 키가 유효하지 않음: MaxQuestNum 초과");
		}
	}

	void SpawnNpc()
	{
		Value currentQuest = npcDictionary[currentQuestKey];
		string spawnPosName = currentQuest.npcSpawnPos.gameObject.name;

		Vector3 spawnPos;

		if (spawnPosName == "NearPlayer")
		{
			Vector3 nearPlayerPos = playerPos.position + playerPos.forward * 10;
			spawnPos = new Vector3(nearPlayerPos.x, 10, nearPlayerPos.z);
		}
		else
		{
			spawnPos = currentQuest.npcSpawnPos.transform.position;
		}

		GameObject spawnedNpc = Instantiate(currentQuest.npc, spawnPos, currentQuest.npcSpawnPos.rotation,
			                                                            currentQuest.npcSpawnPos.transform) as GameObject;
		ActiveNpcs.Add(spawnedNpc);


		//Debug.Log("NPC Spawned");
		IsNpcActivated = true;

	}
	#endregion


	#region Check Distance of Npc and Player : Enable Touch & Delete Npc

	IEnumerator GetDofPlayerVNpc()
	{
		float distance;
		Vector3 NpcPos;
		Transform nameTagTransform;


		while (isNpcActivated)
		{
			for (int i = 0; i < ActiveNpcs.Count; i++)
			{
				if (GameManager.currentGameMode == GameManager.GameMode.FieldMode)
				{
					NpcPos = ActiveNpcs[i].transform.position;
					distance = Vector3.Distance(playerPos.position, NpcPos);
					nameTagTransform = ActiveNpcs[i].transform.Find("NpcTagHead");


					//활성화된 NPC 공통 -> 거리에 따른 명찰 ON/OFF
					if (distance < nameTagOnDistance)
						NameTagManager.ActivateNameTag(nameTagTransform, activeNpcs[i].name);
					else
						NameTagManager.InActivateNameTag(nameTagTransform);
					//TODO: Send npc name by switch block check (not gameobject name)


					//최근 npc면 -> 대화 체크
					if (i == ActiveNpcs.Count - 1)
					{
						Npc recentActiveNpc = ActiveNpcs[i].GetComponentInChildren<Npc>();

						if (distance > npcTouchableDistance)
						{
							if (recentActiveNpc.IsTouchable)		recentActiveNpc.IsTouchable = false;
						}
						else
						{
							if (!recentActiveNpc.IsTouchable)	recentActiveNpc.IsTouchable = true;
						}
					}
					//이미 쓰인 npc면 -> 멀어지면 삭제
					else
					{
						if (distance > npcDeleteDistance)
						{
							ActiveNpcs[i].SetActive(false);
							ActiveNpcs.RemoveAt(i);
						}
					}

				}

				yield return new WaitForSecondsRealtime(0.5f);
			}
		}


		yield return null;
	}

	#endregion


	#region Start Dialogue of Npc
	//NPC가 두 번 클릭됐을 때, 대화 시작
	//다이얼로그 매니저로 옮겨도 괜찮을 듯
	public void StartDialogue(Transform npcHead, int questNum)
	{
		CamMover.WatchNpc(npcHead);

		//TODO: Couroutine으로 지속적으로 WatchNPC 끝났는지 체크, 이후 Start Dialouge OR Delegate 사용해보기
		DialogueManager.StartDialogue(questNum);
	}

	//대화 끝낼 때, 카메라를 플레이어 모드로 복귀
	//카메라 매니저로 옮겨도 괜찮을 듯
	public void SpawnNextNpcNWatchPlayer()
	{
		IsNpcActivated = false;
		FindRightSpawnPosNActiveNpc();

		CamMover.CurrentMode = MainCamMoving.CameraMode.PlayerViewMode;
	}
	#endregion
}
