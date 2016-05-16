using UnityEngine;
using System.Collections;
using System;

public class ProceduralTest : ProceduralBase {

  public int value;

  [Persistent]
  public int persistentValue;

  public override void ProceduralInit() {
    Debug.Log("Init");
  }

  public override void ProceduralCleanup() {
    Debug.Log("Cleanup");
  }

  public override void ProceduralUpdate() {
    Debug.Log("Update");
  }
}
