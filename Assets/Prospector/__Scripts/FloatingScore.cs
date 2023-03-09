using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(BezierMover))]
[RequireComponent(typeof(TMP_Text))]  // FloatingScore will require TextMeshPro
public class FloatingScore : MonoBehaviour
{
    void Awake()
    {  // This is a testing method that will be completely replaced.
        int numPoints = 4;
        BezierMover mover = GetComponent<BezierMover>();

        List < Vector2 > points = new List<Vector2>();
        Vector2 v2Half = new Vector2(0.5f, 0.5f);
        points.Add(v2Half);
        for (int i = 0; i < numPoints; i++)
        {
            points.Add(Random.insideUnitCircle * 0.5f + v2Half);
        }

        mover.completionEvent.AddListener(MoverCompleteCallback);
        mover.Init(points, numPoints);
    }

    void MoverCompleteCallback()
    {
        print("FloatingScore is done!");
    }
}
