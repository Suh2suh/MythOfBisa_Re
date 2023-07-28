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


		[Serializable]
		public enum State
		{
			UsualState,
			MapState,
			GameState,
			DialogueState
		};

		[Serializable]
		public enum MapUIState
		{
			MapOpened,
			MapSearchOn,
			MapPinOn,
			InfoPanelOn
		};

		public enum DialogueUIState
		{
			Usual,
			SelectEvent
		};


		public uiStateList UIStateList;
		[Serializable]
		public struct uiStateList
		{
			public GameObject UsualStateGUI;
			public GameObject MapStateGUI;
			public GameObject GameStateGUI;
			public GameObject DialogueStateGUI;

		}

		public mapUIList MapUIList;
		[Serializable]
		public struct mapUIList
		{
			public GameObject OnSearchBtn;
			public GameObject BackBtnInMap;
			public GameObject SearchBtn;
			public GameObject MapDescriptionPanel;

		}

		public dialogueUIList DialogueUIList;
		[Serializable]
		public struct dialogueUIList
		{
			public GameObject SelectEventPanel;

		}

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
						UIStateList.UsualStateGUI.SetActive(true);
						UIStateList.MapStateGUI.SetActive(false);
						UIStateList.GameStateGUI.SetActive(false);
						UIStateList.DialogueStateGUI.SetActive(false);

						mainCamMover.CurrentMode = MainCamMoving.CameraMode.PlayerViewMode;
						break;

					case State.MapState:
						UIStateList.MapStateGUI.SetActive(true);
						UIStateList.UsualStateGUI.SetActive(false);

						MapState = MapUIState.MapOpened;

						mainCamMover.CurrentMode = MainCamMoving.CameraMode.TopViewMode;
						break;

					case State.GameState:
						UIStateList.GameStateGUI.SetActive(true);
						UIStateList.UsualStateGUI.SetActive(false);

						mainCamMover.CurrentMode = MainCamMoving.CameraMode.TopViewMode;
						break;

					case State.DialogueState:
						UIStateList.DialogueStateGUI.SetActive(true);
						UIStateList.UsualStateGUI.SetActive(false);

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
						MapUIList.OnSearchBtn.SetActive(true);
						MapUIList.SearchBtn.SetActive(false);
						break;
					case MapUIState.MapSearchOn:
						/* SearchOnBtn Clicked */
						MapUIList.SearchBtn.SetActive(true);
						MapUIList.OnSearchBtn.SetActive(false);
					break;
					case MapUIState.MapPinOn:
						/* One of SearchBtns Clicked */
						//MapPins are On
						MapUIList.SearchBtn.SetActive(false);
						MapUIList.MapDescriptionPanel.SetActive(false);
					break;
					case MapUIState.InfoPanelOn:
						/* One of MapPins Clicked */
						//InfoPanel is On
						MapUIList.MapDescriptionPanel.SetActive(true);
					break;
					default:
						Debug.Log("Please Check the MapStage Value before setting it");
						break;
				}
			}
		}

	#endregion


	#region Dialogue State

		DialogueUIState dialogueState;
		public DialogueUIState DialogueState
		{
			get
			{
				return dialogueState;
			}
			set
			{
				dialogueState = value;

				switch (dialogueState)
				{
					case DialogueUIState.Usual:
						DialogueUIList.SelectEventPanel.SetActive(false);
						break;
					case DialogueUIState.SelectEvent:
						DialogueUIList.SelectEventPanel.SetActive(true);
						break;
					default:
						Debug.Log("DialogueStateError");
						break;
				}
			}
		}

	#endregion

	#region UI State Methods

		#region For Button in Inspector

			public void SetUIState(GUIStateComponent StateComponent)
				{
					if(!mainCamMover.IsCameraConvertingOn)
						UIState = StateComponent.State;
				}

				public void SetMapState(GUIStateComponent StateComponent)
				{
					//if (!mainCamMover.IsCameraConvertingOn)
						MapState = StateComponent.MapUIState;
				}

				public void BackInMap()
				{
					switch(mapState)
					{
						case MapUIState.MapOpened:
							/* Close the Map And Change Cam to Usual*/
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


		public void SetUIState(State NewState)
		{
			if (!mainCamMover.IsCameraConvertingOn)
				UIState = NewState;
		}

		public void SetDialogueState(DialogueUIState NewDialogueState)
		{
			if (UIState == State.DialogueState)
			{
				DialogueState = NewDialogueState;
			}
		}

	#endregion



}
