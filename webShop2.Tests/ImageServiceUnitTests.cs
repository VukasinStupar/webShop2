   
using System.Timers;
using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

    public class ImageServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IImageRepository> _imageRepositoryMock;
        private readonly ImageService _service;

        public ImageServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _imageRepositoryMock = new Mock<IImageRepository>();

            _unitOfWorkMock
                .Setup(x => x.Images)
                .Returns(_imageRepositoryMock.Object);

            _service = new ImageService(_unitOfWorkMock.Object);
        }

    [Fact]
        public async Task GetByIdAsync_ReturnsImage_WhenImageExists()
        {
            Image image = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "image.jpg",
                IsMain = true
            };

            _imageRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(image);

            Image? result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(image, result);

            _imageRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenImageDoesNotExist()
        {
            _imageRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Image?)null);

            Image? result = await _service.GetByIdAsync(1);

            Assert.Null(result);

            _imageRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllImages()
        {
            List<Image> images =
            [
                new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "image1.jpg"
            },
            new Image
            {
                Id = 2,
                ProductId = 10,
                Url = "image2.jpg"
            }
            ];

            _imageRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(images);

            List<Image> result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(images, result);

            _imageRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByProductAsync_ReturnsImagesForProduct()
        {
            List<Image> images =
            [
                new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "image1.jpg"
            },
            new Image
            {
                Id = 2,
                ProductId = 10,
                Url = "image2.jpg"
            }
            ];

            _imageRepositoryMock
                .Setup(x => x.GetByProductAsync(10))
                .ReturnsAsync(images);

            List<Image> result =
                await _service.GetByProductAsync(10);

            Assert.Equal(2, result.Count);
            Assert.Equal(images, result);

            _imageRepositoryMock.Verify(
                x => x.GetByProductAsync(10),
                Times.Once);
        }

        [Fact]
        public async Task GetMainImageAsync_ReturnsMainImage_WhenImageExists()
        {
            Image image = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "main.jpg",
                IsMain = true
            };

            _imageRepositoryMock
                .Setup(x => x.GetMainImageAsync(10))
                .ReturnsAsync(image);

            Image? result =
                await _service.GetMainImageAsync(10);

            Assert.NotNull(result);
            Assert.Equal(image, result);

            _imageRepositoryMock.Verify(
                x => x.GetMainImageAsync(10),
                Times.Once);
        }

        [Fact]
        public async Task GetMainImageAsync_ReturnsNull_WhenMainImageDoesNotExist()
        {
            _imageRepositoryMock
                .Setup(x => x.GetMainImageAsync(10))
                .ReturnsAsync((Image?)null);

            Image? result =
                await _service.GetMainImageAsync(10);

            Assert.Null(result);

            _imageRepositoryMock.Verify(
                x => x.GetMainImageAsync(10),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_AddsImageAndSavesChanges()
        {
            Image image = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "image.jpg",
                IsMain = true
            };

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            Image result =
                await _service.CreateAsync(image);

            Assert.Equal(image, result);

            _imageRepositoryMock.Verify(
                x => x.AddAsync(image),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesImage_WhenImageExists()
        {
            Image existingImage = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "old.jpg",
                IsMain = false
            };

            Image imageUpdate = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "new.jpg",
                IsMain = true
            };

            _imageRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(existingImage);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            Image? result =
                await _service.UpdateAsync(imageUpdate);

            Assert.NotNull(result);
            Assert.Equal("new.jpg", result.Url);
            Assert.True(result.IsMain);

            _imageRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);

            _imageRepositoryMock.Verify(
                x => x.Update(existingImage),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenImageDoesNotExist()
        {
            Image imageUpdate = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "new.jpg",
                IsMain = true
            };

            _imageRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Image?)null);

            Image? result =
                await _service.UpdateAsync(imageUpdate);

            Assert.Null(result);

            _imageRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);

            _imageRepositoryMock.Verify(
                x => x.Update(It.IsAny<Image>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrueAndDeletesImage_WhenImageExists()
        {
            Image image = new Image
            {
                Id = 1,
                ProductId = 10,
                Url = "image.jpg"
            };

            _imageRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(image);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            bool result =
                await _service.DeleteAsync(1);

            Assert.True(result);

            _imageRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);

            _imageRepositoryMock.Verify(
                x => x.Delete(image),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenImageDoesNotExist()
        {
            _imageRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Image?)null);

            bool result =
                await _service.DeleteAsync(1);

            Assert.False(result);

            _imageRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);

            _imageRepositoryMock.Verify(
                x => x.Delete(It.IsAny<Image>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }
    }
