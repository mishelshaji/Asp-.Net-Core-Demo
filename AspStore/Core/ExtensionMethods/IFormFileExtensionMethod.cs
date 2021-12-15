namespace AspStore.Core.ExtensionMethods
{
    public static class IFormFileExtensionMethod
    {
        public static async Task<string> SaveFileToDiskAsync(this IFormFile file, string basePath)
        {
            var uniqueFileName = Guid.NewGuid().ToString();
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!(fileExtension.Equals(".jpg") || fileExtension.Equals(".png")))
            {
                throw new ApplicationException("Invalid File Type");
            }

            string uploadPath = Path.Combine(basePath, uniqueFileName + fileExtension);
            try
            {
                Directory.CreateDirectory(basePath);
                //var stream = new FileStream(uploadPath, FileMode.Create);
                //await file.CopyToAsync(stream);
                //stream.Dispose();
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return uniqueFileName + fileExtension;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
