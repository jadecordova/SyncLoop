namespace SyncLoopLibrary
{
    /// <summary>
    /// Defines the current video engine used in the application.
    /// Options are:
    /// QuickTime (which requires the presence of QuickTime installed in the user machine),
    /// FFME, an implementation of FFMPG for .NET,
    /// Internal, which uses VisualStudio MediaElement.
    /// </summary>
    public enum VideoMode
    {
        /// <summary>
        /// QuickTime.
        /// </summary>
        QuickTime,
        /// <summary>
        /// FFME.
        /// </summary>
        FFME,
        /// <summary>
        /// Internal MediaElement.
        /// </summary>
        Internal
    }
}
