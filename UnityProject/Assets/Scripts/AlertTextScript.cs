using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertTextScript : MonoBehaviour
{
    string textSet = "Alert:";
    [SerializeField] private TextMeshProUGUI alertText;
    // Start is called before the first frame update
    void Start()
    {
        RunPythonScript.Instance.OnHeartRateTooHigh += RunPythonScript_OnHeartRateTooHigh;
        RunPythonScript.Instance.OnHeartRateSpike += RunPythonScript_OnHeartRateSpike;
    }

    void Update(){
        alertText.text = textSet;
    }

    private void RunPythonScript_OnHeartRateSpike(object sender, EventArgs e){
        textSet = "Alert: Heart rate spike detected!";
        // StartCoroutine(WaitAndClearAlertText());
    }

    private void RunPythonScript_OnHeartRateTooHigh(object sender, EventArgs e){
        textSet = "Alert: Heart rate is too high!";
        // StartCoroutine(WaitAndClearAlertText());
    }

    IEnumerator WaitAndClearAlertText(){
        yield return new WaitForSeconds(2);
        alertText.text = "";
    }

    private void OnDestroy()
    {
        RunPythonScript.Instance.OnHeartRateTooHigh -= RunPythonScript_OnHeartRateTooHigh;
        RunPythonScript.Instance.OnHeartRateSpike -= RunPythonScript_OnHeartRateSpike;
    }
}
