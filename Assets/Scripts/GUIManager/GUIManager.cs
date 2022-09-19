using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
	#region Initialization

		//For getting Off Map Pins in Back Button
		public MapPinManager mapPinManager;
		public MainCamMoving mainCamMover;


		public List<GameObject> uiStateList;

		[Serializable]
		public enum State
		{
			UsualState,
			MapState
		};


		public List<GameObject> mapUiList;

		[Serializable]
		public enum MapUIState
		{
			MapOpened,
			MapSearchOn,
			MapPinOn,
			InfoPanelOn
		};

	#endregion


	#region Game UI State

		State uiState;
		public State UIState
		{
			get
			{
				return uiState;
			}
			set
			{
				uiState = value;

				switch (uiState)
				{
					case State.UsualState:
						uiStateList[0].SetActive(true);
						uiStateList[1].SetActive(false);
						break;
					case State.MapState:
						uiStateList[1].SetActive(true);
						uiStateList[0].SetActive(false);
						MapState = MapUIState.MapOpened;
						mainCamMover.CurrentMode = MainCamMoving.CameraMode.TopViewMode;
						break;
				}
			}
		}

	#endregion


	#region Map State

		MapUIState mapState;
		public MapUIState MapState
		{
			get
			{
				return mapState;
			}
			set
			{
				mapState = value;
				//Debug.Log("맵스테이지 변경 TO: " + mapState);

				switch (mapState)
				{
					case MapUIState.MapOpened:
						/* Map is just Opened */
						mapUiList[0].SetActive(true);
						mapUiList[2].SetActive(false);
						break;
					case MapUIState.MapSearchOn:
						/* SearchOnBtn Clicked */
						mapUiList[2].SetActive(true);
						mapUiList[0].SetActive(false);
						break;
					case MapUIState.MapPinOn:
						/* One of SearchBtns Clicked */
						//MapPins are On
						mapUiList[2].SetActive(false);
						mapUiList[3].SetActive(false);
						break;
					case MapUIState.InfoPanelOn:
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

	#endregion


	#region UI State Methods

		public void SetUIState(GUIStateComponent StateComponent)
		{
			if(!mainCamMover.IsCameraConvertingOn)
				UIState = StateComponent.State;
		}
		public void SetMapState(GUIStateComponent StateComponent)
		{
			if (!mainCamMover.IsCameraConvertingOn)
				MapState = StateComponent.MapUIState;
		}

		public void BackInMap()
		{
			switch(mapState)
			{
				case MapUIState.MapOpened:
					/* Close the Map And Change Cam to Usual*/
					mainCamMover.CurrentMode = MainCamMoving.CameraMode.PlayerViewMode;
					UIState = State.UsualState;
					break;
				case MapUIState.MapSearchOn:
					/* Close SearchBtns */
					MapState = MapUIState.MapOpened;
					break;
				case MapUIState.MapPinOn:
					/* One of SearchBtns Clicked */
					mapPinManager.TakePinOffAll();
					MapState = MapUIState.MapSearchOn;
					break;
				case MapUIState.InfoPanelOn:
					/* One of MapPins Clicked */
					//Off Panels
					MapState = MapUIState.MapPinOn;
					break;
				default:
					Debug.Log("Please Check the MapStage Value before setting it");
					break;
			}
		}

	#endregion

}
