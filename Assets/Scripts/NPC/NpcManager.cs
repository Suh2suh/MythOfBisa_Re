using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
	//GameData 퀘스트 번호 변경됨 -> 새로운 퀘스트 인식
	//새로운 퀘스트 -> npc 서서히 사라짐

	#region Initialization


	public Transform playerPos;

	List<NpcSpawner> NpcSpawnPoses;
	List<NpcSpawner> CurrentActivePos;


	[SerializeField]
	float minDistance = 20.0f;
	[SerializeField]
	float maxDistance = 100.0f;


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
				StartCoroutine("GetDofPlayerVUsingNpc");

				if (CurrentActivePos.Count > 1)
					StartCoroutine("GetDofPlayervUselessNpc");
			}
		}
	}


	private void Awake()
	{
		NpcSpawnPoses = new List<NpcSpawner>();
		CurrentActivePos = new List<NpcSpawner>();

		for (int i = 0; i < transform.childCount; i++)
		{
			NpcSpawner ns = transform.GetChild(i).gameObject.GetComponent<NpcSpawner>();
			NpcSpawnPoses.Add(ns);

			Debug.Log(ns);
		}
    }


	#endregion


	#region Spawn Npc  


	void Start()
    {
		FindRightSpawnPosNActiveNpc();
	}


	public void FindRightSpawnPosNActiveNpc()
	{
		foreach (NpcSpawner ns in NpcSpawnPoses)
		{
			if (ns.questNum == DataManager.Instance.questNum)
			{
				CurrentActivePos.Add(ns);
			}
		}

		//Count - 1 => 이미 활성화된 Npc가 있을 수 있기 때문에, 새롭게 들어갈 Npc만 스폰
		if(DataManager.Instance.questNum != 0)
			CurrentActivePos[CurrentActivePos.Count - 1].SpawnNpc();
		else
			SpawnNpcNearPlayer();


		if (!IsNpcActivated)
			IsNpcActivated = true;
	}

	void SpawnNpcNearPlayer()
	{
		NpcSpawner nearNpcPos = NpcSpawnPoses[NpcSpawnPoses.Count-1];

		Vector3 nearPlayerPos = playerPos.position + playerPos.forward * 10;
		nearNpcPos.SpawnPos = new Vector3(nearPlayerPos.x, nearNpcPos.transform.position.y, nearPlayerPos.z);

		nearNpcPos.SpawnNpc();
	}


	#endregion


	#region Check Distance of Npc and Player : Enable Touch & Delete Npc


	NpcSpawner recentActivePos;

	IEnumerator GetDofPlayerVUsingNpc()
	{
		recentActivePos = CurrentActivePos[CurrentActivePos.Count - 1];

		float distance;
		Vector3 NpcPos = CurrentActivePos[CurrentActivePos.Count - 1].SpawnPos;

		do
		{
			distance = Vector3.Distance(playerPos.position, NpcPos);

			if (distance < minDistance)
				recentActivePos.IsTouchable = true;
			else if (distance > minDistance)
				recentActivePos.IsTouchable = false;


			yield return new WaitForSecondsRealtime(1.0f);

		} while (!recentActivePos.isClicked);
	}


	IEnumerator GetDofPlayervUselessNpc()
	{
		float distance;
		Vector3 NpcPos;

		do
		{
			for (int i = 0; i <= CurrentActivePos.Count - 2; i++)
			{
				NpcPos = CurrentActivePos[i].SpawnPos;
				distance = Vector3.Distance(playerPos.position, NpcPos);

				if (distance > maxDistance)
				{
					CurrentActivePos[i].DeleteNpc();
					CurrentActivePos.RemoveAt(i);
				}
			}

			yield return new WaitForSecondsRealtime(1.0f);
		} while (CurrentActivePos.Count < 2);
	}

	//when Npc Clicked, make bool false

	#endregion

}
