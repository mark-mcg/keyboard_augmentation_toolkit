using System;

namespace KAT
{
    [Serializable]
    public class TriggerState
    {
        public const float TRIGGERED = 100;
        public const float NOT_TRIGGERED = 0;

        public float amount;

        public bool IsTriggered()
        {
            return amount >= TRIGGERED;
        }

        public bool SetAmount(float newAmount)
        {
            bool diff = amount != newAmount;
            amount = newAmount;
            return diff;
        }

        public bool SetTriggerState(bool triggered)
        {
            if (triggered)
                return SetAmount(TRIGGERED);
            else
                return SetAmount(NOT_TRIGGERED);
        }
    }
}