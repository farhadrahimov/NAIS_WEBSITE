namespace NAIS_Website.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string uploadFolderPath, string product = null);
        void DeleteImage(string imagePath);
    }

    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string category, string product = null)
        {
            if (imageFile == null || imageFile.Length == 0)
                return string.Empty;

            // Определяем базовую папку для категорий
            string baseFolderPath = Path.Combine("files", "photos", "catalog-categories", category);

            // Если передан продукт, добавляем его как подкаталог
            if (!string.IsNullOrEmpty(product))
            {
                baseFolderPath = Path.Combine(baseFolderPath, product);
            }

            // Полный путь на сервере
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, baseFolderPath);
            Directory.CreateDirectory(uploadPath);  // Создаем директорию, если её нет

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Возвращаем относительный путь для записи в базу данных
            return Path.Combine(baseFolderPath, fileName);
        }

        public void DeleteImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }
    }

}
