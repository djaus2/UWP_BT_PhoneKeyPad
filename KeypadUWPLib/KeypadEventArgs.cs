using System;
using System.Collections.Generic;

namespace KeypadUWPLib
{
    public enum KeypadActions { Down, Up, Holding }
    public  class KeypadEventArgs 
    {
        /// <summary>
        /// Valid chars from keypad
        /// </summary>
        List<char> validActions = new List<char>() { '+', '-', '@' };
        public static List<char> ValidKeys = new List<char>()
        { '0', '1', '2', '3',
            '4', '5', '6', '7', '8','9', '#', '*', 'C','T' };
        //If you have other keys just add them here, like C and T

        public KeypadEventArgs(KeypadActions action, char key)
        {

            if (!ValidKeys.Contains(key))
            {
                //Dispose
            }
            else
            {
                Key = key;
                Action = action;
            }
        }

        public KeypadEventArgs(char action, char key)
        {
            if (!ValidKeys.Contains(key))
            {
                //Dispose
            }else
            {
                Key = key;
                if (!validActions.Contains(action))
                {
                    //Dispose
                }
                else
                {
                    switch (action)
                    {
                        case '+':
                            Action = KeypadActions.Down;
                            break;
                        case '-':
                            Action = KeypadActions.Up;
                            break;
                        case '@':
                            Action = KeypadActions.Holding;
                            break;
                    }
                }
            }
        }

        public char Key { get; internal set; }
        public KeypadActions Action { get; internal set; }
    }


}