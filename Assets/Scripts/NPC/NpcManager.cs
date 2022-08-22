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

	//GameData ����Ʈ ��ȣ ����� -> ���ο� ����Ʈ �ν�
	//���ο� ����Ʈ -> npc ������ �����


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

		//Count - 1 => �̹� Ȱ��ȭ�� Npc�� ���� �� �ֱ� ������, ���Ӱ� �� Npc�� ����
		if(CurrentActivePos.Count > 0)
			CurrentActivePos[CurrentActivePos.Count-1].SpawnNpc();
	}


	// CurrentActivePos[CurrentActivePos.Count - 1].questNum != GameData.QuestNum -> �Ÿ� üũ �� �־����� Npc ����
	//turon on screen touch if the distance is close enough
	IEnumerator GetDofPlayerVNpc()
	{
		//���� �ֱٿ� ������ npc�� ����Ʈ�ѹ��� ���� ����Ʈ�ѹ��� ��ġ�Ѵٸ�

		if(CurrentActivePos[CurrentActivePos.Count - 1].questNum == GameData.questNum)
		{
			float distance;
			Vector3 NpcPos = CurrentActivePos[CurrentActivePos.Count - 1].GetSpawnPos();

			do
			{
				distance = Vector3.Distance(playerPos.position, NpcPos);

				yield return new WaitForSecondsRealtime(1.0f);
			} while (distance > minDistance);

			//����� ���������
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
