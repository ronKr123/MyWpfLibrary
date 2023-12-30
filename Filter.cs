using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfLibrary
{
    public class Filter
    {
        private string bookNameTxt;

        private int genreBookCode;

        private int writerBookCode;

        public string BookNameTxt { get => bookNameTxt; set => bookNameTxt = value; }
        public int GenreBookCode { get => genreBookCode; set => genreBookCode = value; }
        public int WriterBookCode { get => writerBookCode; set => writerBookCode = value; }


        public string BuildFilter()
        {

            int codeSwich = 0;
            if(bookNameTxt != null && genreBookCode != null && writerBookCode != null)
            {
                codeSwich = 1;
            }
            else
            {
                if (bookNameTxt != null && genreBookCode != null)
                {
                    codeSwich = 2;
                }
                else
                {
                    if(genreBookCode != null && writerBookCode != null)
                    {
                        codeSwich = 3;
                    }
                    else
                    {
                        if(writerBookCode != null && bookNameTxt != null)
                        {
                            codeSwich = 4;
                        }
                        else
                        {
                            if(bookNameTxt != null && genreBookCode == null && writerBookCode == null)
                            {
                                codeSwich = 5;
                            }
                            else
                            {
                                if(bookNameTxt == null && genreBookCode != null && writerBookCode == null)
                                {
                                    codeSwich = 6;
                                }
                                else
                                {
                                    if(bookNameTxt == null && genreBookCode == null && writerBookCode != null)
                                    {
                                        codeSwich = 7;
                                    }
                                    else
                                    {
                                        codeSwich = 8;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            // Using Swich Case

            switch (codeSwich)
            {
                case 1:
                    return $"select * from bookTbl where bookName like '{BookNameTxt}%' and genreCode = {GenreBookCode} and writerCode = {WriterBookCode} ";
                    break;
                case 2:
                    return $"select * from bookTbl where bookName like '{BookNameTxt}%' and genreCode = {GenreBookCode}";
                    break;
                case 3:
                    return $"select * from bookTbl where genreCode = {GenreBookCode} and writerCode = {WriterBookCode}";
                    break;
                case 4:
                    return $"select * from bookTbl where bookName like '{BookNameTxt}%' and writerCode = {WriterBookCode}";
                    break;
                case 5:
                    return $"select * from bookTbl where bookName like '{BookNameTxt}%' ";
                    break;
                case 6:
                    return $"select * from bookTbl where genreCode = {GenreBookCode} ";
                    break;
                case 7:
                    return $"select * from bookTbl where writerCode = {WriterBookCode} ";
                    break;
                case 8:
                    return $"select * from bookTbl";
                    break;
                default:
                    return $"select * from bookTbl";
                    break;
            }

        }
    }
}

