﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Interface
{
    public interface IController
    {
        void Initialize();

        void Run();

        void Shutdown();
    }
}
