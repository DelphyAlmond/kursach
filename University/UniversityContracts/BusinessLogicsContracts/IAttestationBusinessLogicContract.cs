using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityContracts.BindingModels;
using UniversityContracts.SearchModels;
using UniversityContracts.ViewModels;

namespace UniversityContracts.BusinessLogicsContracts
{
    public interface IAttestationBusinessLogicContract
    {
        List<AttestationViewModel>? ReadList(AttestationSearchModel? model);
        AttestationViewModel? ReadElement(AttestationSearchModel model);
        bool CreateAttestation(AttestationBindingModel model);
        bool DeleteAttestation(AttestationBindingModel model);
        bool UpdateAttestation(AttestationBindingModel model);

    }
}
