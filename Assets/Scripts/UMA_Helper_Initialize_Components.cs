using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMA_Helper_Initialize_Components : MonoBehaviour
{
    public List<MonoBehaviour> scriptsToInitialise;

    // Start is called before the first frame update
    public void InitialiseScripts()
    {
        foreach (MonoBehaviour script in scriptsToInitialise)
            script.enabled = true;
    }
}
