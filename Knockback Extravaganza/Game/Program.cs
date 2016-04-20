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
            using (Microsoft.Xna.Framework.Game game = new Game1())
                game.Run();
        }
    }
#endif
}
