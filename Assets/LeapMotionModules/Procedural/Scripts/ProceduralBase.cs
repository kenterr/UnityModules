using UnityEngine;
using System.Collections;
using System;

public abstract class ProceduralBase : MonoBehaviour {

  public abstract void ProceduralInit();
  public abstract void ProceduralUpdate();
  public abstract void ProceduralCleanup();

}

public abstract class ProceduralBase<T> : ProceduralBase where T : ScriptableObject {

  [SerializeField]
  protected T _data;

}
