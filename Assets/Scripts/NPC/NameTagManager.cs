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
			//TODO: �� �� ȿ������ ���� ã�� (switch�� �ٲ� ���)
			//TODO: ���� �����������? -> npc ��ũ��Ʈ�� üũ�ϴ� ���� �ƴ϶� ���̾ üũ�ϴ� ������� �ٲ�� �ϳ�?
			if (name.Contains("Smith"))				name = "���&���̽� Bisa&Smith";
			else if (name.Contains("Adam"))		name = "�ִ� Adam";
			else if (name.Contains("Will"))			name = "�� Will";
			else if (name.Contains("Bell"))			name = "�� Bell";
			else if (name.Contains("Susan"))	name = "���� Susan";
			else if (name.Contains("Bisa"))		name = "��� Bisa";
			else if (name.Contains("Mitty"))		name = "��Ƽ Mitty";
			else if (name.Contains("Damon"))	name = "���̸� Damon";

			return name;
		}

	#endregion
}
