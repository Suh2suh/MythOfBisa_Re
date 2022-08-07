using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinManager : MonoBehaviour
{
    List<List<MapPinCreater>> ActiveMapPinList = new List<List<MapPinCreater>>();

    void MakePinOnCategory(List<MapPinCreater> category)
    {
        if (ActiveMapPinList.Count > 0)
            TakePinOffAll();

        ActiveMapPinList.Add(category);

        /* Pin is On -> Touch Detect Needed */
        TouchOnScreen.isTouchDetectNeeded = true;

        foreach (MapPinCreater mapPinCreater in ActiveMapPinList[0])
        {
            mapPinCreater.IsPinNeeded = true;
        }
    }
    public void TakePinOffAll()
	{
        foreach (List<MapPinCreater> SearchedCategory in ActiveMapPinList)
		{
            foreach (MapPinCreater mapPinCreater in SearchedCategory)
            {
                mapPinCreater.IsPinNeeded = false;
            }
        }

        ActiveMapPinList.Clear();
        /* Pin is Off -> Touch Detect Needed */
        TouchOnScreen.isTouchDetectNeeded = false;
    }


	#region MapCategories
	public List<MapPinCreater> Universities;
	public List<MapPinCreater> GraduateSchools;
	public List<MapPinCreater> Laboratories;
	public List<MapPinCreater> ParkingLots;
	public List<MapPinCreater> Organizations;
	public List<MapPinCreater> SmokingAreas;
	public List<MapPinCreater> ScooterAreas;
	public List<MapPinCreater> CarChargers;

    public void MakePinOnUniversity()
    {
        MakePinOnCategory(Universities);
    }
    public void MakePinOnGraduateSchool()
    {
        MakePinOnCategory(GraduateSchools);
    }    
    public void MakePinOnLaboratory()
    {
        MakePinOnCategory(Laboratories);
    }
    public void MakePinOnParkingLot()
    {
        MakePinOnCategory(ParkingLots);
    }
    public void MakePinOnOrganization()
    {
        MakePinOnCategory(Organizations);
    }
    public void MakePinOnSmokingArea()
    {
        MakePinOnCategory(SmokingAreas);
    }
    public void MakePinOnScooterArea()
    {
        MakePinOnCategory(ScooterAreas);
    }
    public void MakePinOnCarCharger()
    {
        MakePinOnCategory(CarChargers);
    }
    #endregion
}
