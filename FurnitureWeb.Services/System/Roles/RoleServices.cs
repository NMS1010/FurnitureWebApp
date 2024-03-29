﻿using Domain.EF;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.System.Roles
{
    public class RoleServices : IRoleServices
    {
        private readonly AppDbContext _context;

        public RoleServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(RoleCreateRequest request)
        {
            try
            {
                var role = new IdentityRole()
                {
                    Name = request.RoleName
                };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> Delete(string id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<PagedResult<RoleViewModel>> RetrieveAll(RoleGetPagingRequest request)
        {
            try
            {
                var query = await _context.Roles
                .ToListAsync();
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query
                        .Where(x => x.Name.Contains(request.Keyword))
                        .ToList();
                }
                var data = query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new RoleViewModel()
                    {
                        RoleId = x.Id,
                        RoleName = x.Name,
                    }).ToList();

                return new PagedResult<RoleViewModel>
                {
                    TotalItem = query.Count,
                    Items = data
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<RoleViewModel> RetrieveById(string id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                return new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> Update(RoleUpdateRequest request)
        {
            try
            {
                var role = await _context.Roles.FindAsync(request.RoleId);
                role.Name = request.RoleName;
                _context.Roles.Update(role);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
        }
    }
}