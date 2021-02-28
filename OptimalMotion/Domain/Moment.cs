using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class Moment : IMoment
    {
        public Moment(int value)
        {
            Value = value;
        }

        private int value;
        public int Value
        {
            get => value;
            set
            {
                if (value >= 0)
                    this.value = value;
                else
                    throw new ArgumentException();
            }
        }

        public int CompareTo(IMoment otherMoment)
        {
            return value.CompareTo(otherMoment.Value);
        }

        public Moments Type { get; set; }

        public static int operator -(IMoment moment1, IMoment moment2)
        {
            return moment1.Value - moment2.Value;
        }
    }
}
