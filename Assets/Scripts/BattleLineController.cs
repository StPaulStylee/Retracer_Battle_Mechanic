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
  private void Awake() {
    lineRenderer = GetComponent<LineRenderer>();
    edgeCollider = GetComponent<EdgeCollider2D>();
  }
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    Draw();
    SetEdgeCollider();
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
}
