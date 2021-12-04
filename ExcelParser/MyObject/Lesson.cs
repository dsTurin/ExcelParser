﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExcelParser.MyObject
{
    public class Lesson
    {
        public string name { get; set; }
        public string type { get; set; }
        public string teacher { get; set; }
        public string corpus { get; set; }
        public string classRoom { get; set; }
        public string description { get; set; }

        //Тестовый метод разбора
        public static Lesson GetLesson(string rowFirst, string rowSecond)
        {
            if (string.IsNullOrWhiteSpace(rowFirst))
                return new Lesson();

            Lesson l = new Lesson();

            string[] s1 = rowFirst.Split('-');

            if(s1.Length > 2)
                l.name = s1[0] ?? "" + s1[1] ?? "";

            if (s1.Length > 2)
                l.type = s1[2] ?? "";
            else if(s1.Length > 1)
                l.type = s1[1] ?? "";

            string[] s2 = rowSecond.Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();

            if (!string.IsNullOrWhiteSpace(rowSecond))
            {
                //string[] s2 = rowSecond.Split(' ');
                if (s1.Length > 2)
                    l.teacher = s2[0] ?? "" + s1[1] ?? "";
                else 
                    l.teacher = s2[0] ?? "";

                if (s2.Length > 2)
                {
                    //string[] s3 = s2.Where(w => !string.IsNullOrWhiteSpace(w)).ToArray()[1]?.Split('-');
                    string[] s3 = s2[2]?.Split('-');
                    l.corpus = s3[0] ?? "";
                    l.classRoom = s3[1] ?? "";
                }
                else if (s2.Length == 2)
                {
                    string[] s3 = s2[1]?.Split('-');
                    l.corpus = s3[0] ?? "";
                    l.classRoom = s3[1] ?? "";
                }
                
            }
            

            return l;
        }
    }

}
