﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public enum ReservationStatus
    {
        InPreparation,
        Issued,
        Returned,
        Lost,
        Destroyed
    }
}
