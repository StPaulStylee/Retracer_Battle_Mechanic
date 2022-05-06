using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class BattleLineController : MonoBehaviour {
  private LineRenderer lineRenderer;
  private EdgeCollider2D edgeCollider;
  [SerializeField]
  private int points;
  [SerializeField]
  private float amplitude = 1f;
  [SerializeField]
  private float frequency = 0.25f;
  [SerializeField]
  private Vector2 xLimit = new Vector2(8, -8);
  [SerializeField]
  private float movementSpeed = 1f;
  //public int TriggerEnterCount, TriggerStayCount, TriggerExitCount = 0;
  private Vector2 lastEnterPoint, lastStayPoint;
  private int enterIndex, exitIndex;
  private int intersectTotal = 0;
  public int IntersectTotal { 
    get { return intersectTotal; }
    set { intersectTotal += value; }
  }
  private Vector3[] linePositions;
  private void Awake() {
    lineRenderer = GetComponent<LineRenderer>();
    edgeCollider = GetComponent<EdgeCollider2D>();
  }

  private void Start() {
    Draw();
    SetEdgeCollider();
    linePositions = new Vector3[lineRenderer.positionCount];
    lineRenderer.GetPositions(linePositions);
  }

  void Update() {
    //Draw();
    //SetEdgeCollider();
  }

  private void Draw() {
    float tau = 2 * Mathf.PI;
    float xStart = xLimit.x;
    float xFinish = xLimit.y;
    lineRenderer.positionCount = points;
    for (int i = 0; i < points; i++) {
      float progess = (float)i / (points - 1);
      float x = Mathf.Lerp(xStart, xFinish, progess);
      float y = amplitude * Mathf.Sin(tau * frequency * x + Time.timeSinceLevelLoad * movementSpeed);
      // Consider using SetPositions here for a performance improvement
      lineRenderer.SetPosition(i, new Vector3(x, y, 0));
    }
  }

  private void SetEdgeCollider() {
    List<Vector2> edges = new List<Vector2>();
    for (int i = 0; i < points; i++) {
      Vector3 point = lineRenderer.GetPosition(i);
      edges.Add(new Vector2(point.x, point.y));
    }
    edgeCollider.SetPoints(edges);
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    lastEnterPoint = collision.GetContact(0).point;
    //Debug.Log(String.Format("{0:f6}", lastEnterPoint));
    enterIndex = FindCollisionIndex(lastEnterPoint);
    //Debug.Log(enterIndex);
    //TriggerEnterCount++;
  }

  private void OnCollisionStay2D(Collision2D collision) {
    // This doesn't always get called if it is a quick intersection therefore we need to figure out
    // how to handle that case.
    lastStayPoint = collision.GetContact(0).point;
    //TriggerEnterCount++;
  }

  private void OnCollisionExit2D(Collision2D collision) {
    //Debug.Log(String.Format("{0:f6}", lastStayPoint));
    exitIndex = FindCollisionIndex(lastStayPoint);
    Debug.Log($"{exitIndex} : {enterIndex}");
    IntersectTotal = exitIndex - enterIndex;
    Debug.Log(intersectTotal);
  }
  
  private int FindCollisionIndex(Vector2 collisionPoint) {
    float previousDistance = Vector3.Distance(linePositions[0], collisionPoint);
    float currentDistance;
    for (int i = 1; i < linePositions.Length; i++) {
      currentDistance = Vector3.Distance(linePositions[i], collisionPoint);
      if (currentDistance > previousDistance) {
        return i;
      }
      previousDistance = currentDistance;
    }
    // Assume that if currentDistance is never great than the previous we exited at the very end of the line
    return linePositions.Length - 1;
  }
}
