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

		//���߿� performance ���� �� �� ->
		//npcManager�� �ι� Ŭ���� Npc ����
		//dialoigueManager�� ��ȭ ��ȣ ����
		public void ClickedTwice()
		{
			IsTouchable = false;
			HideOutline();


			//TODO: questNum -> ���� ������ �ʿ� X
			//            StartDialogue() -> NPC ��ü �����ϴ� �� vs NPCManager���� ��ü ����
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
