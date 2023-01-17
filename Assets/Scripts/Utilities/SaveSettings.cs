using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSettings
{
    private static CameraStates cameraState = CameraStates.RtsCam;

    public static CameraStates CameraState
    {
        get { return cameraState; }
        set { cameraState = value; }
    }
}
