using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Gets FlowDocument descendants.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Gets descendants from a root element.
        /// </summary>
        /// <param name="root">Root element.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> Descendants(this DependencyObject root)
        {
            if (root == null)
                yield break;

            yield return root;

            foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
                foreach (var descendent in child.Descendants())
                    yield return descendent;
        }
    }

}
