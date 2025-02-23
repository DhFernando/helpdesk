﻿using HelpDesk.Entities.Contracts;
using HelpDesk.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Entities.Repository
{
    public class TicketRepository : RepositoryBase<TicketModel>, ITicketRepository
    {
        public TicketRepository(HelpDeskContext helpDeskContext) : base(helpDeskContext) { }
        public void CreateTicket(TicketModel ticket)
        {
            ticket.TicketId = Guid.NewGuid().ToString();
            Create(ticket);
        }

        public void DeleteTicket(TicketModel ticket)
        {
            Delete(ticket);
        }

        public async Task<IEnumerable<TicketModel>> GetAllTicket()
        {
            return await FindAll().OrderBy(tkt => tkt.TktCreatedDate).ToListAsync();
        }

        public async Task<FileStream> GetAttachment(Guid ticketId)
        {
            var ticket = await GetTicketById(ticketId);
            var fileName = ticket.TktAttachment;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "AttachmentStorage", fileName);
            var stream = new FileStream(path, FileMode.Open);
            return stream;
        }

        public async Task<IEnumerable<TicketModel>> GetTicketByCondition(Guid id, string userRole, string userName)
        {
            if (userRole == "Manager")
            {
                return await FindByCondition(tkt => tkt.CompanyId.Equals(id.ToString())).OrderByDescending(tkt => tkt.TktCreatedDate).ToListAsync();
            }
            else if (userRole == "User")
            {
                return await FindByCondition(tkt => tkt.CompanyId == id.ToString() && tkt.TktCreatedBy == userName).OrderByDescending(tkt => tkt.TktCreatedDate).ToListAsync();
            }

            return null;

        }

        public async Task<TicketModel> GetTicketById(Guid id, Boolean noTracking = false)
        {
            if (noTracking)
            {
                return await FindByCondition(tkt => tkt.TicketId.Equals(id.ToString())).AsNoTracking().FirstOrDefaultAsync();
            }
            return await FindByCondition(tkt => tkt.TicketId.Equals(id.ToString())).FirstOrDefaultAsync();
        }

        public async Task<string> GetTicketCodesByCondition(string id)
        {
            return await FindByCondition(tkt => tkt.CompanyId.Equals(id)).OrderByDescending(s => s.TicketCode).Select(tkt => tkt.TicketCode).FirstOrDefaultAsync();
        }

        public void UpdateTicket(TicketModel ticket)
        {
            Update(ticket);
        }

        public async Task<string> UploadAttachment(IFormFile attachment, string ticketId)
        {
            try
            {
                string fileExtension = attachment.FileName.Split('.', StringSplitOptions.None).Last();
                string fileName = ticketId + "." + fileExtension;

                var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "AttachmentStorage");
                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
                }

                var path = Path.Combine(storagePath, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }
                return fileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TicketModel>> GetTicketsByBrand(CompanyBrandModel brand)
        {
            return await FindByCondition(tkt => tkt.CompanyId.Equals(brand.CompanyId)).Where(cpTkt => cpTkt.BrandId.Equals(brand.BrandId)).ToListAsync();
        }

        public async Task<IEnumerable<TicketModel>> GetTicketsByCompany(CompanyModel company)
        {
            return await FindByCondition(tkt => tkt.CompanyId.Equals(company.CompanyId)).ToListAsync();
        }

        public async Task<IEnumerable<TicketModel>> GetTicketsByCategory(CategoryModel category)
        {
            return await FindByCondition(tkt => tkt.CompanyId.Equals(category.CompanyId)).Where(cpTkt => cpTkt.CategoryId.Equals(category.CategoryId)).ToListAsync();
        }

        public async Task<IEnumerable<TicketModel>> GetTicketsByProduct(ProductModel product)
        {
            return await FindByCondition(tkt => tkt.CompanyId.Equals(product.CompanyId)).Where(cpTkt => cpTkt.ProductId.Equals(product.ProductId)).ToListAsync();
        }

        public async Task<IEnumerable<TicketModel>> GetTicketsByModule(ModuleModel module)
        {
            return await FindByCondition(tkt => tkt.CompanyId.Equals(module.CompanyId)).Where(cpTkt => cpTkt.ModuleId.Equals(module.ModuleId)).ToListAsync();
        }


    }
}
