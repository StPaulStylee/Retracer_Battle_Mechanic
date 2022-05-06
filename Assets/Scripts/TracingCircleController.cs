using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class TracingCircleController : MonoBehaviour {
  public CircleCollider2D CircleCollider { get; private set; }
  public Rigidbody2D rb { get; private set; }

  //public int TriggerEnterCount, TriggerStayCount, TriggerExitCount = 0;
  private void Awake() {
    CircleCollider = GetComponent<CircleCollider2D>();
    rb = GetComponent<Rigidbody2D>();
  }

  //private void OnCollisionEnter2D(Collision2D collision) {
  //  Debug.Log("STAYYYYY");
  //}

  //private void OnTriggerEnter2D(Collider2D collision) {
  //  TriggerEnterCount++;
  //  Debug.Log(TriggerEnterCount);
  //}

  //private void OnTriggerStay2D(Collider2D collision) {
  //  TriggerStayCount++;
  //  Debug.Log(TriggerStayCount);
  //}

  //private void OnTriggerExit2D(Collider2D collision) {
  //  TriggerExitCount++;
  //  //Debug.Log(TriggerExitCount);
  //}
}
