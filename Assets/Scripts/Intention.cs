using Assets.Scripts.Enums;

namespace Assets.Scripts
{
    public class Intention
    {
        public Verb Verb { get; set; }
        public int TargetIndex { get; set; }

        public Intention(Verb verb, int targetIndex)
        {
            Verb = verb;
            TargetIndex = targetIndex;
        }
    }
}