using System;

namespace Game {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Albin_local game = new Albin_local())
                game.Run();
        }
    }
#endif
}
