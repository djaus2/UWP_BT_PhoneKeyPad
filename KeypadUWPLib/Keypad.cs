using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace KeypadUWPLib
{
    public class Keypad : IKeypad
    {
        private EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
                m_KeyDownTokenTable = null;

        public event EventHandler<KeypadEventArgs> KeyDown
        {
            add
            {
                /*return*/ EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref m_KeyDownTokenTable)
                    .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref m_KeyDownTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        private EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
            m_KeyUpTokenTable = null;

        public event EventHandler<KeypadEventArgs> KeyUp
        {
            add
            {
                /*return*/
                EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
         .GetOrCreateEventRegistrationTokenTable(ref m_KeyUpTokenTable)
         .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref m_KeyUpTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        private EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
    m_KeyHoldingTokenTable = null;

        public event EventHandler<KeypadEventArgs> KeyHolding
        {
            add
            {
                /*return*/
                EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
         .GetOrCreateEventRegistrationTokenTable(ref m_KeyHoldingTokenTable)
         .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref m_KeyHoldingTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        //internal void OnKeyUp(char key, KeypadActions action )
        //{
        //    EventHandler<KeypadEventArgs> temp =
        //        EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
        //        .GetOrCreateEventRegistrationTokenTable(ref m_KeyUpTokenTable)
        //        .InvocationList;
        //    if (temp != null)
        //    {
        //        temp(this, new KeypadEventArgs(key, action ));
        //    }
        //}

        //internal void OnKeyDown(char key, KeypadActions action)
        //{
        //    EventHandler<KeypadEventArgs> temp =
        //        EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
        //        .GetOrCreateEventRegistrationTokenTable(ref m_KeyDownTokenTable)
        //        .InvocationList;
        //    if (temp != null)
        //    {
        //        temp(this, new KeypadEventArgs(key, action));
        //    }
        //}

        //internal void OnKeyJolding(char key, KeypadActions action)
        //{
        //    EventHandler<KeypadEventArgs> temp =
        //        EventRegistrationTokenTable<EventHandler<KeypadEventArgs>>
        //        .GetOrCreateEventRegistrationTokenTable(ref m_KeyHoldingTokenTable)
        //        .InvocationList;
        //    if (temp != null)
        //    {
        //        temp(this, new KeypadEventArgs(key, action));
        //    }
        //}





        public void RaiseEvent(object sender,   KeypadEventArgs e)
        {

            // Action relevant event
            switch (e.Action)
            {
                case KeypadActions.Down:
                    if (m_KeyDownTokenTable == null)
                        return;
                    m_KeyDownTokenTable.InvocationList.Invoke(sender, e);
                    break;
                case KeypadActions.Up:
                    if (m_KeyUpTokenTable == null)
                        return;
                    m_KeyUpTokenTable.InvocationList.Invoke(sender, e);
                    break;
                case KeypadActions.Holding:
                    if (m_KeyHoldingTokenTable == null)
                        return;
                    m_KeyHoldingTokenTable.InvocationList.Invoke(sender,  e);
                    break;
            }
        }



        public void RaiseEvent(object sender, string keyString)
        {
            //Validate keyString
            if (keyString.Length != 2)
                return;

            KeypadEventArgs e = null;
            char action = keyString[0];
            char key = keyString[1];
            e = new KeypadEventArgs(action, key);

            RaiseEvent(sender, e);
        }


    }
}
