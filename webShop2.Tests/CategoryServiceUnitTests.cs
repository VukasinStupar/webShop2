using Moq;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsCategory_WhenCategoryExists()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        Category category = new Category
        {
            Id = 1,
            Name = "Elektronika"
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(category);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        Category? result =
            await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Elektronika", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(99))
            .ReturnsAsync((Category?)null);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        Category? result =
            await service.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        List<Category> categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Elektronika"
            },
            new Category
            {
                Id = 2,
                Name = "Odeca"
            },
            new Category
            {
                Id = 3,
                Name = "Obuca"
            }
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(categories);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        List<Category> result =
            await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("Elektronika", result[0].Name);
        Assert.Equal("Odeca", result[1].Name);
        Assert.Equal("Obuca", result[2].Name);
    }

    [Fact]
    public async Task GetRootCategoriesAsync_ReturnsRootCategories()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        List<Category> categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Elektronika",
                ParentId = null
            },
            new Category
            {
                Id = 2,
                Name = "Odeca",
                ParentId = null
            }
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetRootCategoriesAsync())
            .ReturnsAsync(categories);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        List<Category> result =
            await service.GetRootCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Null(result[0].ParentId);
        Assert.Null(result[1].ParentId);
    }

    [Fact]
    public async Task GetChildrenAsync_ReturnsChildrenOfCategory()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        List<Category> children = new List<Category>
        {
            new Category
            {
                Id = 2,
                Name = "Telefoni",
                ParentId = 1
            },
            new Category
            {
                Id = 3,
                Name = "Laptopovi",
                ParentId = 1
            }
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetChildrenAsync(1))
            .ReturnsAsync(children);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        List<Category> result =
            await service.GetChildrenAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].ParentId);
        Assert.Equal(1, result[1].ParentId);
    }

    [Fact]
    public async Task CreateAsync_AddsCategoryAndSavesChanges()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        Category category = new Category
        {
            Name = "Elektronika"
        };

        categoryRepositoryMock
            .Setup(repository => repository.AddAsync(category))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        Category result =
            await service.CreateAsync(category);

        // Assert
        Assert.NotNull(result);
        Assert.Same(category, result);
        Assert.Equal("Elektronika", result.Name);

        categoryRepositoryMock.Verify(
            repository => repository.AddAsync(category),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesCategory_WhenCategoryExists()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        Category existingCategory = new Category
        {
            Id = 1,
            Name = "Elektronika",
            ParentId = null
        };

        Category updatedCategory = new Category
        {
            Name = "Mobilna elektronika",
            ParentId = 5
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(existingCategory);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        Category? result =
            await service.UpdateAsync(1, updatedCategory);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Mobilna elektronika", result.Name);
        Assert.Equal(5, result.ParentId);

        categoryRepositoryMock.Verify(
            repository => repository.Update(existingCategory),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        Category updatedCategory = new Category
        {
            Name = "Elektronika"
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(99))
            .ReturnsAsync((Category?)null);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        Category? result =
            await service.UpdateAsync(99, updatedCategory);

        // Assert
        Assert.Null(result);

        categoryRepositoryMock.Verify(
            repository => repository.Update(It.IsAny<Category>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesCategory_WhenCategoryExists()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        Category category = new Category
        {
            Id = 1,
            Name = "Elektronika"
        };

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(category);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        bool result =
            await service.DeleteAsync(1);

        // Assert
        Assert.True(result);

        categoryRepositoryMock.Verify(
            repository => repository.Delete(category),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenCategoryDoesNotExist()
    {
        // Arrange
        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        Mock<IUnitOfWork> unitOfWorkMock =
            new Mock<IUnitOfWork>();

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(99))
            .ReturnsAsync((Category?)null);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Categories)
            .Returns(categoryRepositoryMock.Object);

        CategoryService service =
            new CategoryService(unitOfWorkMock.Object);

        // Act
        bool result =
            await service.DeleteAsync(99);

        // Assert
        Assert.False(result);

        categoryRepositoryMock.Verify(
            repository => repository.Delete(It.IsAny<Category>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(),
            Times.Never);
    }
}