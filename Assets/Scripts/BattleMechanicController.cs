using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BattleMechanicController : MonoBehaviour {
  [Tooltip("TracingCircle")]
  public GameObject TracingCirclePrefab; // This should be private?
  private TracingCircleController tracingCircleController;
  private GameObject tracingCircle;

  [Tooltip("Circle/Tracing Speed")]
  public int interpolationFramesCount = 120;
  private int elapsedFrames;

  private Bounds bounds;
  private Vector3 leftBound, rightBound;

  private void Awake() {
    if (TracingCirclePrefab == null) {
      Debug.LogError($"No TracingCirclePrefab assigned to {this.name}");
    }
    bounds = GetComponent<BoxCollider2D>().bounds;
    leftBound = new Vector3(bounds.min.x, 0 , 0);
    rightBound = new Vector3(bounds.max.x, 0, 0);
  }
  void Start() {
    SetTracingCircleOnStart();
  }

  void Update() {
    // What am I doing here with these bounds?
    rightBound.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    leftBound.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
    Vector3 interpolatedPosition = Vector3.Lerp(leftBound, rightBound, interpolationRatio);
    elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);
    tracingCircle.transform.position = interpolatedPosition;
    //Debug.Log($"{elapsedFrames}/{interpolationFramesCount}");
    //Debug.Log(interpolationRatio);
  }

  private void SetTracingCircleOnStart() {
    tracingCircle = GameObject.FindGameObjectWithTag("TracingCircle");
    if (tracingCircle == null) {
      tracingCircle = Instantiate(TracingCirclePrefab);
      tracingCircleController = tracingCircle.GetComponent<TracingCircleController>();
      tracingCircleController.SetStartingPosition(leftBound.x);
      return;
    }
    tracingCircleController = tracingCircle.GetComponent<TracingCircleController>();
    tracingCircleController.SetStartingPosition(leftBound.x);
  }
}
