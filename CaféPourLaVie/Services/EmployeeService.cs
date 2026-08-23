using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using CaféPourLaVie.Services.Common;
using CaféPourLaVie.Services.Interfaces;
using CaféPourLaVie.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }


        // Get all employees with their associated accounts
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Account)
                .ToListAsync();
        }

        // Get an employee by ID with their associated account
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Account)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }


        // Create Async method to add a new employee
        public async Task<ServiceResult> CreateAsync(EmployeeCreateViewModel model)
        {
            bool exists = await _context.Accounts
                .AnyAsync(a => a.Username == model.Username);

            if (exists)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Tên đăng nhập đã tồn tại."
                };
            }

            var account = new Account
            {
                Username = model.Username,
                Password = model.Password,
                Role = model.Role,
                Status = true
            };

            var employee = new Employee
            {
                EmployeeName = model.EmployeeName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                HireDate = model.HireDate,

                Account = account
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Thêm nhân viên thành công."
            };
        }


        // Update Async method to update an existing employee
        public async Task<ServiceResult> UpdateAsync(EmployeeEditViewModel model)
        {
            var employee = await _context.Employees
                .Include(e => e.Account)
                .FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);

            if (employee == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Không tìm thấy nhân viên."
                };
            }

            // Check if the username already exists for another account
            bool exists = await _context.Accounts
                .AnyAsync(a =>
                    a.Username == model.Username &&
                    a.AccountId != model.AccountId);

            if (exists)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Tên đăng nhập đã tồn tại."
                };
            }

            // Update the information
            employee.EmployeeName = model.EmployeeName;
            employee.Phone = model.Phone;
            employee.Account.Username = model.Username;
            employee.Account.Role = model.Role;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Cập nhật nhân viên thành công."
            };
        }
        // Get an employee edit view model by ID
        public async Task<EmployeeEditViewModel?> GetEditViewModelByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Account)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return null;

            return new EmployeeEditViewModel
            {
                EmployeeId = employee.EmployeeId,

                AccountId = employee.AccountId,

                EmployeeName = employee.EmployeeName,

                Phone = employee.Phone,

                Username = employee.Account.Username,

                Role = employee.Account.Role
            };
        }


        // ToggleStatus Async method to toggle the status of an employee's account
        public async Task<ServiceResult> ToggleStatusAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Account)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Không tìm thấy nhân viên."
                };
            }

            employee.Account.Status = !employee.Account.Status;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = employee.Account.Status
                    ? "Đã mở khóa tài khoản."
                    : "Đã khóa tài khoản."
            };
        }


        // ResetPassword Async method to reset an employee's password
        public async Task<ServiceResult> ResetPasswordAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Account)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Không tìm thấy nhân viên."
                };
            }

            employee.Account.Password = "123456";

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Đặt lại mật khẩu thành công."
            };
        }
    }
}