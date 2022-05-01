using System;

namespace KAT.Interaction
{
    public class TiltEvent : BaseKUIEvent
    {
        public enum Gesture { Left, Right, None };
        public Gesture gesture = Gesture.None;

        public TiltEvent(Gesture gesture)
        {
            this.gesture = gesture;
        }

        public override string ToString()
        {
            return String.Format("SimulatedGestAKeyEvent gesture {0}", gesture);
        }
    }
}