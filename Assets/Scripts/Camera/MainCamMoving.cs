using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamMoving : MonoBehaviour
{
	#region Initialization

	    #region Variables

	    public Transform mainCamTransform;
        public Transform topViewTransform;
        public Transform playerViewTransform;

        [SerializeField]
        float speedOfConvert;

        float lerpPercent = 0f;

        bool isCameraConvertNeeded;

        //It prevents changing view mode when the converting is on
        bool isCameraConvertingOn;
        public bool IsCameraConvertingOn
	    {
            get { return isCameraConvertingOn; }
        }

	    public enum CameraMode
        {
            TopViewMode, PlayerViewMode, DialogueViewMode
        };

        CameraMode currentMode;
        CameraMode oldMode;

        #endregion

        public CameraMode CurrentMode
        {
            set
		    {
                oldMode = currentMode;
                currentMode = value;

                if (oldMode != currentMode)
			    {
                    switch (currentMode)
                    {
                        case CameraMode.TopViewMode:
                            if (mainCamTransform.parent != topViewTransform.transform)  mainCamTransform.parent = topViewTransform.transform;
                            GameManager.currentGameMode = GameManager.GameMode.UIMode;
                            break;

                        case CameraMode.PlayerViewMode:
                            if (mainCamTransform.parent != playerViewTransform.transform)  mainCamTransform.parent = playerViewTransform.transform;
                            break;

                        case CameraMode.DialogueViewMode:
                            GameManager.currentGameMode = GameManager.GameMode.DialogueMode;
                            break;

                        default:
                            break;
                    }

                    isCameraConvertNeeded = true;
                }
            }
	    }

	    private void Start()
	    {
            currentMode = CameraMode.PlayerViewMode;
        }

    #endregion

    private void Update()
	{
		if(isCameraConvertNeeded)
		{
			switch (currentMode)
            {
                case CameraMode.TopViewMode:
                    LerpTransformTo(topViewTransform);
                    break;

                case CameraMode.PlayerViewMode:
                    LerpTransformTo(playerViewTransform);
                    break;

                case CameraMode.DialogueViewMode:
                    //LerpTransformTo(dialogueViewTransform)
                    break;

                default:
                    break;
            }
        }
	}

    void LerpTransformTo(Transform lerpTransform)
	{
        //lerpPercent -> 0.001~ 1
        if (lerpPercent < speedOfConvert*2)
		{
            if (!isCameraConvertingOn)
                isCameraConvertingOn = true;

            mainCamTransform.rotation = Quaternion.Slerp(mainCamTransform.rotation, lerpTransform.rotation, lerpPercent);
            mainCamTransform.position = Vector3.Slerp(mainCamTransform.position, lerpTransform.position, lerpPercent);

            lerpPercent = Mathf.MoveTowards(lerpPercent, 1f, speedOfConvert * Time.deltaTime);
        }
        else
		{
            lerpPercent = 0.0f;

            if (currentMode == CameraMode.PlayerViewMode)
                GameManager.currentGameMode = GameManager.GameMode.FieldMode;

            isCameraConvertingOn = false;
            isCameraConvertNeeded = false;
        }

        //Debug.Log(lerpPercent);
    }
}
