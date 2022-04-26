using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class TracingCircleController : MonoBehaviour {
  public CircleCollider2D CircleCollider { get; private set; }
  public Rigidbody2D rb { get; private set; }
  private void Awake() {
    CircleCollider = GetComponent<CircleCollider2D>();
    rb = GetComponent<Rigidbody2D>();
  }
}
