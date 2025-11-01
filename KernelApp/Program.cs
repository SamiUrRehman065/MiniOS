using System;
using System.Windows.Forms;

namespace KernelApp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Boot message (if passed by our ASM bootloader later)
            string bootMessage = args.Length > 0 ? string.Join(" ", args) : "Booted via ASM loader";

            Application.Run(new MainForm());
        }
    }
}
