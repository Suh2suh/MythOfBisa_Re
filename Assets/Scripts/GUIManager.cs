using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
	[Tooltip("0: UsualState, 1: MapState")]
	public List<GameObject> uiStateList;

	[Tooltip("0: SearchOnBtn, 1: BackBtn, 2: SearchBtns, 3: InfoPanel")]
	public List<GameObject> mapUiList;

	//For getting Off Map Pins in Back Button
	public MapPinManager mapPinManager;
	public MainCamMoving mainCamMover;


	string uiState;
	public string UiState
	{
		get
		{
			return uiState;
		}
		set
		{
			uiState = value;

			switch(uiState)
			{
				case "usual":
				case "Usual":
					uiStateList[0].SetActive(true);
					uiStateList[1].SetActive(false);
					break;
				case "map":
				case "Map":
					uiStateList[1].SetActive(true);
					uiStateList[0].SetActive(false);

					mapStage = 0;
					break;
			}
		}
	}


	#region MapSection

	int mapStage;
	public int MapStage
	{
		get
		{
			return mapStage;
		}
		set
		{
			mapStage = value;
			Debug.Log("맵스테이지 변경 TO: " + mapStage);

			switch (mapStage)
			{
				case 0:
					/* Map is just Opened */
					mapUiList[0].SetActive(true);
					mapUiList[2].SetActive(false);
					break;
				case 1:
					/* SearchOnBtn Clicked */
					mapUiList[2].SetActive(true);
					mapUiList[0].SetActive(false);
					break;
				case 2:
					/* One of SearchBtns Clicked */
					//MapPins are On
					mapUiList[2].SetActive(false);
					break;
				case 3:
					/* One of MapPins Clicked */
					//InfoPanel is On
					mapUiList[3].SetActive(true);
					break;
				default:
					Debug.Log("Please Check the MapStage Value before setting it");
					break;
			}
		}
	}

	public void BackInMap()
	{
		switch(mapStage)
		{
			case 0:
				/* Close the Map And Change Cam to Usual*/
				mainCamMover.OnOffTopViewMode();
				UiState = "usual";
				break;
			case 1:
				/* Close SearchBtns */
				mapUiList[0].SetActive(true);
				mapUiList[2].SetActive(false);
				mapStage = 0;
				break;
			case 2:
				/* One of SearchBtns Clicked */
				mapPinManager.TakePinOffAll();
				mapUiList[2].SetActive(true);
				mapStage = 1;
				break;
			case 3:
				/* One of MapPins Clicked */
				//Off Panels
				mapUiList[3].SetActive(false);
				mapStage = 2;
				break;
			default:
				Debug.Log("Please Check the MapStage Value before setting it");
				break;
		}
	}
	#endregion
}
