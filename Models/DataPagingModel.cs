using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class DataPagingModel
    {
        private Dictionary<string, string> _SearchParameter = new Dictionary<string, string>();

        public DataPagingModel()
        {
            this.PageNumber = 0;
            this.PageSize = 10;
            this.TotalRecords = 0;
            this.TotalPages = 0;
            this.PageUrl = "";
            this.SearchText = "";
            this.SortingColumn = "";
            this.SortingOrder = SortingOrder.Ascending;
            this.TempData = 1;
            this.CurrentUserid = 0;
            this.CurrentRoleId = 0;
            this.IsAllRecords = false;
            this.SearchType = "";
        }

        public string ProfileId { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int StartPageNumber { get; set; }

        public int EndPageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int StartRecord { get; set; }

        public int EndRecord { get; set; }

        public int PageSize { get; set; }

        public string PageUrl { get; set; }

        public string AjaxUrl { get; set; }

        public int Incidentstatus { get; set; }

        public string SortingColumn { get; set; }

        public SortingOrder SortingOrder { get; set; }

        public string SearchText { get; set; }

        public int TempData { get; set; }

        public int CurrentUserid { get; set; }
        public int DistrictId { get; set; }

        public int CurrentRoleId { get; set; }
        public bool IsAllRecords { get; set; }

       
        public string SearchType { get; set; }

        public Dictionary<string, string> SearchParameter
        {
            get
            {
                return this._SearchParameter;
            }
            set
            {
                this._SearchParameter = value;
            }
        }
        
    }
    public enum SortingOrder
    {
        Ascending = 1,
        Descending = 2,
    }
}