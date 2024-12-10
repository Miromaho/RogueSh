﻿using RoguelikeCL.Core;
using RoguelikeCL.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoguelikeCL.interfaces
{
    public interface IBehavior
    {
        bool Act(Enemy enemy, CommandSys commandSys);
    }
}
