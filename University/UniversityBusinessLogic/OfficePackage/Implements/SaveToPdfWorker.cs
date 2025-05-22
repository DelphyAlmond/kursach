using UniversityBusinessLogic.OfficePackage.HelperEnums;
using UniversityBusinessLogic.OfficePackage.HelperModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace UniversityBusinessLogic.OfficePackage.Implements
{
    public class SaveToPdfWorker : AbstractSaveToPdfWorker
    {
        private Document? _document;

        private Section? _section;

        private Table? _table;

        private static ParagraphAlignment GetParagraphAlignment(PdfParagraphAlignmentType type)
        {
            return type switch
            {
                PdfParagraphAlignmentType.Center => ParagraphAlignment.Center,
                PdfParagraphAlignmentType.Left => ParagraphAlignment.Left,
                PdfParagraphAlignmentType.Right => ParagraphAlignment.Right,
                _ => ParagraphAlignment.Justify,
            };
        }

        /// <summary>
        /// Создание стилей для документа
        /// </summary>
        /// <param name="document"></param>
        private static void DefineStyles(Document document)
        {
            var style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;

            style = document.Styles.AddStyle("NormalTitle", "Normal");
            style.Font.Bold = true;
        }

        protected override void CreatePdf(PdfInfoWorker info)
        {
            _document = new Document();
            DefineStyles(_document);
            _document.DefaultPageSetup.Orientation = Orientation.Landscape;
            _section = _document.AddSection();
        }

        protected override void CreateParagraph(PdfParagraph pdfParagraph)
        {
            if (_section == null)
            {
                return;
            }
            var paragraph = _section.AddParagraph(pdfParagraph.Text);
            paragraph.Format.SpaceAfter = "1cm";
            paragraph.Format.Alignment = GetParagraphAlignment(pdfParagraph.ParagraphAlignment);
            paragraph.Style = pdfParagraph.Style;
        }

        protected override void CreateTable(List<string> columns)
        {
            if (_document == null)
            {
                return;
            }
            _table = _document.LastSection.AddTable();

            foreach (var elem in columns)
            {
                _table.AddColumn(elem);
            }
        }

        protected override void CreateRow(PdfRowParameters rowParameters)
        {
            if (_table == null)
            {
                return;
            }
            var row = _table.AddRow();
            for (int i = 0; i < rowParameters.Texts.Count; ++i)
            {
                row.Cells[i].AddParagraph(rowParameters.Texts[i]);

                if (!string.IsNullOrEmpty(rowParameters.Style))
                {
                    row.Cells[i].Style = rowParameters.Style;
                }

                Unit borderWidth = 0.5;

                row.Cells[i].Borders.Left.Width = borderWidth;
                row.Cells[i].Borders.Right.Width = borderWidth;
                row.Cells[i].Borders.Top.Width = borderWidth;
                row.Cells[i].Borders.Bottom.Width = borderWidth;

                row.Cells[i].Format.Alignment = GetParagraphAlignment(rowParameters.ParagraphAlignment);
                row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
            }
        }

        protected override void SavePdf(PdfInfoWorker info)
        {
            if (_document == null)
            {
                throw new InvalidOperationException("Document is not initialized.");
            }
            var renderer = new PdfDocumentRenderer(true)
            {
                Document = _document
            };
            try
            {
                renderer.RenderDocument();
                renderer.PdfDocument.Save(info.FileName);
            }
            catch (NullReferenceException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}