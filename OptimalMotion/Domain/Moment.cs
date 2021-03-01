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

        //public static int operator -(Moment moment1, Moment moment2)
        //{
        //    return moment1.Value - moment2.Value;
        //}

        //public static bool operator >(Moment moment1, Moment moment2)
        //{
        //    var compareResult = moment1.CompareTo(moment2);
        //    return compareResult > 0;
        //}

        //public static bool operator <(Moment moment1, Moment moment2)
        //{
        //    return moment2 > moment1;
        //}
    }
}
