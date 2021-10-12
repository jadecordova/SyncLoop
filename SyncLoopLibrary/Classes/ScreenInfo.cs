using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Screen information.
    /// </summary>
    public class ScreenInfo
    {

        #region MEMBERS

        /// <summary>
        /// Vertical border width of window.
        /// </summary>
        double verticalBorderWidth = SystemParameters.BorderWidth;

        /// <summary>
        /// Interop helper.
        /// </summary>
        WindowInteropHelper helper;

        /// <summary>
        /// Current monitor screen.
        /// </summary>
        Screen currentScreen;

        /// <summary>
        /// Current form.
        /// </summary>
        Window window;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// Screen width.
        /// </summary>
        public int ScreenWidth { get; set; }

        /// <summary>
        /// Screen height.
        /// </summary>
        public int ScreenHeight { get; set; }

        #endregion



        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="window">Window object to get info from.</param>
        public ScreenInfo(Window window)
        {
            this.window = window;

            helper = new WindowInteropHelper(window);

            currentScreen = Screen.FromHandle(helper.Handle);

            ScreenWidth = currentScreen.WorkingArea.Width;

            ScreenHeight = currentScreen.WorkingArea.Height;

        }

        #endregion



        #region METHODS

        /// <summary>
        /// Gets default coordinates of current window.
        /// </summary>
        /// <returns>Coordinates point.</returns>
        public Point GetVideoWindowDefaultCoordinates()
        {
            // Width of video window borders.
            double width = 0;
            // Get width.
            width = SystemParameters.WindowNonClientFrameThickness.Left +
                    SystemParameters.WindowNonClientFrameThickness.Right +
                    SystemParameters.WindowResizeBorderThickness.Left +
                    SystemParameters.WindowResizeBorderThickness.Right;
            
            // Get left and top coordinates.
            return new Point
            {
                X = ScreenWidth - window.Width - width,
                Y = 1
            };
        }

        #endregion
    }
}
