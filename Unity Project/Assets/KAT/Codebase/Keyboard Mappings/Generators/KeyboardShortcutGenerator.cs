using KAT.Layouts;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{
    public class KeyboardShortcutGenerator_v2 : MonoBehaviour
    {
        [Button]
        public virtual void Build()
        {
            Build_internal(KUIElementCollectionHelpers.CreateNewCollection(MappingName()));
        }

        protected virtual string MappingName()
        {
            return "Default";
        }

        protected virtual void Build_internal(KUIMapping collection)
        {

        }
    }
}