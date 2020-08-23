using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

public class FlexibleInput_PlayerCamera : FlexibleBehaviourInput
{
    public int currentCameraIndex = 0;
    public bool orbitalMode = false;
    public float scrollAxis;
    public Transform[] camerasTransforms;
    public Transform orbitalCameraTransform;
    public UnityEvent orbitalCamEnable, orbitalCamDisable;
    public CinemachineVirtualCamera[] virtualCameras;
    public CinemachineTransposer[] virtualCameraTransposers;
    public CinemachineTransposer orbitalCameraTransposer;
    public float zoomSpeed, zoom, min_zoom, max_zoom;
    public float h, v;

    public override void AdditionalInit()
    {
    }

    public void Awake()
    {
        virtualCameraTransposers = new CinemachineTransposer[camerasTransforms.Length];
        virtualCameras = new CinemachineVirtualCamera[camerasTransforms.Length];

        for (int i = 0; i < camerasTransforms.Length; i++)
        {
            virtualCameras[i] = camerasTransforms[i].GetComponent<CinemachineVirtualCamera>();
            virtualCameraTransposers[i] = virtualCameras[i].GetCinemachineComponent<CinemachineTransposer>();
            camerasTransforms[i].gameObject.SetActive(false);
        }

        orbitalCameraTransposer = orbitalCameraTransform.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        camerasTransforms[currentCameraIndex].gameObject.SetActive(true);
    }


    public override void UpdateInputs()
    {
        scrollAxis = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_AxisScroll].axisValue;
        h = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_Axis11].axisValue;
        v = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_Axis12].axisValue;
    }

    public void SwitchCamera()
    {
        int oldIndex = currentCameraIndex;

        if (!orbitalMode)
        {
            if (currentCameraIndex == 0)
            {
                if (zoom < max_zoom)
                {
                    currentCameraIndex = 1;
                }
            }
            else
            {
                if (zoom >= max_zoom)
                {
                    currentCameraIndex = 0;
                }
            }

            if (oldIndex != currentCameraIndex)
            {
                camerasTransforms[oldIndex].gameObject.SetActive(false);
                camerasTransforms[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }

    public override void Enable()
    {
        this.enabled = true;
    }

    public override void Disable()
    {
        this.enabled = false;
    }

    public override void BehaviourUpdate()
    {
        if (h == 0f && v == 0f && currentCameraIndex != 0)
        {
            //orbitalMode = true;

            orbitalCameraTransform.gameObject.SetActive(true);

            for (int i = 0; i < camerasTransforms.Length; i++)
                camerasTransforms[i].gameObject.SetActive(false);

            orbitalCamEnable.Invoke();
        }
        else
        {
            orbitalCameraTransform.gameObject.SetActive(false);
            camerasTransforms[currentCameraIndex].gameObject.SetActive(true);

            orbitalCamDisable.Invoke();
        }

        zoom = zoom + scrollAxis * zoomSpeed * Time.deltaTime;

        SwitchCamera();

        zoom = Mathf.Clamp(zoom, min_zoom, max_zoom);

        if (orbitalMode)
            orbitalCameraTransposer.m_FollowOffset.z = zoom;
        else if (currentCameraIndex != 0)
            virtualCameraTransposers[currentCameraIndex].m_FollowOffset.z = zoom;

        orbitalMode = mgr.getOrbitalCamBool();
    }
}