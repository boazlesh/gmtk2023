using Assets.Scripts.Enums;

namespace Assets.Scripts
{
    public class Intention
    {
        public Unit Originator { get; set; }
        public Verb Verb { get; set; }
        public int TargetIndex { get; set; }

        public Intention(Unit originator, Verb verb, int targetIndex)
        {
            Originator = originator;
            Verb = verb;
            TargetIndex = targetIndex;
        }
    }
}