/*    WaypointRoute.cs by NGC6543(ngc6543@me.com)
  * 
  *  Based on the WaypointCircuit.cs from Unity's Standard Assets.
  *  It supports both Closed Circuit and Open Route.
  *  
  */

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class WaypointRoute : MonoBehaviour
{
    public enum RouteType { ClosedCircuit, OpenRoute }
    [Header("Waypoint Circuit Settings", order = 0)]
    public Transform[] Waypoints;
    public RouteType routeType = RouteType.ClosedCircuit;

    //public WaypointList waypointList = new WaypointList();
    [SerializeField] private bool smoothRoute = true;
    int numPoints;
    [SerializeField, HideInInspector] Vector3[] points;
    [SerializeField, HideInInspector] float[] distances;

#if UNITY_EDITOR
    [Header("Editor & Gizmo")]
    public bool showName = false;
    [SerializeField] float editorVisualisationSubsteps = 100;
    [SerializeField] Color pathColor = Color.magenta;
    [SerializeField] Color nodeColor = Color.red;
    [SerializeField, Range(0.1f, 1f)] float nodeSize = 0.5f;
    [SerializeField] bool moveTo1stPos;
    [SerializeField] bool moveToCenter;
    [SerializeField] public bool reverseDirection;
    public float NodeSize { get { return nodeSize; } }
#endif

    public float Length { get; private set; }

    [System.Serializable]
    public class WaypointList
    {
        //public WaypointCircuit circuit;
        public Transform[] items = new Transform[0];
    }

    public struct RoutePoint
    {
        public Vector3 position;
        public Vector3 direction;


        public RoutePoint(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }

    //this being here will save GC allocs
    private int p0n;
    private int p1n;
    private int p2n;
    private int p3n;

    private float lerpRatio;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;
    private Vector3 P3;

    // Use this for initialization
    private void Awake()
    {
        if (Waypoints.Length > 1)
        {
            CachePositionsAndDistances();
        }
        numPoints = Waypoints.Length;
    }


    public RoutePoint GetRoutePoint(float dist)
    {
        // position and direction
        Vector3 p1 = GetRoutePosition(dist);
        Vector3 p2 = GetRoutePosition(dist + 0.1f);
        Vector3 delta = p2 - p1;
        return new RoutePoint(p1, delta.normalized);
    }


    public Vector3 GetRoutePosition(float dist)
    {
        int index = 0;

        if (Length == 0)
        {
            Length = distances[distances.Length - 1];
        }

        if (routeType == RouteType.ClosedCircuit)
        {
            dist = Mathf.Repeat(dist, Length);
            while (distances[index] < dist)
            {
                ++index;
            }

            // get nearest two points, ensuring points wrap-around start & end of circuit
            p1n = ((index - 1) + numPoints) % numPoints;
            p2n = index;

            // found point numbers, now find interpolation value between the two middle points

            lerpRatio = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

            if (smoothRoute)
            {
                // smooth catmull-rom calculation between the two relevant points


                // get indices for the surrounding 2 points, because
                // four points are required by the catmull-rom function
                p0n = ((index - 2) + numPoints) % numPoints;
                p3n = (index + 1) % numPoints;

                // 2nd point may have been the 'last' point - a dupe of the first,
                // (to give a value of max track distance instead of zero)
                // but now it must be wrapped back to zero if that was the case.
                p2n = p2n % numPoints;

                P0 = points[p0n];
                P1 = points[p1n];
                P2 = points[p2n];
                P3 = points[p3n];

                return CatmullRom(P0, P1, P2, P3, lerpRatio);
            }
            else
            {
                // simple linear lerp between the two points:

                p1n = ((index - 1) + numPoints) % numPoints;
                p2n = index;

                return Vector3.Lerp(points[p1n], points[p2n], lerpRatio);
            }
        }
        else
        {
            dist = Mathf.Clamp(dist, 0f, Length);
            while (distances[index] < dist)
            {
                ++index;
            }

            if (index < 2 || index == distances.Length - 1)
            {
                if (index == 0) index = 1;
                p1n = (index - 1);
                p2n = index;
                lerpRatio = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);
                return Vector3.Lerp(points[p1n], points[p2n], lerpRatio);
            }
            else
            {
                p1n = (index - 1);
                p2n = index;

                // found point numbers, now find interpolation value between the two middle points

                lerpRatio = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

                if (smoothRoute)
                {
                    // smooth catmull-rom calculation between the two relevant points


                    // get indices for the surrounding 2 points, because
                    // four points are required by the catmull-rom function
                    p0n = (index - 2);
                    p3n = (index + 1);

                    P0 = points[p0n];
                    P1 = points[p1n];
                    P2 = points[p2n];
                    P3 = points[p3n];
                    //Debug.Log(p0n +", "+ p1n +", "+ p2n +", "+ p3n);
                    return CatmullRom(P0, P1, P2, P3, lerpRatio);
                }
                else
                {
                    if (index == 0) index = 1;
                    p1n = (index - 1);
                    p2n = index;

                    return Vector3.Lerp(points[p1n], points[p2n], lerpRatio);
                }
            }
        }
    }

    public float GetProgress(Vector3 position, out int crntNearestNodeID, int prevNearestNodeID = -1)
    {
        //int index = 0;

        if (Length == 0)
        {
            Length = distances[distances.Length - 1];
        }

        int nearestNodeID = -1;
        if (prevNearestNodeID == -1)
        {
            // find nearest node for once
            float dist = 99999f;
            for (int i = 0; i < Waypoints.Length; i++)
            {
                float temp = Vector3.SqrMagnitude(position - Waypoints[i].position);
                if (temp < dist)
                {
                    dist = temp;
                    nearestNodeID = i;
                }
            }
            //Debug.Log("initial nearest node : " + nearestNodeID);
        }
        else
        {
            nearestNodeID = prevNearestNodeID;
        }
        int i0;
        int i1;
        Vector3 p0;
        Vector3 v0;
        Vector3 v1;
        Vector3 n;
        float lerp = 0;
        float result = 0f;

        switch (routeType)
        {
            case RouteType.OpenRoute:
                i0 = nearestNodeID == numPoints ? numPoints - 1 : nearestNodeID;
                i1 = nearestNodeID + 1;
                p0 = position - Waypoints[i0].position;
                v0 = Waypoints[i0].position;
                v1 = Waypoints[i1].position;
                n = v1 - v0;

                if (Vector3.Dot(p0, n) > 0)
                {
                    // nearest node is backward and consistent!
                    lerp = Vector3.Dot(n, position - v0) / n.sqrMagnitude;
                    result = Mathf.Lerp(distances[i0], distances[i1] + (i1 == 0 ? Length : 0), lerp);
                    //Debug.Log("nearest : " + i0 +", backward\n" +
                    //          "lerp : " + lerp +"\n" +
                    //          "result : " + result);
                    if (lerp > 0.5f) nearestNodeID = i1;
                }
                else
                {
                    // nearest node is forward!
                    i0 = (nearestNodeID - 1 + numPoints) % numPoints;
                    i1 = nearestNodeID;
                    v0 = Waypoints[i0].position;
                    v1 = Waypoints[i1].position;
                    n = v1 - v0;

                    lerp = Vector3.Dot(n, position - v0) / n.sqrMagnitude;
                    result = Mathf.Lerp(distances[i0], distances[i1] + (i1 == 0 ? Length : 0), lerp);
                    //Debug.Log("nearest : " + i1 +", forward\n" +
                    //          "lerp : " + lerp +"\n" +
                    //          "result : " + result);
                }

                crntNearestNodeID = nearestNodeID;
                break;
            case RouteType.ClosedCircuit:
                // determine the closest node is forward / backward the nearest line.
                // assume the nearest node is backward point
                i0 = nearestNodeID;
                i1 = (nearestNodeID + 1) % numPoints;
                p0 = position - Waypoints[i0].position;
                v0 = Waypoints[i0].position;
                v1 = Waypoints[i1].position;
                n = v1 - v0;
                if (Vector3.Dot(p0, n) > 0)
                {
                    // nearest node is backward and consistent!
                    lerp = Vector3.Dot(n, position - v0) / n.sqrMagnitude;
                    result = Mathf.Lerp(distances[i0], distances[i1] + (i1 == 0 ? Length : 0), lerp);
                    //Debug.Log("nearest : " + i0 +", backward\n" +
                    //          "lerp : " + lerp +"\n" +
                    //          "result : " + result);
                    if (lerp > 0.5f) nearestNodeID = i1;
                }
                else
                {
                    // nearest node is forward!
                    i0 = (nearestNodeID - 1 + numPoints) % numPoints;
                    i1 = nearestNodeID;
                    v0 = Waypoints[i0].position;
                    v1 = Waypoints[i1].position;
                    n = v1 - v0;

                    lerp = Vector3.Dot(n, position - v0) / n.sqrMagnitude;
                    result = Mathf.Lerp(distances[i0], distances[i1] + (i1 == 0 ? Length : 0), lerp);
                    //Debug.Log("nearest : " + i1 +", forward\n" +
                    //          "lerp : " + lerp +"\n" +
                    //          "result : " + result);
                }

                crntNearestNodeID = nearestNodeID;
                break;

            default:
                crntNearestNodeID = nearestNodeID;
                break;
        }

        return result;
    }


    public float GetProgressDistanceOnNode(int nodeIndex)
    {
        //Debug.Log(name + " node count : " + nodes.Length +"\n" +
        //          "requested index : " + nodeIndex);

        try
        {
            return distances[nodeIndex];
        }
        catch (System.IndexOutOfRangeException e)
        {
            return 999f;
        }
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        // comments are no use here... it's the catmull-rom equation.
        // Un-magic this, lord vector!
        return 0.5f *
                    ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                    (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }


    private void CachePositionsAndDistances()
    {
        // transfer the position of each point and distances between points to arrays for
        // speed of lookup at runtime


        float accumulateDistance = 0;
        switch (routeType)
        {
            case RouteType.ClosedCircuit:

                points = new Vector3[Waypoints.Length + 1];
                distances = new float[Waypoints.Length + 1];

                for (int i = 0; i < points.Length; ++i)
                {
                    var t1 = Waypoints[(i) % Waypoints.Length];
                    var t2 = Waypoints[(i + 1) % Waypoints.Length];
                    if (t1 != null && t2 != null)
                    {
                        Vector3 p1 = t1.position;
                        Vector3 p2 = t2.position;
                        points[i] = Waypoints[i % Waypoints.Length].position;
                        distances[i] = accumulateDistance;
                        accumulateDistance += (p1 - p2).magnitude;
                    }
                }
                break;
            case RouteType.OpenRoute:

                points = new Vector3[Waypoints.Length];
                distances = new float[Waypoints.Length];

                for (int j = 0; j < Waypoints.Length - 1; ++j)
                {
                    var t1 = Waypoints[j];
                    var t2 = Waypoints[j + 1];
                    if (t1 != null && t2 != null)
                    {
                        Vector3 p1 = t1.position;
                        Vector3 p2 = t2.position;
                        points[j] = Waypoints[j].position;
                        distances[j] = accumulateDistance;
                        accumulateDistance += (p1 - p2).magnitude;
                    }
                }
                points[points.Length - 1] = Waypoints[Waypoints.Length - 1].position;
                distances[distances.Length - 1] = accumulateDistance;
                break;

            default:
                break;

        }

    }

#if UNITY_EDITOR

    void Update()
    {
        if (Waypoints.Length != transform.childCount) UpdateNodes();
        if (moveTo1stPos)
        {
            moveTo1stPos = false;
            MoveTo1stNode();
        }
        if (moveToCenter)
        {
            moveToCenter = false;
            MoveToCenter();
        }
        if (reverseDirection)
        {
            reverseDirection = false;
            ReverseDirection();
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }


    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }


    private void DrawGizmos(bool selected)
    {
        //waypointList.circuit = this;

        switch (routeType)
        {
            case RouteType.ClosedCircuit:
                if (Waypoints.Length > 1)
                {
                    numPoints = Waypoints.Length;

                    CachePositionsAndDistances();
                    Length = distances[distances.Length - 1];

                    // draw path
                    Gizmos.color = pathColor;
                    Vector3 prev = Waypoints[0].position;
                    if (smoothRoute)
                    {
                        float step = Length / editorVisualisationSubsteps;
                        for (float dist = 0; dist < Length; dist += step)
                        {
                            Vector3 next = GetRoutePosition(dist + 1);
                            Gizmos.DrawLine(prev, next);
                            prev = next;
                        }
                        Gizmos.DrawLine(prev, Waypoints[0].position);
                    }
                    else
                    {
                        for (int n = 0; n < Waypoints.Length; ++n)
                        {
                            Vector3 next = Waypoints[(n + 1) % Waypoints.Length].position;
                            Gizmos.DrawLine(prev, next);
                            prev = next;
                        }
                    }


                }
                break;
            case RouteType.OpenRoute:
                if (Waypoints.Length > 1)
                {
                    numPoints = Waypoints.Length;
                    CachePositionsAndDistances();
                    Length = distances[distances.Length - 1];

                    // draw path
                    Gizmos.color = pathColor;
                    Vector3 prev = points[0];
                    if (smoothRoute)
                    {
                        float step = Length / editorVisualisationSubsteps;
                        for (float dist = 0; dist < Length; dist += step)
                        {
                            Vector3 next = GetRoutePosition(dist + 1);
                            Gizmos.DrawLine(prev, next);
                            prev = next;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Waypoints.Length - 1; i++)
                        {
                            Vector3 next = Waypoints[i + 1].position;
                            Gizmos.DrawLine(prev, next);
                            prev = next;
                        }
                    }
                }
                break;
            default:
                break;
        }
        // draw node
        Gizmos.color = nodeColor;
        foreach (Transform t in Waypoints)
        {
            Gizmos.DrawCube(t.position, Vector3.one * nodeSize);
        }
    }

    public void UpdateNodes()
    {
        Waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Waypoints[i] = transform.GetChild(i);
            Waypoints[i].name = "Node" + i;
        }
    }

    void MoveTo1stNode()
    {
        if (Waypoints.Length == 0)
        {
            Debug.LogWarning("There are no path points yet.");
            return;
        }
        Vector3 offset = transform.position - Waypoints[0].position;
        transform.position = Waypoints[0].position;
        foreach (Transform t in Waypoints)
        {
            t.position += offset;
        }
    }
    void MoveToCenter()
    {
        if (Waypoints.Length == 0)
        {
            Debug.LogWarning("There are no path points yet.");
            return;
        }

        Vector3 center = Vector3.zero;
        for (int i = 0; i < Waypoints.Length; i++)
        {
            center += Waypoints[i].position;
        }
        center /= Waypoints.Length;
        Vector3 offset = transform.position - center;
        transform.position = center;
        foreach (Transform t in Waypoints)
        {
            t.position += offset;
        }
    }
    void ReverseDirection()
    {
        if (Waypoints.Length == 0)
        {
            Debug.LogWarning("There are no nodes yet. Add child Transform.");
            return;
        }

        Transform[] temp = Waypoints;
        foreach (Transform t in temp)
        {
            t.SetParent(null);
        }
        for (int i = temp.Length - 1; i > -1; i--)
        {
            temp[i].SetParent(transform);
        }
        UpdateNodes();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaypointRoute)), CanEditMultipleObjects]
