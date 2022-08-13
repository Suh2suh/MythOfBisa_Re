using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinCreater : MonoBehaviour
{
    public GameObject mapPinPrefab;
	Vector3 mapPinPos;
	GameObject mapPin;

	bool isPinNeeded;

	void Awake()
	{
		Quaternion newQuaternion = new Quaternion(Quaternion.identity.x, 180, Quaternion.identity.z, Quaternion.identity.z);
		mapPinPos = new Vector3(transform.position.x, 50, transform.position.z);
		//mapPin = Instantiate(mapPinPrefab, mapPinPos, Quaternion.identity);
		mapPin = Instantiate(mapPinPrefab, mapPinPos, newQuaternion);

		mapPin.transform.parent = gameObject.transform;

		mapPin.SetActive(false);
	}

	public bool IsPinNeeded
	{
		get
		{
            return isPinNeeded;
		}
		set
		{
            isPinNeeded = value;

            if(isPinNeeded)
			{
				turnPinOn();
			}
            else
			{
				turnPinOff();
			}
		}
	}

    void turnPinOn()
	{
		mapPin.SetActive(true);
	}
    void turnPinOff()
	{
		mapPin.SetActive(false);
	}

}
