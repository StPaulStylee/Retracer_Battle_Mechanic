using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIntersectCalculator : MonoBehaviour
{
  public LineRenderer BattleLine;
  public LineRenderer DrawnLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (DrawnLine == null) return;
    Debug.Log("Drawnline set");
    }
}
