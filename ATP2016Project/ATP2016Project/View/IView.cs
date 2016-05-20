using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.View
{
    interface IView
    {
        void Start();
        void Output(string output);
        string input();
    }
}
