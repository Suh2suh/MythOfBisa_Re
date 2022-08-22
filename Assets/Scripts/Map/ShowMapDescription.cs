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

	#region Searching Index
	// -1: Building Index 찾기 전 초기화 상태
	int buildingStartIndex = -1;
	int buildingEndIndex = -1;

	public int BuildingStartIndex
	{
		get { return buildingStartIndex;  }
		set { buildingStartIndex = value;  }
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
			Debug.Log("BStart: " + buildingStartIndex);
			Debug.Log("BEnd: " + buildingEndIndex);

			if (buildingStartIndex >= 0)
				ShowDescription();
		}
	}


	void SearchStartIndex(string buildingName)
	{
		buildingStartIndex = -1;

		for (int i = 0; i < lastIndex; i++)
		{
			if (kor_Description[i]["BuildingCode"].ToString() == buildingName)
			{
				buildingStartIndex = i;
				break;
			}
		}

		SearchEndIndex();
	}

	void SearchEndIndex()
	{
		buildingEndIndex = lastIndex;

		for (int i = buildingStartIndex+1; i <= lastIndex; i++)
		{
			if (kor_Description[i]["BuildingCode"].ToString().Length > 0)
			{
				buildingEndIndex = i-1;
				break;
			}
		}
	}

	#endregion


	public TextMeshProUGUI descriptionTitle;

	public GameObject descriptionContent;
	public GameObject MapFloorSection; // FloorText, ToiletImage
	public GameObject MapRoomText;
	public GameObject MapFloorDivider;

	List<GameObject> ContentChilds = new List<GameObject>();


	//Change Description With This
	GameObject floorSection;
	void ShowDescription()
	{
		InitializeContent();

		//Change Building Title
		descriptionTitle.text = kor_Description[buildingStartIndex]["Name"].ToString();

		Vector3 contentPos = descriptionContent.transform.position;
		Quaternion contentRot = descriptionContent.transform.rotation;

		for (int index = buildingStartIndex; index <= buildingEndIndex; index++)
		{
			//When new FloorSection is Needed
			if(kor_Description[index]["Floor"].ToString().Length > 0)
			{
				//1. Make Divider Between Floors
				if(index != buildingStartIndex)
				{
					GameObject Divider = Instantiate(MapFloorDivider) as GameObject;
					Divider.transform.SetParent(descriptionContent.transform, worldPositionStays:false);
					ContentChilds.Add(Divider);
				}

				//2. Create Floor Section
				floorSection = Instantiate(MapFloorSection) as GameObject;
				floorSection.transform.SetParent(descriptionContent.transform, worldPositionStays: false);
				ContentChilds.Add(floorSection);

				//3. Change Text of Building Floor
				TextMeshProUGUI floorText = floorSection.transform.Find("FloorText").GetComponent<TextMeshProUGUI>();
				floorText.text = kor_Description[index]["Floor"].ToString();

				//4. Check the RestRoom of the Floor and Change the Toilet Image
				//kor_Description[index]["RestRoom"].ToString();
			}

			//Add RoomText in Loop if the FloorSection is not null
			if(floorSection)
			{
				GameObject roomText = Instantiate(MapRoomText) as GameObject;
				roomText.transform.SetParent(floorSection.transform, worldPositionStays: false);

				string roomInfo = "(" + kor_Description[index]["Room"].ToString() + ")" 
					                     + " " + kor_Description[index]["Info"].ToString();
				roomText.GetComponent<TextMeshProUGUI>().text = roomInfo;
			}
		}
	}

	void InitializeContent()
	{
		for(int i = 0; i < ContentChilds.Count; i ++)
		{
			Destroy(ContentChilds[i].gameObject);
		}

		ContentChilds.Clear();
		floorSection = null;
	}
}
