using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
  private Coroutine drawing;
  [SerializeField]
  private GameObject linePrefab;
  private GameObject currentLine;
  [SerializeField]
  //private LineIntersectCalculator lineIntersectCalculator;
  void Start() {
    //lineIntersectCalculator = FindObjectOfType<LineIntersectCalculator>();
    //if (lineIntersectCalculator == null) {
    //  Debug.LogError("A LineIntersectCalculator must be available in scene!");
    //}
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      StartLine();
    }
    if (Input.GetMouseButtonUp(0)) {
      FinishLine();
    }
  }

  private void FinishLine() {
    StopCoroutine(drawing);
    //lineIntersectCalculator.DrawnLine = null;
  }

  private void StartLine() {
    if (drawing != null) {
      StopCoroutine(drawing);
    }
    drawing = StartCoroutine(Draw());
  }

  private IEnumerator Draw() {
    currentLine = Instantiate(linePrefab);
    LineRenderer lineRenderer = currentLine.GetComponent<LineRenderer>();
    //lineIntersectCalculator.DrawnLine = lineRenderer;
    if (lineRenderer == null) {
      Debug.LogError($"No line renderer component in {currentLine.name}");
    }
    lineRenderer.positionCount = 0;
    while (true) {
      Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePosition.z = 0;
      lineRenderer.positionCount++;
      lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePosition);
      // act once per frame and then wait until next frame to act again
      yield return null;
    }
  }
}
