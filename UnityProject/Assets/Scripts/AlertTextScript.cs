using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertTextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alertText;
    // Start is called before the first frame update
    void Start()
    {
        RunPythonScript.Instance.OnHeartRateTooHigh += RunPythonScript_OnHeartRateTooHigh;
        RunPythonScript.Instance.OnHeartRateSpike += RunPythonScript_OnHeartRateSpike;
    }

    private void RunPythonScript_OnHeartRateSpike(object sender, EventArgs e){
        alertText.text = "Alert: Heart rate spike detected!";
        StartCoroutine(WaitAndClearAlertText());
    }

    private void RunPythonScript_OnHeartRateTooHigh(object sender, EventArgs e){
        alertText.text = "Alert: Heart rate is too high!";
        StartCoroutine(WaitAndClearAlertText());
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
