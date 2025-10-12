using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class AFX_TimeSpeed : MonoBehaviour
{
    public float timeSpeed = 1;
    void Start()
    {
        Time.timeScale = timeSpeed;
    }
    void Update()
    {
        Time.timeScale = timeSpeed;
    }
    private void OnEnable()
    {
        Time.timeScale = timeSpeed;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
