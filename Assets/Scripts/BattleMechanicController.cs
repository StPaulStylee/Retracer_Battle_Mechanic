using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BattleMechanicController : MonoBehaviour {
  public GameObject TracingCirclePrefab;
  private GameObject tracingCircle;
  private Bounds bounds;

  private void Awake() {
    bounds = GetComponent<BoxCollider2D>().bounds;
  }
  // Start is called before the first frame update
  void Start() {
    tracingCircle = GameObject.FindGameObjectWithTag("TracingCircle");
    if (tracingCircle == null) {
      Debug.Log("INSTANTIATING!");
      tracingCircle = InstantiateTracingCircle();
      return;
    }
    SetTracingCirclePosition(ref tracingCircle);
    Debug.Log("FOUND!");
  }

  // Update is called once per frame
  void Update() {

  }

  private GameObject InstantiateTracingCircle() {
    GameObject tracingCircle = Instantiate(TracingCirclePrefab);
    SetTracingCirclePosition(ref tracingCircle);
    return tracingCircle;
  }

  private void SetTracingCirclePosition(ref GameObject tracingCircle) {
    // Still need to accound for the radius of the circle
    TracingCircleController circleCtrl = tracingCircle.GetComponent<TracingCircleController>();
    tracingCircle.transform.position = new Vector3(-bounds.extents.x + circleCtrl.CircleCollider.radius, bounds.center.y, 0);
  }
}
