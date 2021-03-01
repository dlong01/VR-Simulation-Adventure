using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackScript : MonoBehaviour
{
    public Image blackOverlay;

    bool fadeOut = false;
    Collider safeBox;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut == true)
        {
            FadeScreen();
        }
    }

    private void FadeScreen()
    {
        Color objectColor = blackOverlay.GetComponent<Image>().color;
        float objectAlpha = CalculateAlpha(objectColor);

        objectColor.a = objectAlpha;

        blackOverlay.GetComponent<Image>().color = objectColor;
    }

    private float CalculateAlpha(Color objectColor)
    {
        float alpha = objectColor.a;

        //float[] edgeDistances = new float[4];

        Vector3 currentPoint = transform.position;
        /*
        edgeDistances[0] = Vector3.Distance(wallXpos.ClosestPointOnBounds(currentPoint), currentPoint);
        edgeDistances[1] = Vector3.Distance(wallXneg.ClosestPointOnBounds(currentPoint), currentPoint);
        edgeDistances[2] = Vector3.Distance(wallZpos.ClosestPointOnBounds(currentPoint), currentPoint);
        edgeDistances[3] = Vector3.Distance(wallZneg.ClosestPointOnBounds(currentPoint), currentPoint);

        System.Array.Sort(edgeDistances);
        */
        Vector3 closestSafePoint = safeBox.ClosestPointOnBounds(currentPoint);

        float distanceSafe = Vector3.Distance(closestSafePoint, currentPoint);
        //float totalDistance = edgeDistances[0] + distanceSafe;

        alpha = (distanceSafe / 0.6F) * 1.0F;

        return alpha;
    }

    private void OnTriggerExit(Collider other)
    {
        fadeOut = true;
        safeBox = other;
    }

    private void OnTriggerEnter(Collider other)
    {
        Color color = blackOverlay.GetComponent<Image>().color;
        color.a = 0.0F;
        blackOverlay.GetComponent<Image>().color = color;
        fadeOut = false;
    }
}
