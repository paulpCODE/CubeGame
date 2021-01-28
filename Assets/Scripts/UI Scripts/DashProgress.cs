using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class DashProgress : MonoBehaviour
{
    [SerializeField] private ProgressBar dashProgressBar;
    [SerializeField] private GameObject parentTarget;
    private void Awake()
    {
        dashProgressBar = gameObject.GetComponent<ProgressBar>();
    } 
    private void OnEnable()
    {
        playercontroller.OnCanDashChanged += UpdateProgress;
        playercontroller.OnAxisChanged += ChangeDirection;
    }
    private void OnDisable()
    {
        playercontroller.OnCanDashChanged -= UpdateProgress;
        playercontroller.OnAxisChanged -= ChangeDirection;
    }
    private void UpdateProgress(bool value, float secondsToReload)
    {
       
        if(value == false)
        {
            dashProgressBar.currentPercent = 0;
        }
        dashProgressBar.maxValue = 100;
        dashProgressBar.speed =  (int)(100/secondsToReload);

    }
    
    private void ChangeDirection(sbyte direction)
    {
        if (direction == 1)
            parentTarget.transform.eulerAngles = new Vector3(0,0, 0);
        else
            parentTarget.transform.eulerAngles = new Vector3(0,-180,0);
    }
   
}
