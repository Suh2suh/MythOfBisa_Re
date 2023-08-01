using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NameTagManager : MonoBehaviour
{
	#region Initialization

		[SerializeField]
		GameObject nameTagPrefab;
		[SerializeField]
		GameObject nameTagFolder;

		Camera cam;

		Dictionary<Transform, GameObject> NameTagsPerNpc;
		List<Transform> TagOnNpcs;

		private void Awake()
		{
			cam = transform.GetComponent<Camera>();

			NameTagsPerNpc = new Dictionary<Transform, GameObject>();
			TagOnNpcs = new List<Transform>();
		}

	#endregion


	#region On/Off NameTags
		
		private void Update()
		{
			if(TagOnNpcs.Count > 0)
			{
				foreach(var npcTagTransform in TagOnNpcs)
				{
					var screenPos = cam.WorldToScreenPoint(npcTagTransform.position);
					var isNameTagInScreen = (screenPos.z >= 0);

				    if(isNameTagInScreen)
					{
						if(GameManager.currentGameMode == GameManager.GameMode.FieldMode)
							NameTagsPerNpc[npcTagTransform].transform.position = screenPos;

						else
							InActivateNameTag(npcTagTransform);
					}
					else
					{
						InActivateNameTag(npcTagTransform);
					}
				}
			}
		}

		public void ActivateNameTag(Transform nameTagTransform, string name)
		{
			if(!NameTagsPerNpc.ContainsKey(nameTagTransform))
			{
				var npcNameTag = Instantiate(nameTagPrefab, nameTagTransform.position, Quaternion.identity) as GameObject;
				npcNameTag.transform.SetParent(nameTagFolder.transform, false);

				var nameTagText = npcNameTag.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
				nameTagText.text = ChangeNameTagText(name);


				NameTagsPerNpc.Add(nameTagTransform, npcNameTag);
				TagOnNpcs.Add(nameTagTransform);
			}
			else
			{
				if(!TagOnNpcs.Contains(nameTagTransform))
				{
					NameTagsPerNpc[nameTagTransform].gameObject.SetActive(true);

					TagOnNpcs.Add(nameTagTransform);
				}
			}
		}
		public void InActivateNameTag(Transform nameTagTransform)
		{
			if(TagOnNpcs.Contains(nameTagTransform))
			{
				NameTagsPerNpc[nameTagTransform].gameObject.SetActive(false);

				TagOnNpcs.Remove(nameTagTransform);
			}
		}


		string ChangeNameTagText(string name)
		{
			//TODO: 좀 더 효율적인 구문 찾기 (switch로 바꿀 방법)
			//TODO: 만약 여러마리라면? -> npc 스크립트를 체크하는 것이 아니라 레이어를 체크하는 방식으로 바꿔야 하나?
			if (name.Contains("Smith"))				name = "비사&스미스 Bisa&Smith";
			else if (name.Contains("Adam"))		name = "애덤 Adam";
			else if (name.Contains("Will"))			name = "윌 Will";
			else if (name.Contains("Bell"))			name = "벨 Bell";
			else if (name.Contains("Susan"))	name = "수잔 Susan";
			else if (name.Contains("Bisa"))		name = "비사 Bisa";
			else if (name.Contains("Mitty"))		name = "미티 Mitty";
			else if (name.Contains("Damon"))	name = "데이먼 Damon";

			return name;
		}

	#endregion
}
