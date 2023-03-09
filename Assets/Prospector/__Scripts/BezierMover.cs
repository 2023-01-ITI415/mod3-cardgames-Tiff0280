using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Events;                                                    

/// <summary>
/// This class enables controllable, fire-and-forget motion for GameObjects that
///  use it. It includes a UnityEvent completionEvent that is invoked when the
///  motion has concluded. This class requires and makes use of the Utils class,
///  particularly the Bezier() and Easing() methods. 
/// </summary>
public class BezierMover : MonoBehaviour
{
    // An enum to track the possible states of a Bezier movement
    public enum eState { idle, pre, active, post }
    
    // An enum to set the way position is set (see Tooltip that follows)
    public enum ePosMode { world, local, ugui }

    [Header("Inscribed")]
    [Tooltip("world sets transform.position, " +
    "local sets transform.localPosition, " +
    "ugui sets anchorMin and anchorMax of RectTransform.")]
    public ePosMode posMode = ePosMode.ugui;
    
    [Header("Dynamic")]
    public eState state = eState.idle;
    public float timeStart = -1f;
    public float timeDuration = 1f;
    public string easingCurve = Easing.InOut;// Uses Easing() in Utils.cs
    public List<Vector3> bezierPts;// Bézier points for movement

    public UnityEvent completionEvent;                                    

    // Allow others to see but not set u and uCurved
    public float u { get; private set; }                           
    public float uCurved { get; private set; }
 
    private RectTransform rectTrans;
 
    /// <summary>
    /// Initialize the movement of this BezierMover. The BezierMover will always
    ///  jump to the 0th point in the pts list and wait until timeS to begin
    ///  moving. If timeS is set to the default 0 value, then timeS will be set
    ///  to Time.time (the current time). 
    /// </summary>
    /// <param name="pts">List of Vector3 Bezier points</param>
    /// <param name="timeD">The duration of the movement in seconds</param>
    /// <param name="timeS">The start time (default of 0 is now)</param>
    public void Init(List<Vector3> pts, float timeD = 1, float timeS = 0)
    {
        // Note the use of parameter default values for timeD & timeS
        if (pts == null || pts.Count == 0)
        {
            Debug.LogError("You must pass at least one point into Init()!");
            return;
        }

        rectTrans = GetComponent<RectTransform>();

        // Always jump to the 0th point
        pos = pts[0];

        // And if there’s only that one point, then just callback
        if (pts.Count == 1)
        {
            completionEvent.Invoke();                                      
            return;
        }

        bezierPts = new List<Vector3>(pts);

        // If timeS is the default, just start at the current time
        if (timeS == 0) timeS = Time.time;
        timeStart = timeS;
        timeDuration = timeD;

        state = eState.pre;// Set it to the pre state, ready to start moving
    }

    /// <summary>
    /// An overload of <see cref="Init(List{UnityEngine.Vector3},float,float)"/>
    ///  that accepts Vector2 overload method to initialize the movement of this
    ///  BezierMover. The ptsV2 Vector2s are converted to Vector3s and the
    ///  Vector3 version is called.
    /// </summary>
    /// <param name="ptsV2">List of Vector2 Bezier points</param>
    /// <param name="timeD">The duration of the movement in seconds</param>
    /// <param name="timeS">The start time (default of 0 is now)</param>
    public void Init(List<Vector2> ptsV2, float timeD = 1, float timeS = 0)
    {
        // Cannot implicitly convert from List<Vector2> to List<Vector3>
        //  so we’ll do it manually.
        List < Vector3 > ptsV3 = new List<Vector3>();
        foreach (Vector2 v2 in ptsV2) { ptsV3.Add((Vector3)v2); }        // e

        // Then call the Vector3 overload of Init()
        Init(ptsV3, timeD, timeS);
    }

    /// <summary>
    /// <para>pos is a property that acts differently based on posMode:</para>
    /// <para>– world affects transform.position,</para>
    /// <para>– local affects transform.localPosition,</para>
    /// <para>– ugui affects the RectTransform.anchorMin and .anchorMax.</para>
    /// </summary>
    public Vector3 pos
    {                                                      // f
        get
        {
            if (posMode == ePosMode.ugui)
            {
                return rectTrans.anchorMin;
            }
            else if (posMode == ePosMode.local)
            {
                return transform.localPosition;
            }
            else
            {
                return transform.position;
            }
        }
        private set
        {
            if (posMode == ePosMode.ugui)
            {
                rectTrans.anchorMin = rectTrans.anchorMax = value;
            }
            else if (posMode == ePosMode.local)
            {
                transform.localPosition = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }
 
    void Update()
    {
        // If this is not moving, just return
        if (state == eState.idle || state == eState.post) return;

        // Get u from the current time and duration
        // u ranges from 0 to 1 (usually)
        u = (Time.time - timeStart) / timeDuration;
        // Use Easing class from Utils to curve the u value
        uCurved = Easing.Ease(u, easingCurve);

        if (u < 0)
        {  // If u<0, then we shouldn’t move yet.
            state = eState.pre;
        }
        else
        {
            if (u < 1)
            {  // 0<=u<1, which means this is active and moving

                state = eState.active;
            }
            else
            {  // u>=1, which means this is done moving
                uCurved = 1;// Set uCurved=1 so we don’t overshoot
                state = eState.post;
                completionEvent.Invoke();
            }

            // Use the Utils Bézier curve method to move this to the right point
            pos = Utils.Bezier(uCurved, bezierPts);
        }
    }
}
