using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_generated.entities;
using wpf_generated.ui;

namespace wpf_generated {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      InitializeContext();

      uxWpfStoryBook.InitializeComponents(Components());
    }

    private void InitializeContext() {
      AppStatics.Singleton.Context = new __Context__();
    }

    private static List<Type> Components() {
      return new List<Type>() {
        typeof(BuildingForm),
        typeof(Buildings),
      };
    }
  }
}
