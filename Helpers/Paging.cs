using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataGridPagingDemo.Helpers
{
    public class Paging<T>
    {
        /// <summary>
        /// Current Page Index Number
        /// </summary>
        public int PageIndex { get; set; }

        ObservableCollection<T> PagedList;

        /// <summary>
        /// Show the next set of Items based on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public ObservableCollection<T> Next(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show the previous set of items base on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public ObservableCollection<T> Previous(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show first the set of Items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public ObservableCollection<T> First(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show the last set of items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public ObservableCollection<T> Last(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Performs a LINQ Query on the List and returns a DataTable
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
		public ObservableCollection<T> SetPaging(IList<T> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<T> PagedList = new List<T>();

            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

            ObservableCollection<T> FinalPaging = PagedTable(PagedList);

            return FinalPaging;
        }

        //If youre paging say 30,000 rows and you know the processors of the users will be slow you can ASync thread both of these to allow the UI to update after they finish and prevent a hang.

        /// <summary>
        /// Internal Method: Performs the Work of converting the Passed in list to a DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SourceList"></param>
        /// <returns>DataTable</returns>
		private ObservableCollection<T> PagedTable(IList<T> SourceList)
        {
            Type columnType = typeof(T);
            ObservableCollection<T> CollectionToReturn = new ObservableCollection<T>();

            foreach (T item in SourceList)
            {
                CollectionToReturn.Add(item);
            }

            return CollectionToReturn;
        }
    }
}
