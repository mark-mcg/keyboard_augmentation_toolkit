using KAT;
using PubSub;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HubFixTest : MonoBehaviour
{
    // Start is called before the first frame update
    Hub hub = new Hub();

    void Awake()
    {
        LayoutChangedEvent layoutEvent = new LayoutChangedEvent();
        BaseKUIEvent baseEvent = layoutEvent;
        MappingChangedEvent mappingEvent = new MappingChangedEvent();

        //hub.Subscribe<BaseKUIEvent>(BaseEvent);
        hub.Subscribe<LayoutChangedEvent>(BaseEvent);
        hub.Subscribe<LayoutChangedEvent>(LayoutEvent);


        Debug.LogError("Publishing base type event");
        hub.Publish(baseEvent);
        //Debug.LogError("Publishing layout changed type event");
        //hub.Publish(layoutEvent);

        TypeTest<LayoutChangedEvent>(baseEvent);
        TypeTest<LayoutChangedEvent>(mappingEvent);
    }

    public bool TypeTest<T>(object toTest)
    {
        bool result = toTest.GetType().GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo());
        Debug.LogError("does " + toTest + " match " + typeof(T) + " " + result);
        return result;
    }

    public void BaseEvent(BaseKUIEvent baseEvent)
    {
        Debug.LogError("-- BaseEvent Called " + baseEvent);
    }

    public void LayoutEvent(LayoutChangedEvent layoutEvent)
    {
        Debug.LogError("-- LayoutEvent Called " + layoutEvent);
    }
}
