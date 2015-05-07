using System;

namespace ostrich.Core
{
    public class SeasonalProduct : Product
    {
        private DateTime? seasonEndsAt;
        private DateTime? seasonStartsAt;

        public SeasonalProduct(int productId, string name)
            : base(productId, name)
        {
        }

        public DateTime? SeasonStartsAt
        {
            get { return seasonStartsAt; }
            set
            {
                if (SeasonEndsAt < value)
                    throw new InvalidOperationException("Start date must be before end date");
                seasonStartsAt = value;
            }
        }

        public DateTime? SeasonEndsAt
        {
            get { return seasonEndsAt; }
            set
            {
                if (SeasonStartsAt > value)
                    throw new InvalidOperationException("End date must be after start date");
                seasonEndsAt = value;
            }
        }

        public override bool Active
        {
            get
            {
                DateTime now = DateTime.Now;
                bool seasonBegan = SeasonStartsAt == null || now >= SeasonStartsAt;
                bool seasonNotEnded = SeasonEndsAt == null || now <= SeasonEndsAt;
                return seasonBegan && seasonNotEnded;
            }
            set { throw new InvalidOperationException("Active property cannot be set for seasonal products."); }
        }
    }
}