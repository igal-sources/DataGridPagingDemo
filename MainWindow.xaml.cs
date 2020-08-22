using DataGridPagingDemo.ViewModels;
using System.Windows;

namespace DataListRealTimeLoadDemo
{
    public partial class MainWindow : Window
    {
        private DataGridPagingViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new DataGridPagingViewModel();
            DataContext = _viewModel;
        }
    }
}
