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
      AppStatics.Create();

      InitializeComponent();
      InitializeContext();

      AppStatics.Singleton.Navigation = uxWpfStoryBook;
      uxWpfStoryBook.InitializeComponents(Components());
      uxWpfStoryBook.NavigateToUrl("/buildings");
    }

    private void InitializeContext() {
      AppStatics.Singleton.Context = new __Context__() {
        Today = DateTime.Today,
      };
    }

    private static List<Type> Components() {
      return new List<Type>() {
        typeof(BuildingForm),
        typeof(Buildings),
      };
    }
  }
}
