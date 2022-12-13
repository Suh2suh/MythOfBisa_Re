using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NameTagManager : MonoBehaviour
{
	#region Initialization

		[SerializeField]
		GameObject nametagPrefab;
		[SerializeField]
		GameObject nametagFolder;

		Camera cam;

		Dictionary<Transform, GameObject> NpcHeadToTag;
		Dictionary<Transform, GameObject> ActiveNeededTags;

		private void Awake()
		{
			cam = transform.GetComponent<Camera>();

			NpcHeadToTag = new Dictionary<Transform, GameObject>();
			ActiveNeededTags = new Dictionary<Transform, GameObject>();
		}

	#endregion


	#region On/Off NameTags
		
		private void Update()
		{
			if(ActiveNeededTags.Count > 0)
			{
				foreach(var head in ActiveNeededTags.Keys.ToList())
				{
					var screenPos = cam.WorldToScreenPoint(head.position);
					var isInScreen = (screenPos.z >= 0);

				    if(isInScreen)
					{
						if(GameManager.currentGameMode == GameManager.GameMode.FieldMode)
						{
							ActiveNeededTags[head].transform.position = screenPos;
							//Debug.Log(head);
							//break;
							continue;
						}
						else
						{
							InactiveNametag(head);
						}
					}
					else
					{
						InactiveNametag(head);
					}
				}
			}
		}

		public void AddNpcHead(Transform npcHead, string name)
		{
			if(!NpcHeadToTag.ContainsKey(npcHead))
			{
				var npcNameTag = Instantiate(nametagPrefab, npcHead.position, Quaternion.identity) as GameObject;

				npcNameTag.transform.SetParent(nametagFolder.transform, false);
				var nameTagText = npcNameTag.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
				nameTagText.text = ChangeNameTagText(name);

				NpcHeadToTag.Add(npcHead, npcNameTag);
				ActiveNeededTags.Add(npcHead, NpcHeadToTag[npcHead]);
			}
			else
			{
				if(!ActiveNeededTags.ContainsKey(npcHead))
				{
					NpcHeadToTag[npcHead].gameObject.SetActive(true);

					ActiveNeededTags.Add(npcHead, NpcHeadToTag[npcHead]);
				}
			}
		}
		public void InactiveNametag(Transform npcHead)
		{
			if(ActiveNeededTags.ContainsKey(npcHead))
			{
				NpcHeadToTag[npcHead].gameObject.SetActive(false);

				ActiveNeededTags.Remove(npcHead);
			}
		}


		string ChangeNameTagText(string name)
		{
			//TODO: �� �� ȿ������ ���� ã�� (switch�� �ٲ� ���)
			//TODO: ���� �����������? -> npc ��ũ��Ʈ�� üũ�ϴ� ���� �ƴ϶� ���̾ üũ�ϴ� ������� �ٲ�� �ϳ�?
			if (name.Contains("Smith")) name = "���&���̽� Bisa&Smith";
			else if (name.Contains("Adam")) name = "�ִ� Adam";
			else if (name.Contains("Will")) name = "�� Will";
			else if (name.Contains("Bell")) name = "�� Bell";
			else if (name.Contains("Susan")) name = "���� Susan";
			else if (name.Contains("Bisa")) name = "��� Bisa";
			else if (name.Contains("Mitty")) name = "��Ƽ Mitty";
			else if (name.Contains("Damon")) name = "���̸� Damon";

			return name;
		}

	#endregion
}
