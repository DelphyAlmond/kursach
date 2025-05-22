using UniversityBusinessLogic.OfficePackage.HelperEnums;
using UniversityBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToExcelStorekeeper
    {
        public void CreateReport(ExcelInfoStorekeeper info)
        {
            CreateExcel(info);
            InsertCellInWorksheet(new ExcelCellParameters
            {
                ColumnName = "A",
                RowIndex = 1,
                Text = info.Title,
                StyleInfo = ExcelStyleInfoType.Title
            });
            MergeCells(new ExcelMergeParameters
            {
                CellFromName = "A1",
                CellToName = "C1"
            });
            uint rowIndex = 2;
            foreach (var t in info.Teachers)
            {
                InsertCellInWorksheet(new ExcelCellParameters
                {
                    ColumnName = "A",
                    RowIndex = rowIndex,
                    Text = t.TeacherName,
                    StyleInfo = ExcelStyleInfoType.Text
                }); ;
                rowIndex++;
                foreach (var st in t.Students)
                {
                    InsertCellInWorksheet(new ExcelCellParameters
                    {
                        ColumnName = "B",
                        RowIndex = rowIndex,
                        Text = st,
                        StyleInfo =
                    ExcelStyleInfoType.TextWithBroder
                    });
                    rowIndex++;
                }
                rowIndex++;
            }
            SaveExcel(info);
        }
        protected abstract void CreateExcel(ExcelInfoStorekeeper info);
        protected abstract void InsertCellInWorksheet(ExcelCellParameters excelParams);
        protected abstract void MergeCells(ExcelMergeParameters excelParams);
        protected abstract void SaveExcel(ExcelInfoStorekeeper info);
    }
}
