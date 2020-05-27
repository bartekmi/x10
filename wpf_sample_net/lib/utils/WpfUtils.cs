using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace wpf_sample.lib.utils {
  public static class WpfUtils {
    public static T FindVisualChild<T>(FrameworkElement root, string name = null) where T : FrameworkElement {
      DependencyObject result = FindVisualChild(typeof(T), root, name);
      return (T)result;
    }

    public static DependencyObject FindVisualChild(Type type, FrameworkElement root, string name = null) {
      if (name == null) {
        if (root.GetType() == type)
          return root;
      } else
          if (root.Name == name)
        return root;

      int count = VisualTreeHelper.GetChildrenCount(root);
      for (int ii = 0; ii < count; ii++) {
        FrameworkElement child = VisualTreeHelper.GetChild(root, ii) as FrameworkElement;
        if (child == null) continue;
        DependencyObject found = FindVisualChild(type, child, name);
        if (found != null)
          return found;
      }

      return null;
    }

    public static List<T> FindLogicalChildren<T>(FrameworkElement root) where T : FrameworkElement {
      List<T> children = new List<T>();
      FindLogicalChildren(children, root);
      return children;
    }

    private static void FindLogicalChildren<T>(List<T> children, FrameworkElement node) where T : FrameworkElement {
      foreach (object child in LogicalTreeHelper.GetChildren(node))
        if (child is FrameworkElement)
          FindLogicalChildren(children, child as FrameworkElement);

      if (node is T)
        children.Add((T)node);
    }

    public static T FindAncestorVM<T>(FrameworkElement element) where T : class {
      while (element != null && !(typeof(T).IsAssignableFrom(element.DataContext.GetType())))
        element = (FrameworkElement)VisualTreeHelper.GetParent(element);

      return element.DataContext as T;
    }

    public static T FindAncestor<T>(FrameworkElement element) where T : class {
      while (element != null && !(typeof(T).IsAssignableFrom(element.GetType())))
        element = (FrameworkElement)VisualTreeHelper.GetParent(element);

      return element as T;
    }

    // Find the first non-null Tag, going up the parent chain
    public static object FindAncestorTag(FrameworkElement element, bool canReturnMyself = true) {
      if (!canReturnMyself)
        element = (FrameworkElement)VisualTreeHelper.GetParent(element);

      while (element != null && element.Tag == null)
        element = (FrameworkElement)VisualTreeHelper.GetParent(element);

      return element == null ? null : element.Tag;
    }

    // Find the first element that allows drop, starting with myself and going up the parent chain
    public static FrameworkElement FindAncestorAllowDrop(FrameworkElement element) {
      if (!element.AllowDrop)
        return null;

      while (element != null) {
        if (DependencyPropertyHelper.GetValueSource(element, UIElement.AllowDropProperty).BaseValueSource == BaseValueSource.Inherited)
          element = (FrameworkElement)VisualTreeHelper.GetParent(element);
        else
          break;
      }

      return element;
    }

    // Find the first non-empty Name, starting with myself and going up the parent chain
    public static string FindAncestorName(FrameworkElement element) {
      while (element != null && string.IsNullOrWhiteSpace(element.Name))
        element = (FrameworkElement)VisualTreeHelper.GetParent(element);

      return element == null ? null : element.Name;
    }

    public static Binding CreateBinding(string path, IValueConverter converter = null, object converterParameter = null) {
      Binding binding = new Binding(path);

      if (converter != null)
        binding.Converter = converter;
      if (converterParameter != null)
        binding.ConverterParameter = converterParameter;

      return binding;
    }
  }
}
