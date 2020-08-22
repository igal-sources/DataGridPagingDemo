using DataGridPagingDemo.DataModels;
using DataGridPagingDemo.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace DataGridPagingDemo.ViewModels
{
    public class EventItems : ViewModelBase
    {
        private ObservableCollection<EventTypes> _eventTypes;
        public ObservableCollection<EventTypes> EventTypes
        {
            get { return _eventTypes; }
            set
            {
                if (_eventTypes == value) return;

                _eventTypes = value;
                OnPropertyChanged("EventTypes");
            }
        }
    }

    public class DataGridPagingViewModel : ViewModelBase
    {
        #region Properties

        public IList<EventTypes> EventTypesList { get; set; }

        private EventItems _eventTypesUI;
        public EventItems EventTypesUI
        {
            get { return _eventTypesUI; }
            set
            {
                if (_eventTypesUI == value) return;

                _eventTypesUI = value;
                OnPropertyChanged("EventTypesUI");
            }
        }

        private EventTypes _selectedEventTypesUI;
        public EventTypes SelectedEventTypesUI
        {
            get { return _selectedEventTypesUI; }
            set
            {
                if (_selectedEventTypesUI == value) return;

                _selectedEventTypesUI = value;
                OnPropertyChanged("SelectedEventTypesUI");
            }
        }

        private ICollectionView _eventTypesItemsView;
        public ICollectionView EventTypesItemsView
        {
            get { return _eventTypesItemsView; }
            set
            {
                if (_eventTypesItemsView == value) return;

                _eventTypesItemsView = value;
                OnPropertyChanged("EventTypesItemsView");
            }
        }

        private string _nameFilterText = "";
        public string NameFilterText
        {
            get { return _nameFilterText; }
            set
            {
                if (_nameFilterText == value) return;

                _nameFilterText = value;
                OnPropertyChanged("NameFilterText");
                SearchItems(value);
            }
        }


        private string _pageInfoContent;
        public string PageInfoContent
        {
            get { return _pageInfoContent; }
            set
            {
                if (_pageInfoContent == value) return;

                _pageInfoContent = value;
                OnPropertyChanged("PageInfoContent");
            }
        }

        private int _selectedNumberOfRecords;
        public int SelectedNumberOfRecords
        {
            get { return _selectedNumberOfRecords; }
            set
            {
                if (_selectedNumberOfRecords == value) return;

                _selectedNumberOfRecords = value;
                OnPropertyChanged("SelectedNumberOfRecords");
            }
        }

        private ObservableCollection<int> _numberOfRecordsUI;
        public ObservableCollection<int> NumberOfRecordsUI
        {
            get { return _numberOfRecordsUI; }
            set
            {
                if (_numberOfRecordsUI == value) return;

                _numberOfRecordsUI = value;
                OnPropertyChanged("NumberOfRecordsUI");
            }
        }

        #endregion

        private static Paging<EventTypes> PagedTable = new Paging<EventTypes>();

        public DataGridPagingViewModel()
        {
            EventTypesUI = DeserializeEvents();
            EventTypesItemsView = CollectionViewSource.GetDefaultView(EventTypesUI.EventTypes);
            EventTypesList = new List<EventTypes>(EventTypesUI.EventTypes).OrderBy(c => c.EventTypeName).ToList();
            NumberOfRecordsUI = new ObservableCollection<int>();

            PagedTable.PageIndex = 0; //Sets the Initial Index to a default value

            int[] RecordsToShow = { 10, 20, 30, 50, 100, 200 }; //This Array can be any number groups

            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecordsUI.Add(RecordGroup); //Fill the ComboBox with the Array
            }

            SelectedNumberOfRecords = 30; //Initialize the ComboBox

            EventTypesUI.EventTypes = PagedTable.SetPaging(EventTypesList, SelectedNumberOfRecords); //Fill a DataTable with the First set based on the numberOfRecPerPage
            RefreshUI();
        }

        private EventItems DeserializeEvents()
        {
            using (StreamReader reader = new StreamReader(@"C:\CIT-Stuff\MyDevelopment\DotNetSamples\DataGridPagingDemo\DataModels\event-types.json"))
            {
                string jsonString = reader.ReadToEnd();
                var events = (EventItems)JsonConvert.DeserializeObject(jsonString, typeof(EventItems));

                return events;
            }
        }

        public void SearchItems(string seatchText)
        {
            if (seatchText == string.Empty)
            {
                PagerFirstPage();
                return;
            }

            EventTypesItemsView = CollectionViewSource.GetDefaultView(EventTypesList);
            EventTypesItemsView.Refresh();
            EventTypesItemsView.Filter = new Predicate<object>(o => FilterEventTypes(o as EventTypes, seatchText));
        }

        private bool FilterEventTypes(EventTypes cls, string seatchText)
        {
            return seatchText == null || cls.EventTypeName.IndexOf(seatchText, StringComparison.OrdinalIgnoreCase) != -1;
        }

        #region Commands

        private RelayCommand<object> _firstPageCommand;

        public ICommand FirstPageCommand
        {
            get
            {
                if (_firstPageCommand == null)
                {
                    _firstPageCommand = new RelayCommand<object>(x => PagerFirstPage());
                }

                return _firstPageCommand;
            }
        }

        private RelayCommand<object> _previousPageCommand;

        public ICommand PreviousPageCommand
        {
            get
            {
                if (_previousPageCommand == null)
                {
                    _previousPageCommand = new RelayCommand<object>(x => PagerPreviousPage());
                }

                return _previousPageCommand;
            }
        }

        private RelayCommand<object> _lastPageCommand;

        public ICommand LastPageCommand
        {
            get
            {
                if (_lastPageCommand == null)
                {
                    _lastPageCommand = new RelayCommand<object>(x => PagerLastPage());
                }

                return _lastPageCommand;
            }
        }

        private RelayCommand<object> _forwardPageCommand;

        public ICommand ForwardPageCommand
        {
            get
            {
                if (_forwardPageCommand == null)
                {
                    _forwardPageCommand = new RelayCommand<object>(x => PagerForwardPage());
                }

                return _forwardPageCommand;
            }
        }

        private RelayCommand<object> _selectedNumberOfRecordsChangedCommand;

        public ICommand SelectedNumberOfRecordsChangedCommand
        {
            get
            {
                if (_selectedNumberOfRecordsChangedCommand == null)
                {
                    _selectedNumberOfRecordsChangedCommand = new RelayCommand<object>(x => PagerFirstPage());
                }

                return _selectedNumberOfRecordsChangedCommand;
            }
        }

        #endregion Commands

        private void PagerFirstPage()
        {
            EventTypesUI.EventTypes = PagedTable.First(EventTypesList, SelectedNumberOfRecords);
            RefreshUI();
        }

        private void PagerPreviousPage()
        {
            EventTypesUI.EventTypes = PagedTable.Previous(EventTypesList, SelectedNumberOfRecords);
            RefreshUI();
        }

        private void PagerLastPage()
        {
            EventTypesUI.EventTypes = PagedTable.Last(EventTypesList, SelectedNumberOfRecords);
            RefreshUI();
        }

        private void PagerForwardPage()
        {
            EventTypesUI.EventTypes = PagedTable.Next(EventTypesList, SelectedNumberOfRecords);
            RefreshUI();
        }

        private void RefreshUI()
        {
            EventTypesItemsView = CollectionViewSource.GetDefaultView(EventTypesUI.EventTypes);
            EventTypesItemsView.Refresh();
            PageInfoContent = PageNumberDisplay();
        }

        private string PageNumberDisplay()
        {
            int PagedNumber = SelectedNumberOfRecords * (PagedTable.PageIndex + 1);
            if (PagedNumber > EventTypesList.Count)
            {
                PagedNumber = EventTypesList.Count;
            }
            return PagedNumber + " of " + EventTypesList.Count; //This dramatically reduced the number of times I had to write this string statement
        }
    }
}