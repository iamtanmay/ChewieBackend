using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using RootMotion.Demos;
using RootMotion.Dynamics;

public class UMA_Puppetmaster_Character_Controller_Initialisation : MonoBehaviour
{
    public Transform characterController;

    public void Initialise()
    {
        CapsuleCollider capsuleCollider = transform.GetComponent<CapsuleCollider>();
        CapsuleCollider tempCollider = characterController.GetComponent<CapsuleCollider>();
        UMAData umaData = transform.GetComponent<UMAData>();
        tempCollider.radius = umaData.characterRadius;
        tempCollider.height = umaData.characterHeight;
        tempCollider.center = new Vector3(0, tempCollider.height / 2, 0);


        capsuleCollider.enabled = false;
        transform.gameObject.name = "Animation Controller";
        characterController.parent = transform.parent;
        //characterController.position = new Vector3();
        //characterController.rotation = new Quaternion();
        characterController.gameObject.SetActive(true);
        transform.parent = characterController;
    }
}
