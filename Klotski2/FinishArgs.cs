using System;

namespace Klotski
{
    class FinishArgs : EventArgs
    {
        public bool IsAuto { get; }

        public FinishArgs(bool isAuto)
        {
            IsAuto = isAuto;
        }
    }
}