using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExcelParser.MyObject
{
    public class Parser
    {
        public static void StartParser()
        {
            string path = "raspisanie.xls";
            List<Group> groupList = new List<Group>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // Преобразовываем excel в dataSet
                    var result = reader.AsDataSet();

                    List<Group> listGroups = new List<Group>();

                    

                    foreach (DataTable dt in result.Tables)
                    {
                        //if (dt.TableName.Contains("5 курс") || dt.TableName.Equals("магистратура"))
                        //    continue;

                        var indexs = GetValueIndex(dt.TableName);
                        int fWeekIndexStart = indexs.f1;
                        int fWeekIndexEnd = indexs.f2;
                        int sWeekIndexStart = indexs.f3;
                        int sWeekIndexEnd = indexs.f4;


                        //Первая неделя расписания

                        for (int i = fWeekIndexStart; i <=fWeekIndexEnd ; i++)
                        {
                            Group group = new Group();
                            group.paraList = new List<Para>();
                            group.weekNumber = int.Parse(dt.Rows[0][0].ToString());
                            group.groupName = dt.Rows[0][i].ToString();

                            int indexDay = 1;
                            for (int j = 1; j < dt.Rows.Count; j++)
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[j][0].ToString()) && indexDay < 7)
                                {
                                    //Встали на строку с днем недели и от нее начинаем бегать по всем парам
                                    for (int k = j; k < dt.Rows.Count-1; k++)
                                    {
                                        //Производим полную инициализацию
                                        Para para = new Para();
                                        para.timeList = new List<Time>();
                                        para.lesson = new Lesson();
                                        
                                        //Получаем день недели
                                        para.dayOfWeek = dt.Rows[j][0].ToString();
                                        
                                        //Получаем время
                                        for (int l = 0; l < 2; l++)
                                        {
                                            string[] t = dt.Rows[k + l][1].ToString().Split("-");
                                            if (t.Length > 1)
                                            {
                                                Time time = new Time();
                                                time.timeStart = t[0] ?? "";
                                                time.timeEnd = t[1] ?? "";
                                                para.timeList.Add(time);
                                            }
                                        }

                                        //Получаем занятия и приписку с Преподавателем, корпусом и кабинетом; i - колонка группы
                                        para.lesson = Lesson.GetLesson(dt.Rows[k][i].ToString(), dt.Rows[k + 1][i].ToString());
                                        //para.lesson.name = dt.Rows[k][i].ToString();
                                        //para.lesson.description = dt.Rows[k + 1][i].ToString();

                                        //Получаем номер пары
                                        //Где то в конце расписания нет номера пары, но есть время, значит она восьмая
                                        para.Number = string.IsNullOrWhiteSpace(dt.Rows[k][2].ToString()) ? 8 : int.Parse(dt.Rows[k][2].ToString());
                                        
                                        if (para.Number == 8 && !group.paraList
                                                .Where(w => w.dayOfWeek == para.dayOfWeek).ToList()
                                                .Exists(e => e.Number == 7))
                                        {
                                            para.Number = 7;
                                        }

                                        if (group.paraList.Where(w=>w.dayOfWeek==para.dayOfWeek).ToList().Exists(e=>e.Number == para.Number)) 
                                            break;

                                        k++;
                                        group.paraList.Add(para);
                                    }
                                    
                                    indexDay++;
                                }
                            }
                           
                            listGroups.Add(group);
                        }

                        //Вторая неделя расписания
                        for (int i = sWeekIndexStart; i <= sWeekIndexEnd; i++)
                        {
                            Group group = new Group();
                            group.paraList = new List<Para>();
                            group.weekNumber = int.Parse(dt.Rows[0][0].ToString());
                            group.groupName = dt.Rows[0][i].ToString();

                            int indexDay = 1;
                            for (int j = 1; j < dt.Rows.Count; j++)
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[j][0].ToString()) && indexDay < 7)
                                {
                                    //Встали на строку с днем недели и от нее начинаем бегать по всем парам
                                    for (int k = j; k < dt.Rows.Count-1; k++)
                                    {
                                        //Производим полную инициализацию
                                        Para para = new Para();
                                        para.timeList = new List<Time>();
                                        para.lesson = new Lesson();

                                        //Получаем день недели
                                        para.dayOfWeek = dt.Rows[j][0].ToString();

                                        //Получаем время
                                        for (int l = 0; l < 2; l++)
                                        {
                                            string[] t = dt.Rows[k + l][1].ToString().Split("-");
                                            if (t.Length > 1)
                                            {
                                                Time time = new Time();
                                                time.timeStart = t[0] ?? "";
                                                time.timeEnd = t[1] ?? "";
                                                para.timeList.Add(time);
                                            }
                                        }

                                        //Получаем занятия и приписку с Преподавателем, корпусом и кабинетом; i - колонка группы

                                        para.lesson = Lesson.GetLesson(dt.Rows[k][i].ToString(), dt.Rows[k + 1][i].ToString());
                                        //para.lesson.name = dt.Rows[k][i].ToString();
                                        //para.lesson.description = dt.Rows[k + 1][i].ToString();

                                        //Получаем номер пары
                                        //Где то в конце расписания нет номера пары, но есть время, значит она восьмая
                                        para.Number = string.IsNullOrWhiteSpace(dt.Rows[k][2].ToString()) ? 8 : int.Parse(dt.Rows[k][2].ToString());
                                        
                                        if (para.Number == 8 && !group.paraList
                                                .Where(w => w.dayOfWeek == para.dayOfWeek).ToList()
                                                .Exists(e => e.Number == 7))
                                        {
                                            para.Number = 7;
                                        }

                                        if (group.paraList.Where(w => w.dayOfWeek == para.dayOfWeek).ToList().Exists(e => e.Number == para.Number))
                                            break;

                                        k++;
                                        group.paraList.Add(para);
                                    }

                                    indexDay++;
                                }
                            }
                            listGroups.Add(group);
                        }
                    }
                    groupList = listGroups;
                }
            }

            //var paraList = groupList.SelectMany(w => w.paraList);
            //var timeList = paraList.Select(w => w.timeList);

            //foreach (var item in groupList)
            //{
            //    Console.WriteLine($"{item.weekNumber} - {item.groupName}");

                
            //    string dayofWeek = string.Empty;
            //    int k = 0;
            //    foreach (var para in item.paraList)
            //    {
            //        if (!dayofWeek.Equals(para.dayOfWeek))
            //        {
            //            dayofWeek = para.dayOfWeek;
            //            Console.WriteLine(para.dayOfWeek);
            //        }

            //        string startTime = $"{para.timeList[0].timeStart} {para.timeList[0].timeEnd}";
            //        string endTime = $"{para.timeList[1].timeStart} {para.timeList[1].timeEnd}";

            //        Console.WriteLine($"{para.Number} - {startTime} - {endTime} - {para.lesson.name} - {para.lesson.description}");
            //    }

            //    Console.WriteLine();
            //}
            
            Console.ReadKey();

        }

        private  static (int f1, int f2,int f3,int f4) GetValueIndex(string tableName)
        {
            switch (tableName)
            {
                case "5 курс":
                    return (3, 7, 11, 15);
                case "магистратура":
                    return (3, 18, 22, 37);
                default:
                    return (3, 14, 18, 28);
            }
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            if (dt.Rows.Count == 0) return ""; // enter code here

            StringBuilder builder = new StringBuilder();
            builder.Append("<html>");
            builder.Append("<head>");
            builder.Append("<title>");
            builder.Append("Page-");
            builder.Append(Guid.NewGuid());
            builder.Append("</title>");
            builder.Append("</head>");
            builder.Append("<body>");
            builder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
            builder.Append("style='border: solid 1px Silver; font-size: x-small;'>");
            builder.Append("<tr align='left' valign='top'>");
            foreach (DataColumn c in dt.Columns)
            {
                builder.Append("<td align='left' valign='top'><b>");
                builder.Append(c.ColumnName);
                builder.Append("</b></td>");
            }
            builder.Append("</tr>");
            foreach (DataRow r in dt.Rows)
            {
                builder.Append("<tr align='left' valign='top'>");
                foreach (DataColumn c in dt.Columns)
                {
                    builder.Append("<td align='left' valign='top'>");
                    builder.Append(r[c.ColumnName]);
                    builder.Append("</td>");
                }
                builder.Append("</tr>");
            }
            builder.Append("</table>");
            builder.Append("</body>");
            builder.Append("</html>");

            return builder.ToString();
        }
    }
}
