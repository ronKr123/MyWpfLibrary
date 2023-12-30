using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLibrary
{
    public class Filter
    {
        private string bookNameTxt;

        public string BookNameTxt { get => bookNameTxt; set => bookNameTxt = value; }

        public string BuildFilter()
        {
            string sql = $"select * from bookTbl where ";
            sql += $"bookName like '{BookNameTxt}%' ";
            return sql;
        }
    }
}

