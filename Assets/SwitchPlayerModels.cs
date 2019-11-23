using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerModels : MonoBehaviour
{
    public GameObject model3D;
    public GameObject model2D;

    public void ModelSwitch()
    {
        if (model3D.activeSelf)
        {
            model3D.SetActive(false);
            model2D.SetActive(true);
        }
        else
        {
            model3D.SetActive(true);
            model2D.SetActive(false);
        }
    }
}