public class WaypointRouteEditor : Editor
{
    private const float handleSize = 0.08f;
    private const float pickSize = 0.06f;

    WaypointRoute myScript;
    Transform handleTransform;

    public override void OnInspectorGUI()
    {

        //        EditorGUILayout.HelpBox("Two Types of paths\n" +
        //            "1. No margin : It starts from the first path points.\n" +
        //            "2. With margin : it starts from the second path points.\n\n" +
        //            "Option 2 needs more than two path points. " +
        //            "This is useful if there are more than two paths they needed to be smoothly blended. " +
        //            "This can be achieved by positioning the first two path points of the later path " +
        //            "at the last two path points of the prior one", MessageType.Info);
        DrawDefaultInspector();
        myScript = target as WaypointRoute;
        if (GUILayout.Button("Rebuild Path"))
        {
            myScript.UpdateNodes();
        }
    }
    bool nodeSelected = false;
    int selectedNode = -1;
    GUIStyle normalStyle = new GUIStyle();
    void OnSceneGUI()
    {
        myScript = target as WaypointRoute;
        handleTransform = myScript.transform;
        Transform[] nodes = myScript.Waypoints;

        normalStyle.normal.textColor = Color.green;
        normalStyle.fontSize = 14;
        Vector3 camPos = Camera.current.transform.position;
        Vector3 camUp = Camera.current.transform.up;
        // node handle
        Handles.color = new Color(0, 1f, 0, 1f);// green
        for (int i = 0; i < nodes.Length; i++)
        {
            float size = HandleUtility.GetHandleSize(nodes[i].position);
            Vector3 temp = Vector3.Cross((nodes[i].position - camPos), camUp).normalized * myScript.NodeSize; ;

            if (myScript.showName)
                Handles.Label(myScript.transform.position, myScript.name, normalStyle);

            Handles.Label(nodes[i].position + temp, i.ToString(), normalStyle);

            if (Handles.Button(nodes[i].position, Quaternion.identity, size * handleSize, size * pickSize, Handles.DotCap))
            {
                nodeSelected = true;
                selectedNode = i;
                break;
            }
            if (nodeSelected)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 point = Handles.DoPositionHandle(nodes[selectedNode].position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(myScript, "move node");
                    EditorUtility.SetDirty(myScript);
                    myScript.Waypoints[selectedNode].position = point;
                }
            }
        }
    }
}

#endif