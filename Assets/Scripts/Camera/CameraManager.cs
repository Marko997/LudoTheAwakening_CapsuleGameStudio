using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CameraType
{
    MainMenuCamera,
    OfflineCharacterSelectionCamera,
    LoadoutCamera,
	NoCamera
}
public class CameraManager : Singleton<CameraManager>
{
	List<CameraModel> cameraModelList;
	CameraModel lastActiveCamera;

	protected override void Awake()
	{
		base.Awake();
		cameraModelList = GetComponentsInChildren<CameraModel>().ToList();

		cameraModelList.ForEach(x => x.gameObject.SetActive(false));
		SwitchCamera(CameraType.MainMenuCamera);

	}

	public void SwitchCamera(CameraType _type)
	{
		if (lastActiveCamera != null)
		{
			lastActiveCamera.gameObject.SetActive(false);
		}
		CameraModel desiredCamera = cameraModelList.Find(x => x.cameraType == _type);

		if (desiredCamera != null)
		{
			desiredCamera.gameObject.SetActive(true);
			lastActiveCamera = desiredCamera;
		}
		else
		{
			Debug.LogWarning("The desired camera was not found");
		}
	}
}
