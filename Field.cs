using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tron
{
    class Field
    {
        public bool avaible { get; set; }
        public int MotorId { get; set; }

        public Field()
        {
            avaible = true;
            MotorId = 0;
        }
    }
}
