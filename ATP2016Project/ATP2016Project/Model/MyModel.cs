using ATP2016Project.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model
{
    class MyModel : IModel
    {
        private IController m_controller;

        public MyModel(IController controller)
        {
            m_controller = controller;
        }
    }
}
