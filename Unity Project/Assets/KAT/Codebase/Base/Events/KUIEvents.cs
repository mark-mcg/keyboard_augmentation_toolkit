using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.WSA.Input;

namespace KAT
{
    [Serializable]
    public class BaseKUIEvent
    {
        public object sender;

        public BaseKUIEvent(object sender = null) 
        {
            this.sender = sender;
        }

        /// <summary>
        /// Use this to direct event to a specific KUI element e.g. location of a particular key
        /// </summary>
        public KUILocationTag selectorTag;
    }

    public class KUIEvents : MonoBehaviour
    {
        //public delegate void KUIEventDelegate(BaseKUIEvent @event);

        public interface KUIElementDepedent
        {
            void SetKUIParent(KUIElement parent);
        }

        //public interface KUIEventProvider
        //{
        //    event KUIEventDelegate OnKUIEvent;
        //}

        //public interface KUIEventReceiver
        //{
        //    void ReceiveKUIEvent(BaseKUIEvent @event);
        //}

        //public interface KUIParentEventProvider
        //{
        //    event KUIEventDelegate OnKUIParentEvent;
        //    void AddChildListener(KUIParentEventReceiver child);
        //    void RemoveChildListener(KUIParentEventReceiver child);
        //}

        //public interface KUIParentEventReceiver
        //{
        //    void ReceiveParentKUIEvent(BaseKUIEvent @event);
        //}

        //public interface KUIChildEventProvider
        //{
        //    event KUIEventDelegate OnKUIChildEvent;
        //    void AddParentListener(KUIChildEventReceiver child);
        //    void RemoveParentListener(KUIChildEventReceiver child);

        //}

        //public interface KUIChildEventReceiver
        //{
        //    void ReceiveChildKUIEvent(BaseKUIEvent @event);
        //}
    }
}
