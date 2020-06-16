using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.Entities.Models;

namespace HelpDesk.Entities.Contracts
{
    public interface ICompanyRepository : IRepositoryBase<CompanyModel>
    {
        Task<IEnumerable<CompanyModel>> GetAllCompanies();
        Task<CompanyModel> GetCompanyById(Guid id);
        void CreateCompany(CompanyModel company);
        void UpdateCompany(CompanyModel company);
        void DeleteCompany(CompanyModel company);
    }
}
