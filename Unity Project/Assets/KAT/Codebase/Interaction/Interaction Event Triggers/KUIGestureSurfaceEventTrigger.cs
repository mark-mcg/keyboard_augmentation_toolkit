using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    public abstract class KUIGestureSurfaceEventTrigger<T> : KUIElementBaseTrigger<T>
        where T: BaseKUIEvent
    {

    }
}