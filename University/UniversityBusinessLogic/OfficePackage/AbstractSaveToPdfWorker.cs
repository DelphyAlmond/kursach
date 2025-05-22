using UniversityBusinessLogic.OfficePackage.HelperEnums;
using UniversityBusinessLogic.OfficePackage.HelperModels;

namespace UniversityBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToPdfWorker
    {
        public void CreateDoc(PdfInfoWorker info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph { Text = info.Title, Style = "NormalTitle", ParagraphAlignment = PdfParagraphAlignmentType.Center });

            CreateTable(new List<string> { "4cm", "5cm", "6cm", "5cm", "5 cm" });

            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Номер", "План обучения", "Дисциплина", "Форма оценивания" },
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });

            foreach (var item in info.PlanOfStudyAndStudent)
            {
                foreach (var studentName in item.StudentName)
                {
                    var rowTexts = new List<string> { item.Id.ToString(), item.PlanOfStudyName, studentName };
                    if (item.DisciplineName.Any())
                    {
                        foreach (var disciplineName in item.DisciplineName)
                        {
                            rowTexts.Add(disciplineName);
                            CreateRow(new PdfRowParameters
                            {
                                Texts = rowTexts,
                                Style = "Normal",
                                ParagraphAlignment = PdfParagraphAlignmentType.Left
                            });
                            rowTexts.RemoveAt(rowTexts.Count - 1);
                        }
                    }
                    else
                    {
                        // Если нет дисциплин, добавляем пустую строку
                        rowTexts.Add("");
                        CreateRow(new PdfRowParameters
                        {
                            Texts = rowTexts,
                            Style = "Normal",
                            ParagraphAlignment = PdfParagraphAlignmentType.Left
                        });
                    }
                }
            }
            SavePdf(info);
        }

        /// <summary>
		/// Создание doc-файла
		/// </summary>
		/// <param name="info"></param>
        protected abstract void CreatePdf(PdfInfoWorker info);

        /// <summary>
        /// Создание параграфа с текстом
        /// </summary>
        /// <param name="title"></param>
        /// <param name="style"></param>
        protected abstract void CreateParagraph(PdfParagraph paragraph);

        /// <summary>
        /// Создание таблицы
        /// </summary>
        /// <param name="title"></param>
        /// <param name="style"></param>
        protected abstract void CreateTable(List<string> columns);

        /// <summary>
        /// Создание и заполнение строки
        /// </summary>
        /// <param name="rowParameters"></param>
        protected abstract void CreateRow(PdfRowParameters rowParameters);

        /// <summary>
		/// Сохранение файла
		/// </summary>
		/// <param name="info"></param>
        protected abstract void SavePdf(PdfInfoWorker info);
    }
}
