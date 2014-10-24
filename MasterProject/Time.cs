using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterProject
{
    class Time
    {
        public double hour;
        public double minute;
        public double second;

        public Time()
        {
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;
            second = DateTime.Now.Second;
        }

        public override string ToString()
        {
            return hour + ":" + minute + ":" + second;
        }
    }
}
