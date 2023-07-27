using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
	#region Initialization

		bool isTouchable = false;
		bool isClicked = false;

		Outline outliner;

		public bool IsTouchable
		{
			get
			{
				return isTouchable;
			}
			set
			{
				isTouchable = value;
				//Debug.Log(transform.name + " Touchable: " + isTouchable);

				if(isTouchable)
					TouchOnScreen.isTouchDetectNeeded = true;
				else
					TouchOnScreen.isTouchDetectNeeded = false;
			}
		}
		public bool IsClicked
		{
			get { return isClicked; }
			set { isClicked = value; }
		}


		private void Start()
		{
			outliner = gameObject.GetComponent<Outline>();
		}

	#endregion


	#region Clicked in Screen / Quest Process

		public void ClickedOnce()
		{
			IsClicked = true;
			ShowOutline();
		}

		//나중에 performance 차이 비교 후 ->
		//npcManager에 두번 클릭된 Npc 전달
		//dialoigueManager에 대화 번호 전달
		public void ClickedTwice()
		{
			IsTouchable = false;
			HideOutline();


			//TODO: questNum -> 굳이 선언할 필요 X
			//            StartDialogue() -> NPC 자체 선언하는 것 vs NPCManager에서 전체 관리
			int questNum = DataManager.Instance.questNum;

			Transform npcCameraHead = transform.Find("CameraHead");
			transform.root.GetComponent<NpcManager>().StartDialogue(npcCameraHead, questNum);
		}

		public void CancleTouched()
		{
			IsClicked = false;
			HideOutline();
		}

	#endregion

	#region OutLine Control

		public void ShowOutline()
		{
			outliner.OutlineMode = Outline.Mode.OutlineVisible;
		}
		public void HideOutline()
		{
			outliner.OutlineMode = Outline.Mode.OutlineHidden;
		}

	#endregion

}
