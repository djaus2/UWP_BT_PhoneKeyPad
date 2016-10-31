using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPadKeysUWPLib
{
    public class KeyFunctions
    {
        KeypadUWPLib.Keypad Keypad = null;

        public delegate void Keypress();
        Dictionary<char, Keypress> Keypresses = null;


        public KeyFunctions(KeypadUWPLib.Keypad keypad)
        {
            Keypresses = new Dictionary<char, Keypress>();

            foreach (char ch in KeypadUWPLib.KeypadEventArgs.ValidKeys)
            {
                Keypresses.Add(ch, null);
            }

            Keypad = keypad;
            Keypad.KeyDown += Keypad_KeyDown;
        }

        public void Set(Keypress del, char ch)
        {
            if (KeypadUWPLib.KeypadEventArgs.ValidKeys.Contains(ch))
            {
                Keypresses[ch] = del;
            }
        }

        public Delegate Get( char ch)
        {
            return Keypresses[ch];
        }

        public void Clear(char ch)
        {
            Keypresses[ch] = null;
        }

        public void Action(char ch)
        {
            if (Keypresses[ch] != null)
            {
                Keypress del = Keypresses[ch];
                del();
            }
        }

        /// <summary>
        /// KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Keypad_KeyDown(object sender, KeypadUWPLib.KeypadEventArgs e)
        {
            char cmd = e.Key;
            Action(cmd);
        }
    }
}
