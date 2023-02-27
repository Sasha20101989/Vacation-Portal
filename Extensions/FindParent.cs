using System.Windows;
using System.Windows.Media;

namespace Vacation_Portal.Extensions {
    public static class FindParent {
        public static T FindParentOfType<T>(this DependencyObject child) where T : DependencyObject {
            DependencyObject parentDepObj = child;
            do {
                parentDepObj = VisualTreeHelper.GetParent(parentDepObj);
                T parent = parentDepObj as T;
                if(parent != null) {
                    return parent;
                }
            }
            while(parentDepObj != null);
            return null;
        }
    }
}
