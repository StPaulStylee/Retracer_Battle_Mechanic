using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour {
  public delegate void OnCoverageTextHandler(string text);

  public static OnCoverageTextHandler OnCoverageText;
  
  [SerializeField]
  private TextMeshProUGUI coverageText;

  private void Awake() {
    OnCoverageText += UpdateCoverageText; 
  }
  // Start is called before the first frame update
  void Start() {
    if (coverageText == null) {
      Debug.LogError("There is no Coverage Text assigned to script!");
    }
  }

  // Do we want this for OnDestroy as well?
  private void OnDisable() {
    OnCoverageText -= UpdateCoverageText;
  }

  private void UpdateCoverageText(string text) {
    coverageText.text = $"{text}% </b> Coverage";
  }
}
