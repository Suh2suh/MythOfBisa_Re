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
			//TODO: 좀 더 효율적인 구문 찾기 (switch로 바꿀 방법)
			//TODO: 만약 여러마리라면? -> npc 스크립트를 체크하는 것이 아니라 레이어를 체크하는 방식으로 바꿔야 하나?
			if (name.Contains("Smith")) name = "비사&스미스 Bisa&Smith";
			else if (name.Contains("Adam")) name = "애덤 Adam";
			else if (name.Contains("Will")) name = "윌 Will";
			else if (name.Contains("Bell")) name = "벨 Bell";
			else if (name.Contains("Susan")) name = "수잔 Susan";
			else if (name.Contains("Bisa")) name = "비사 Bisa";
			else if (name.Contains("Mitty")) name = "미티 Mitty";
			else if (name.Contains("Damon")) name = "데이먼 Damon";

			return name;
		}

	#endregion
}
