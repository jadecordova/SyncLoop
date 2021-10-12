using System.Windows;
using System.Windows.Input;

namespace SyncLoop
{
    public partial class TextEditor : Window
    {
        private void EditShortcuts_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // The variable shortcuts is defined in TextEditor.xaml.cs
        private void EditShortcuts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Create new settings editor.
            ShorcutsEditor editor = new ShorcutsEditor();

            editor.DataContext = shortcuts;

            // Set maximum length of menu texts.
            int max = 20;
            
            // Update menu text. Check for a maximum of 20 displayed characters.
            if (editor.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(shortcuts[0]))
                {
                    if(shortcuts[0].Length <= max)
                    {
                        Short1.Header = shortcuts[0];
                    }
                    else
                    {
                        Short1.Header = shortcuts[0].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[1]))
                {
                    if(shortcuts[1].Length <= max)
                    {
                        Short2.Header = shortcuts[1];
                    }
                    else
                    {
                        Short2.Header = shortcuts[1].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[2]))
                {
                    if(shortcuts[2].Length <= max)
                    {
                        Short3.Header = shortcuts[2];
                    }
                    else
                    {
                        Short3.Header = shortcuts[2].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[3]))
                {
                    if(shortcuts[3].Length <= max)
                    {
                        Short4.Header = shortcuts[3];
                    }
                    else
                    {
                        Short4.Header = shortcuts[3].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[4]))
                {
                    if(shortcuts[4].Length <= max)
                    {
                        Short5.Header = shortcuts[4];
                    }
                    else
                    {
                        Short5.Header = shortcuts[4].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[5]))
                {
                    if(shortcuts[5].Length <= max)
                    {
                        Short6.Header = shortcuts[5];
                    }
                    else
                    {
                        Short6.Header = shortcuts[5].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[6]))
                {
                    if(shortcuts[6].Length <= max)
                    {
                        Short7.Header = shortcuts[6];
                    }
                    else
                    {
                        Short7.Header = shortcuts[6].Substring(0, max) + "...";
                    }
                }

                if (!string.IsNullOrEmpty(shortcuts[7]))
                {
                    if(shortcuts[7].Length <= max)
                    {
                        Short8.Header = shortcuts[7];
                    }
                    else
                    {
                        Short8.Header = shortcuts[7].Substring(0, max) + "...";
                    }
                }
            }
        }
    }
}
