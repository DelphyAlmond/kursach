using UniversityBusinessLogic.OfficePackage.HelperEnums;
using UniversityBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace UniversityBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToWordWorker
    {
        public void CreateDoc(WordInfoWorker info)
        {
            CreateWord(info);
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> { (info.Title, new WordTextProperties 
                { Bold = true, Size = "24", }) },
                TextProperties = new WordTextProperties
                {
                    Size = "24",
                    JustificationType = WordJustificationType.Center
                }
            });
            foreach (var planOfStudys in info.PlanOfStudys)
            {
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> {
                    (planOfStudys.PlanOfStudyName + " :", new WordTextProperties { Size = "24", Bold = true, }), 
                    },
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationType = WordJustificationType.Both
                    }
                });
                foreach(var discipline in planOfStudys.Disciplines)
                {
                    CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)> {
                        (planOfStudys.FormOfStudy + " : ", new WordTextProperties { Size = "24", }),
                        (discipline, new WordTextProperties { Size = "24", }),
                    },
                        TextProperties = new WordTextProperties
                        {
                            Size = "24",
                            JustificationType = WordJustificationType.Both
                        }
                    });
                }
            }
            SaveWord(info);
        }
        protected abstract void CreateWord(WordInfoWorker info);
        protected abstract void CreateParagraph(WordParagraph paragraph);
        protected abstract void SaveWord(WordInfoWorker info);

    }
}
