using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace KeypadUWPLib
{
    public interface IKeypad
    {
         event EventHandler<KeypadEventArgs> KeyDown;
         event EventHandler<KeypadEventArgs> KeyUp;
         event EventHandler<KeypadEventArgs> KeyHolding;
    }
}
