using System;


namespace Framework.Support
{

    public class PagingInfo
    {
        private int _currentPage;
        private int _pageSize;
        private int _totalRows;

        public PagingInfo()
        {
            this._pageSize = 10;
            this._currentPage = 1;
        }

        public PagingInfo(int pageSize, int currentPage)
        {
            this._pageSize = 10;
            this._currentPage = 1;
            this._pageSize = pageSize;
            this._currentPage = currentPage;
        }

        public int getCurrentRow()
        {
            return (this._pageSize * (this._currentPage - 1));
        }

        public int getTotalPages()
        {
            if (this._totalRows <= 0)
            {
                return 0;
            }
            return (((this._totalRows - 1) / this._pageSize) + 1);
        }

        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
            set
            {
                this._currentPage = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                this._pageSize = value;
            }
        }

        public int TotalRows
        {
            get
            {
                return this._totalRows;
            }
            set
            {
                this._totalRows = value;
            }
        }
    }
}

