using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Models
{
    public class PageHistory
    {
        private List<string> pageList;

        public PageHistory() {
            pageList = new();
        }

        public void AddPage(string pageName)
        {
            string pageStr = pageName;
            pageList.Add(pageStr);
            //System.Diagnostics.Debug.WriteLine(pageStr + " Added");
        }

        public string GetPreviousPage()
        {
            if(IsPrevious())
            {
                string returnStr = pageList[pageList.Count - 1];
                pageList.RemoveAt(pageList.Count - 1);
                return "/" + returnStr;
            }

            return "/";
        }

        public bool IsPrevious()
        {
            if(pageList.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
