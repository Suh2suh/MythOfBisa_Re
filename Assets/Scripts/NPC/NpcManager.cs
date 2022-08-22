using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
	#region Initialization

	Transform playerPos;

	List<NpcSpawner> NpcSpawnPoses;
	List<NpcSpawner> CurrentActivePos;

	float minDistance = 50.0f;


	bool isNpcActivated;
    public bool IsNpcActivated
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
				StartCoroutine("GetDofPlayerVNpc");
			}
		}
	}

	//GameData 퀘스트 번호 변경됨 -> 새로운 퀘스트 인식
	//새로운 퀘스트 -> npc 서서히 사라짐


	private void Awake()
	{
		NpcSpawnPoses = new List<NpcSpawner>();
		CurrentActivePos = new List<NpcSpawner>();

		for (int i = 0; i < transform.childCount-1; i++)
		{
			NpcSpawner ns = transform.GetChild(i).gameObject.GetComponent<NpcSpawner>();
			NpcSpawnPoses.Add(ns);
		}
    }
	#endregion

	//Find Quest Position when Start at first
	void Start()
    {
		FindRightSpawnPosNActiveNpc();
	}


	void FindRightSpawnPosNActiveNpc()
	{
		foreach (NpcSpawner ns in NpcSpawnPoses)
		{
			if (ns.questNum == GameData.questNum)
			{
				CurrentActivePos.Add(ns);
			}
		}

		//Count - 1 => 이미 활성화된 Npc가 있을 수 있기 때문에, 새롭게 들어갈 Npc만 스폰
		if(CurrentActivePos.Count > 0)
			CurrentActivePos[CurrentActivePos.Count-1].SpawnNpc();
	}


	// CurrentActivePos[CurrentActivePos.Count - 1].questNum != GameData.QuestNum -> 거리 체크 후 멀어지면 Npc 제거
	//turon on screen touch if the distance is close enough
	IEnumerator GetDofPlayerVNpc()
	{
		//제일 최근에 생성된 npc의 퀘스트넘버가 현재 퀘스트넘버와 일치한다면

		if(CurrentActivePos[CurrentActivePos.Count - 1].questNum == GameData.questNum)
		{
			float distance;
			Vector3 NpcPos = CurrentActivePos[CurrentActivePos.Count - 1].GetSpawnPos();

			do
			{
				distance = Vector3.Distance(playerPos.position, NpcPos);

				yield return new WaitForSecondsRealtime(1.0f);
			} while (distance > minDistance);

			//충분히 가까워지면
			TouchOnScreen.isTouchDetectNeeded = true;
		}
	}

	IEnumerator GetDofPlayervOtherNpc()
	{
		if(CurrentActivePos.Count > 1)
		{
			float distance;
			Vector3 NpcPos;

			do
			{
				for(int i = 0; i <= CurrentActivePos.Count - 2; i++)
				{
					NpcPos = CurrentActivePos[i].GetSpawnPos();
					distance = Vector3.Distance(playerPos.position, NpcPos);

					if (distance < minDistance)
						CurrentActivePos[i].DeleteNpc();
				}

				yield return new WaitForSecondsRealtime(1.0f);
			} while (CurrentActivePos.Count < 2);
		}
	}

	//when Npc Clicked, make bool false


}
