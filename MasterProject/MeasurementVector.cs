using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterProject
{
    public class MeasurementVector
    {
        public float dimension1;
        public float dimension2;

        public MeasurementVector()
        {
        }

        public MeasurementVector(float d1, float d2)
        {
            dimension1 = d1;
            dimension2 = d2;
        }
    }
}
