using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleScore : MonoBehaviour
{
    [SerializeField] GameObject scoreGUI;
    Text score;
    public int scoreVal;

    private void Start()
    {
        score = scoreGUI.GetComponent<Text>();
    }

    private void Update()
    {
        score.text = scoreVal.ToString();
    }

}
