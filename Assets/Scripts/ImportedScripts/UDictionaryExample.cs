using System;
/*
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;*/

using UnityEngine;
/*
using UnityEngine.SceneManagement;
using UnityEngine.AI;*/

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class UDictionaryExample : MonoBehaviour
{

    [UDictionary.Split(35, 65)]
    public UDictionary1 npcDictionary;
    [Serializable]
    public class UDictionary1 : UDictionary<Key, Value> { }

    [Serializable]
    public class Key
    {
        public int questNum;
    }

    [Serializable]
    public class Value
    {
        public GameObject spawnPos;

        public GameObject npc;
    }

}