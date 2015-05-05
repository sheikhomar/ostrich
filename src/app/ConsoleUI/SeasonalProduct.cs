using System;

namespace ConsoleUI
{
    public class SeasonalProduct : Product
    {
        private bool active;

        public DateTime? SeasonStartsAt { get; set; }
        public DateTime? SeasonEndsAt { get; set; }

        public override bool Active
        {
            get { return active; }
            set
            {
                // TODO: Doesn't make any sense to set the Active property
                throw new NotImplementedException("Doesn't make any sense.");
                active = value;
            }
        }
    }
}