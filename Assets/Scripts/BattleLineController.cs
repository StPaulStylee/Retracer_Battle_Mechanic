using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class BattleLineController : MonoBehaviour {
  public int IntersectTotal { 
    get { return intersectTotal; }
    set { intersectTotal += value; }
  }

  public float IntersectCoverage {
    get {
      var coveragePercentageRaw = (float)IntersectTotal / (linePositions.Length - 1) * 100;
      return Mathf.Round(Mathf.Clamp(coveragePercentageRaw, 0, 100f));
    }
  }

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
  [SerializeField]
  [Tooltip("Adds additional indexes to the line collision at both the start and end of the collision.")]
  private int collisionPadding = 3;
  //public int TriggerEnterCount, TriggerStayCount, TriggerExitCount = 0;
  private Vector2 lastEnterPoint, lastStayPoint;
  private int enterIndex, exitIndex;
  private int intersectTotal = 0;
  private Vector3[] linePositions;
  // I don't think this variable is actually necessary
  private bool hasLastStay = false;

  private enum CollisionEventPosition {
    START,
    EXIT
  }

  private void Awake() {
    lineRenderer = GetComponent<LineRenderer>();
    edgeCollider = GetComponent<EdgeCollider2D>();
  }

  private void Start() {
    UiController.OnCoverageText("0");
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
      // Confirm if this movementSpeed is necessary. I don't think it is doing anything since the line is static (not-moving)
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
    // Assign lastStayPoint here to ensure it is always defined
    lastStayPoint = collision.GetContact(0).point;
    //Debug.Log("enter");
    //Debug.Log(String.Format("{0:f6}", lastEnterPoint));
    enterIndex = FindCollisionIndex(lastEnterPoint, CollisionEventPosition.START);
    //Debug.Log(enterIndex);
    //TriggerEnterCount++;
  }

  private void OnCollisionStay2D(Collision2D collision) {
    //Debug.Log("stay");
    hasLastStay = true;
    lastStayPoint = collision.GetContact(0).point;
    //TriggerEnterCount++;
  }

  private void OnCollisionExit2D(Collision2D collision) {
    //Debug.Log(String.Format("{0:f6}", lastStayPoint));
    exitIndex = FindCollisionIndex(lastStayPoint, CollisionEventPosition.EXIT);
    Debug.Log($"{exitIndex} : {enterIndex}");
    IntersectTotal = exitIndex - enterIndex;
    UiController.OnCoverageText(IntersectCoverage.ToString());
    hasLastStay = false;
    //Debug.Log($"{IntersectTotal}/{linePositions.Length}={String.Format("{0:f6}", (float)IntersectTotal/linePositions.Length)}");
    //Debug.Log(String.Format("{0:f6}", (float)IntersectTotal / linePositions.Length * 100));
  }
  
  private int FindCollisionIndex(Vector2 collisionPoint, CollisionEventPosition collisionPosition) {
    // 5/22 - Right now on OnCollisionExit doesn't always have a dependable value for "lastStayPoint" because sometimes
    // "lastStayPoint" may be undefined. If OnCollisionStay doesn't get called we need to use "lastEnterPoint" and we can then
    // use the collisionPadding value to determine the "lastStayValue"
    float previousDistance = Vector3.Distance(linePositions[0], collisionPoint);
    float currentDistance;
    for (int i = 1; i < linePositions.Length; i++) {
      currentDistance = Vector3.Distance(linePositions[i], collisionPoint);
      if (currentDistance > previousDistance) {
        return GetClampedIndex(i, collisionPosition);
      }
      previousDistance = currentDistance;
    }
    // Assume that if currentDistance is never greater than the previous we exited at the very end of the line
    return linePositions.Length - 1;
  }

  private int GetClampedIndex(int index, CollisionEventPosition collisionPosition) {
    if (collisionPosition == CollisionEventPosition.START) {
      //Debug.Log(Mathf.Clamp(index - collisionPadding, 0, linePositions.Length - 1));
      return Mathf.Clamp(index - collisionPadding, 0, linePositions.Length -1);
    }
    //Debug.Log(Mathf.Clamp(index + collisionPadding, index + collisionPadding, linePositions.Length - 1));
    return Mathf.Clamp(index + collisionPadding, index + collisionPadding, linePositions.Length - 1);
  }
}
