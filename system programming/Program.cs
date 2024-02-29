using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace system_programming
{
    internal class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, MessageBoxOptions options);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam); // тут я змінив замість IntPtr на string для lParam

        const uint WM_CLOSE = 0x0010; // для SendMessage, щоб вікно закривалось
        const uint WM_SETTEXT = 0x000C; // для SendMessage, щоб вікно змінювало назву



        static void Main(string[] args)
        {
            int answer;
            Console.WriteLine("1 - Hello World, \n2 - Computer Number Guesser, \n3 - Find and Close Notepad, \n4 - Make notepad title a time");
            int.TryParse(Console.ReadLine(), out answer);

            Console.Clear();

            switch (answer)
            {
                case 1: MessageBox(IntPtr.Zero, "Hello, World!", "Message", 0); break;

                case 2: WindowGame(); break;

                case 3: CloseNotepad(); break;

                case 4: ChangeNotepadTitle(); break;

                default: break;
            }
        }

        public static void ChangeNotepadTitle()
        {
            IntPtr handle = FindWindow("Notepad", null);
            if (handle == IntPtr.Zero)
            {
                MessageBox(IntPtr.Zero, "Notepad was NOT found!", "Information", MessageBoxOptions.OK | MessageBoxOptions.IconInformation);
            }
            else
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (FindWindow("Notepad", null) == IntPtr.Zero)
                    {
                        return;
                    }
                    SendMessage(handle, WM_SETTEXT, IntPtr.Zero, DateTime.Now.ToLongTimeString());
                }
            }
        }

        public static void CloseNotepad()
        {
            IntPtr handle = FindWindow("Notepad", null);
            if (handle == IntPtr.Zero)
            {
                MessageBox(IntPtr.Zero, "Notepad was NOT found!", "Information", MessageBoxOptions.OK | MessageBoxOptions.IconInformation);
            }
            else
            {
                MessageBox(IntPtr.Zero, "Notepad was found!", "Information", MessageBoxOptions.OK | MessageBoxOptions.IconInformation);
                var answer = MessageBox(IntPtr.Zero, "Do you want to close it?", "Closing the notepad", MessageBoxOptions.YesNo | MessageBoxOptions.IconQuestion);
                if (answer == 6)
                {
                    SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                else if (answer == 7)
                {
                    return;
                }
            }
        }

        public static void WindowGame()
        {
            bool isAgain = true;

            while (isAgain)
            {
                PlayGame();
                var answer = MessageBox(IntPtr.Zero, "Do you want to play again?", "Game", MessageBoxOptions.YesNo | MessageBoxOptions.IconQuestion);
                if (answer == 6)
                {
                    isAgain = true;
                }
                else if (answer == 7)
                {
                    isAgain = false;
                }

            }
        }
        static void PlayGame()
        {
            int guess;
            int minRange = 0;
            int maxRange = 100;
            int userInput;

            Console.WriteLine($"Enter number from {minRange} to {maxRange}");
            int.TryParse(Console.ReadLine(), out userInput);

            do
            {
                guess = (minRange + maxRange) / 2;
                Console.WriteLine($"Is your number - {guess}? Yes - 1 Less - 2 More - 3");
                int.TryParse(Console.ReadLine(), out userInput);

                if (userInput == 2)
                {
                    maxRange = guess - 1;
                }
                else if (userInput == 3)
                {
                    minRange = guess + 1;
                }

            } while (userInput != 1);

            Console.WriteLine($"Your number is - {guess}!");
        }
    }

    public enum MessageBoxOptions : uint
    {
        OK = 0x00000000,
        OKCancel = 0x00000001,
        AbortRetryIgnore = 0x00000002,
        YesNoCancel = 0x00000003,
        YesNo = 0x00000004,
        RetryCancel = 0x00000005,
        CancelTryContinue = 0x00000006,
        IconError = 0x00000010,
        IconQuestion = 0x00000020,
        IconWarning = 0x00000030,
        IconInformation = 0x00000040,
        UserIcon = 0x00000080
    }
}


