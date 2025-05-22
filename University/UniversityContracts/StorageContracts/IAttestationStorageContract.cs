using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.StorageContracts
{
    public interface IAttestationStorage
    {
        List<AttestationViewModel> GetFullList();
        List<AttestationViewModel> GetFilteredList(AttestationSearchModel model);
        AttestationViewModel? GetElement(AttestationSearchModel model);
        AttestationViewModel? Insert(AttestationBindingModel model);
        AttestationViewModel? Update(AttestationBindingModel model);
        AttestationViewModel? Delete(AttestationBindingModel model);
    }
}
