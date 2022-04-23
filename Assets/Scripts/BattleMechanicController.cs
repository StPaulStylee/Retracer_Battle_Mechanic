using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BattleMechanicController : MonoBehaviour {
  private LineRenderer lineRenderer;
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
  }
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    Draw();
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
}
