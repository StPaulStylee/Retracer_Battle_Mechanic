using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BattleMechanicController : MonoBehaviour {
  public GameObject TracingCirclePrefab;
  public int interpolationFramesCount = 120;

  private int elapsedFrames;
  private GameObject tracingCircle;
  private Bounds bounds;
  private Vector3 leftBound, rightBound;

  private void Awake() {
    bounds = GetComponent<BoxCollider2D>().bounds;
    leftBound = new Vector3(bounds.min.x, 0 , 0);
    rightBound = new Vector3(bounds.max.x, 0, 0);
  }
  void Start() {
    tracingCircle = GameObject.FindGameObjectWithTag("TracingCircle");
    if (tracingCircle == null) {
      tracingCircle = InstantiateTracingCircle();
      return;
    }
    SetTracingCirclePosition(tracingCircle);
  }

  void Update() {
    rightBound.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    leftBound.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
    Vector3 interpolatedPosition = Vector3.Lerp(leftBound, rightBound, interpolationRatio);
    elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);
    tracingCircle.transform.position = interpolatedPosition;
    //Debug.Log($"{elapsedFrames}/{interpolationFramesCount}");
    //Debug.Log(interpolationRatio);
  }

  private GameObject InstantiateTracingCircle() {
    GameObject tracingCircle = Instantiate(TracingCirclePrefab);
    SetTracingCirclePosition(tracingCircle);
    return tracingCircle;
  }

  private void SetTracingCirclePosition(GameObject tracingCircle) {
    TracingCircleController circleCtrl = tracingCircle.GetComponent<TracingCircleController>();
    tracingCircle.transform.position = new Vector3(leftBound.x + circleCtrl.CircleCollider.radius, 0, 0);
  }
}
