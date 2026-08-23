using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using CaféPourLaVie.Models.Enums;
using CaféPourLaVie.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Services
{
    public class ImportService : IImportService
    {
        private readonly ApplicationDbContext _context;

        public ImportService(ApplicationDbContext context)
        {
            _context = context;
        }


        // =========================
        // GET ALL
        // =========================
        public async Task<List<ImportReceipt>> GetAllAsync()
        {
            return await _context.ImportReceipts
                .Include(i => i.Account)
                .OrderByDescending(i => i.ImportDate)
                .ToListAsync();
        }


        // =========================
        // GET BY ID
        // =========================
        public async Task<ImportReceipt?> GetByIdAsync(int id)
        {
            return await _context.ImportReceipts

                .Include(i => i.Account)

                .Include(i => i.ImportDetails)
                    .ThenInclude(d => d.Product)

                .FirstOrDefaultAsync(i =>
                    i.ImportReceiptId == id);
        }


        // =========================
        // CREATE
        // =========================
        public async Task<int> CreateAsync(
            int accountId,
            List<ImportDetail> details)
        {
            if (details == null || details.Count == 0)
            {
                throw new Exception("Phiếu nhập phải có ít nhất một sản phẩm.");
            }


            var receipt = new ImportReceipt
            {
                ImportDate = DateTime.Now,

                AccountId = accountId,

                Status = ImportStatus.Pending,

                ImportDetails = new List<ImportDetail>()
            };


            decimal totalAmount = 0;


            foreach (var detail in details)
            {
                if (detail.Quantity <= 0)
                {
                    throw new Exception("Số lượng nhập phải lớn hơn 0.");
                }


                if (detail.ImportPrice < 0)
                {
                    throw new Exception("Giá nhập không hợp lệ.");
                }


                var product = await _context.Products
                    .FirstOrDefaultAsync(p =>p.ProductId == detail.ProductId);


                if (product == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm.");
                }


                detail.SubTotal = detail.Quantity * detail.ImportPrice;

                totalAmount += detail.SubTotal;

                receipt.ImportDetails.Add(detail);
            }


            receipt.TotalAmount = totalAmount;


            _context.ImportReceipts.Add(receipt);


            await _context.SaveChangesAsync();


            return receipt.ImportReceiptId;
        }


        // =========================
        // APPROVE
        // =========================
        public async Task ApproveAsync(int id)
        {
            var receipt = await _context.ImportReceipts

                .Include(i => i.ImportDetails)

                .FirstOrDefaultAsync(i => i.ImportReceiptId == id);


            if (receipt == null)
            {
                throw new Exception("Không tìm thấy phiếu nhập.");
            }


            if (receipt.Status != ImportStatus.Pending)
            {
                throw new Exception("Phiếu nhập này đã được xử lý.");
            }


            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var detail in receipt.ImportDetails)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(
                            p => p.ProductId == detail.ProductId);

                    if (product == null)
                    {
                        throw new Exception(
                            $"Không tìm thấy sản phẩm {detail.ProductId}.");
                    }

                    // Plus the imported quantity to the product's stock
                    product.Quantity += detail.Quantity;

                    // Add an inventory transaction record for this imports
                    _context.InventoryTransactions.Add(
                        new InventoryTransaction
                        {
                            ProductId = product.ProductId,
                            TransactionDate = DateTime.Now,
                            Quantity = detail.Quantity,
                            Type = InventoryTransactionType.Import,
                            Note = $"Nhập hàng - Phiếu #{receipt.ImportReceiptId}"
                        });
                }


                receipt.Status = ImportStatus.Approved;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }


        // =========================
        // REJECT
        // =========================
        public async Task RejectAsync(int id)
        {
            var receipt = await _context.ImportReceipts
                .FirstOrDefaultAsync(i =>
                    i.ImportReceiptId == id);


            if (receipt == null)
            {
                throw new Exception("Không tìm thấy phiếu nhập.");
            }


            if (receipt.Status != ImportStatus.Pending)
            {
                throw new Exception("Phiếu nhập này đã được xử lý.");
            }


            receipt.Status = ImportStatus.Rejected;

            await _context.SaveChangesAsync();
        }
    }
}