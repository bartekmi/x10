using System.Windows;
using System.Windows.Controls;

namespace wpf_sample.ui.bookings {
  public partial class BookingForm : UserControl {

    private BookingFormVM ViewModel { get { return (BookingFormVM)DataContext; } }

    public BookingForm() {
      InitializeComponent();
      DataContext = new BookingFormVM(this);
    }

    // Event handlers
    private void SelectConsigneeBooking(object sender, RoutedEventArgs e) {
      ViewModel.SelectConsigneeBooking();
    }
    private void SelectShipperBooking(object sender, RoutedEventArgs e) {
      ViewModel.SelectShipperBooking();
    }

    private void SubmitClick(object sender, RoutedEventArgs e) {
      ViewModel.SubmitData(() => AppStatics.Singleton.DataSource.UpdateOrCreate(ViewModel.Model),
        "Booking saved");
    }
  }
}
