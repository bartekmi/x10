﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace wpf_generated {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      InitializeContext();

      uxWpfStoryBook.InitializeComponents(Components());
    }

    private void InitializeContext() {
      //AppStatics.Singleton.Context = new __Context__() {
      //  User = new User() {
      //    FirstName = "Bartek",
      //    LastName = "Muszynski",
      //    Company = AppStatics.Singleton.DataSource.Companies.First(),
      //  }
      //};
    }

    private static List<Type> Components() {
      return new List<Type>() {
        //typeof(BookingForm),
        //typeof(Bookings),
      };
    }
  }
}
