using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Leap.Unity.Procedural {

  [CustomEditor(typeof(ProceduralBase), editorForChildClasses: true)]
  public class ProceduralBaseEditor : CustomEditorBase {

    protected HashSet<string> _persistentProperties;

    protected override void OnEnable() {
      base.OnEnable();

      _persistentProperties = new HashSet<string>();
      Type targetType = target.GetType();
      FieldInfo[] fields = targetType.GetFields(BindingFlags.Public |
                                                BindingFlags.NonPublic |
                                                BindingFlags.Instance |
                                                BindingFlags.FlattenHierarchy);

      for (int i = 0; i < fields.Length; i++) {
        FieldInfo field = fields[i];
        object[] persistentAttributes = field.GetCustomAttributes(typeof(PersistentAttribute), true);
        if (persistentAttributes.Length != 0) {
          _persistentProperties.Add(field.Name);
        }
      }
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      if (_modifiedProperties.Count != 0) {

        bool isAllPersistent = true;
        for (int i = 0; i < _modifiedProperties.Count; i++) {
          string name = _modifiedProperties[i].name;
          if (!_persistentProperties.Contains(name)) {
            isAllPersistent = false;
            break;
          }
        }

        ProceduralBase proceduralBase = target as ProceduralBase;
        if (!isAllPersistent) {
          proceduralBase.ProceduralCleanup();
          proceduralBase.ProceduralInit();
        }

        proceduralBase.ProceduralUpdate();
      }
    }
  }
}
