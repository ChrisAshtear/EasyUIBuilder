using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

class UIStatBar : MonoBehaviour
{
    IStats statData;
    public string fieldName;
    public GameObject trackingObject;


    public Image maxBarImg;
    public Image tBarImg;
    public Image curBarImg;

    public float animSpeed = 1;

    private float lastVal;
    private float maxVal = 100;
    private float curVal;
    private float tVal;
    private float lastTVal;

    public float timeForAnimation = 0.5f;

    private IEnumerator animCoroutine;

    public bool dontShowModValue = false;//function like a healthbar if this is enabled.
    public bool showMax = false;

    public TMPro.TextMeshProUGUI StatNameText;
    public TMPro.TextMeshProUGUI ValText;

    private void Start()
    {
        if (trackingObject != null) { InitBar(trackingObject); }
        
        //TODO:ammo tracking. should this be a special exception? Or a new script?
    }

    public void InitBar(GameObject trackingObject)
    {
        this.trackingObject = trackingObject;
        statData = trackingObject.GetComponentInChildren<IStats>();
        statData.AddListener(fieldName, UpdateData);
    }

    private void OnDestroy()
    {
        statData?.RemoveListener(fieldName, UpdateData);
    }
    //Stats.maxValue to size bar correctly.
    public void UpdateData(IStat stat)
    {
        TMPro.TextMeshProUGUI[] text = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        //TODO: Text only vals - Ie cash, doesnt need a bar.
        //TODO: test health ui - take damage
        StatNameText.text = fieldName.Split('_')[0];
        ValText.text = stat.FinalValue.ToString();
        if(!dontShowModValue) { ValText.text += " (+ " + stat.Modifiers.ToString() + ")"; }
        if(showMax) { ValText.text += " / " + stat.MaxValue.ToString(); }
        
        

        float oldVal = curVal;
        float oldTVal = tVal;
        curVal = stat.CurrentValue;
        tVal = stat.FinalValue;
        maxVal = stat.MaxValue;

        StopAllCoroutines();

        if(curBarImg != null && tBarImg != null)
        {
            updateBar(curBarImg, oldVal, curVal);
            updateBar(tBarImg, oldTVal, tVal);
        }
    }

    public void updateBar(Image bar,float startVal, float destVal)
    {
        if (gameObject.activeInHierarchy)
        {
            animCoroutine = changeBar(bar, startVal, destVal, animSpeed);
        
            StartCoroutine(animCoroutine);
        }
    }

    private IEnumerator changeBar(Image barImg, float startVal, float endVal, float animTime)
    {

        float speed = Mathf.Abs(startVal - endVal) / animTime;

        float modifier = (Time.deltaTime * speed);//added because we were getting modifiers with 0 value.
        if (endVal < startVal)
        {
            modifier = -modifier;
        }

        for (float i = startVal; i != endVal && Mathf.Abs(i - endVal) > (Time.deltaTime * speed) ; i += modifier)
        {
            Vector3 newSz = new Vector3(Mathf.Clamp((i / maxVal),0.001f,1), barImg.rectTransform.localScale.y, barImg.rectTransform.localScale.z);
            barImg.rectTransform.localScale = newSz;
            yield return new WaitForSeconds(0.0001f);
        }
        float barScale = Mathf.Clamp(((float)endVal / maxVal), 0.001f, 1);
        if(Double.IsNaN(barScale)) { barScale = 0; }
        barImg.rectTransform.localScale = new Vector3(barScale, barImg.rectTransform.localScale.y, barImg.rectTransform.localScale.z);
        yield return null;
    }

}
