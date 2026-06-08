using FinTrack.Data;
using FinTrack.Models.DTOs.CategoryDtos;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;
        private readonly IAuditService _auditService;
        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _auditService = auditService;
        }

        public async Task CreateCategory(string userId,string userName,CategoryDto category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(category));
            }
            Category categoryEntity = new Category
            {
                Name = category.Name,
                ApplicationUserId = userId,
                IsSystemDefined = false
            };
            await _unitOfWork.Category.CreateAsync(categoryEntity);
            await _unitOfWork.Save();
            _logger.LogInformation("Category with cateogry ID : {Id} created successfully for user {UserId}.", categoryEntity.Id, userId);
            AuditData auditData = new AuditData
            {
                UserName = userName,
                Action = "Create",
                EntityActedUpon = "Category",
                EntityId = categoryEntity.Id,
                Timestamp = DateTime.UtcNow
            };
            await _auditService.LogAuditDataAsync(auditData);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Category.FindAllAsync(null);
            return categories;
        }

        public async Task DeleteCategory(string userName, int id)
        {
            
            var category = await _unitOfWork.Category.FindAsync(id, null);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            await _unitOfWork.Category.Delete(category);
            await _unitOfWork.Save();
            _logger.LogInformation("Category with cateogry ID : {Id} deleted successfully.", id);
            AuditData auditData = new AuditData
            {
                UserName = userName,
                Action = "Delete",
                EntityActedUpon = "Category",
                EntityId = id,
                Timestamp = DateTime.UtcNow
            };
            await _auditService.LogAuditDataAsync(auditData);
        }
    }

}
