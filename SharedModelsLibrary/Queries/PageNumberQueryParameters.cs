using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerLibrary.Queries
{
    public class PageNumberQueryParameters
    {
        private int _maxPageSize = 100;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = Math.Min(value, _maxPageSize);
            }
        }
    }
}
