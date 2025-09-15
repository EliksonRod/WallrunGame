using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BoostBar : MonoBehaviour
{
    public RectTransform uiBar;
    public PlayerMovement pm;

    float barWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barWidth = uiBar.anchorMax.x;
    }

    // Update is called once per frame
    void Update()
    {
        pm.BoostTimeLeft = ((pm.BoostTimeLeft > pm.Boost_Timer) ? pm.Boost_Timer : (pm.BoostTimeLeft < 0) ? 0 : pm.BoostTimeLeft);

        float x = ((pm.BoostTimeLeft * (100f / pm.Boost_Timer)) * (1f / barWidth)) / 100f;

        uiBar.anchorMax = new Vector2(x, uiBar.anchorMax.y);
    }
}
