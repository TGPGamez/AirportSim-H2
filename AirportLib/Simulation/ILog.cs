﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportLib
{
    public interface ILog
    {
        void Log(string message);
        void ExceptionLog(string message);
    }
}
