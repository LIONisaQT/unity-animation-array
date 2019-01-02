using System;
using UnityEngine;

/// <summary>
/// Basically a ghetto serializable key-value pair for CustomTriggers and bools LMAO.
/// See https://forum.unity.com/threads/finally-a-serializable-dictionary-for-unity-extracted-from-system-collections-generic.335797/
/// or https://forum.unity.com/threads/released-serializable-dictionary-now-allowing-custom-editor-for-key-field.518178/
/// for actual serialized dictionaries. Making this class have template specialization breaks
/// serialization, so I just went with this lazy solution. To make a "dictionary" of these, you'll
/// need an array/list of this class.
/// </summary>
[Serializable]
public class SerializableKeyValuePairs {
    [Tooltip("Trigger to be affected.")]
    public CustomTrigger trigger;

    [Tooltip("State of trigger when animation finishes.")]
    public bool isTriggered;
}
