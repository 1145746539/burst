using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OEE.bean;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OEE.Util
{
    class ExcelUtil
    {

        private string excelPath = @"D:\equipment\Data";
        private ExcelUtil() { }
        private static ExcelUtil excelUtil = null;
        public static ExcelUtil Instance()
        {
            if (excelUtil == null)
            {
                excelUtil = new ExcelUtil();
            }
            return excelUtil;
        }

        public bool SaveData(FinalResult final)
        {
            try
            {
                DateTime dt = DateTime.Now;
                IWorkbook workbook = null;
                string file = excelPath + "\\" + dt.ToString("yyyy-MM-dd") + ".xls";
                if (!Directory.Exists(excelPath))
                    Directory.CreateDirectory(excelPath);  //使用Directory类的CreateDirectory方法创建文件夹
                if (!File.Exists(file))
                    File.Copy(Application.StartupPath + "\\" + "DATA.xls", file);
                string fileExt = Path.GetExtension(file).ToLower();

                using (FileStream fs = File.Open(file, FileMode.OpenOrCreate,FileAccess.ReadWrite))
                {
                    if (fileExt == ".xlsx")
                    { workbook = new XSSFWorkbook(fs); }
                    else
                    { workbook = new HSSFWorkbook(fs); } //fileExt == ".xls"

                    ISheet sheet = workbook.GetSheet("Sheet1");
                    IRow row0 = sheet.GetRow(0);    //第一行
                    int cellcount = row0.LastCellNum;   //列数
                    int rowcount = sheet.LastRowNum;    //行数 不包含标题行

                    IRow row = sheet.CreateRow(rowcount+1);
                    
                    ICell cell0 = row.CreateCell(0);
                    ICell cell1 = row.CreateCell(1);
                    ICell cell2 = row.CreateCell(2);
                    ICell cell3 = row.CreateCell(3);
                    ICell cell4 = row.CreateCell(4);
                    ICell cell5 = row.CreateCell(5);
                    ICell cell6 = row.CreateCell(6);
                    ICell cell7 = row.CreateCell(7);

                    cell0.SetCellValue(final.CodeIndex);
                    cell1.SetCellValue(final.TestMode);
                    cell2.SetCellValue(final.TestStatus);
                    cell3.SetCellValue(final.Alarm);
                    cell4.SetCellValue(final.PressureValue+final.PressureUnit);
                    cell5.SetCellValue(final.LeakValue + final.LeakUnit);
                    cell6.SetCellValue(final.Result);
                    cell7.SetCellValue(final.Product);
                }


                using (FileStream fs = File.OpenWrite(file))
                {
                    workbook.Write(fs);//向打开的这个xls文件中写入数据

                }



            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// 复制Excel文件
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        //public bool CopyExcel(string newPath)
        //{
        //    try
        //    {
        //        FileInfo info = new FileInfo(path);
        //        info.CopyTo(newPath);
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(  ex.Message );
        //        return false;
        //    }

        //    return true;
        //}

        /// <summary>
        /// DataTable导出成Excel(去除数据库ID列)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public void TableToExcel(DataTable dt, string file)
        {
            IWorkbook workbook;
            //if (!File.Exists(file))
            //{
            //    File.Create(file).Dispose();
            //}

            string fileExt = Path.GetExtension(file).ToLower();
            if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
            if (workbook == null) { return; }
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);

            //表头  
            IRow row = sheet.CreateRow(0);
            //特殊定制 dt.Columns.Count-1 dt.Columns[i+1]
            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i + 1].ColumnName);
            }

            //数据  
            //特殊定制 dt.Columns.Count-1 dt.Columns[i+1]
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i + 1][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }

        }



        /* Microsoft.Office.Interop.*/
        Excel.Application xApp;
        /*  Microsoft.Office.Interop.*/
        Excel.Workbook xBook;
        /*   Microsoft.Office.Interop.*/
        Excel.Worksheet xSheet;
        Excel.Worksheet ySheet;
        // Excel.Worksheet zSheet;




    }

}
