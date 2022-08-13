using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMapDescription : MonoBehaviour
{
	#region Initialization

	// CSV File Read
	List<Dictionary<string, object>> kor_Description;
	private void Awake()
	{
		kor_Description = CSVReader.Read("Kor_UnivDescription_MOB_CSV");
	}

	// Last Index Check
	int lastIndex;
	private void Start()
	{
		lastIndex = kor_Description.Count - 1;

		//Debug.Log(lastIndex);
	}

	#endregion

	public TextMeshProUGUI descriptionTitle;
	public GameObject descriptionContent;

	// -1: Building Index 찾기 전 초기화 상태
	int startIndex = -1;
	public int StartIndex
	{
		get { return startIndex;  }
		set { startIndex = value;  }
	}

	string buildingName;
	public string BuildingName
	{
		get
		{
			return buildingName;
		}
		set
		{
			buildingName = value;
			SearchStartIndex(buildingName);
			Debug.Log(startIndex);

			if (startIndex >= 0)
				ShowDescription();
		}
	}

	void SearchStartIndex(string buildingName)
	{
		for (int i = 0; i < lastIndex; i++)
		{
			if (kor_Description[i]["BuildingCode"].ToString() == buildingName)
			{
				startIndex = i;
			}
		}
	}

	void ShowDescription()
	{
		string str = kor_Description[startIndex]["Name"].ToString();
		Debug.Log(str);
		descriptionTitle.text = str;
	}
}
