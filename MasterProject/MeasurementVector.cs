using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterProject
{
    public class MeasurementVector
    {
        public int dimension1;
        public int dimension2;

        public MeasurementVector()
        {
        }

        public MeasurementVector(int d1, int d2)
        {
            dimension1 = d1;
            dimension2 = d2;
        }
    }
}
