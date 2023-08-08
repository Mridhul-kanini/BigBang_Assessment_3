using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly ImageContext _context;

        public ImagesRepository(ImageContext context)
        {
            _context = context;
        }

        public IEnumerable<Images> GetImages()
        {
            return _context.Images.ToList();
        }

        public Images GetImageById(int id)
        {
            return _context.Images.FirstOrDefault(x => x.ImgId == id);
        }

        public Images CreateImage(Images image)
        {
            _context.Images.Add(image);
            _context.SaveChanges();
            return image;
        }

        public Images UpdateImage(int id, Images updatedImage)
        {
            var image = _context.Images.Find(id);
            if (image != null)
            {
                image.Name = updatedImage.Name;
                image.Location = updatedImage.Location;
                image.ImageUrl = updatedImage.ImageUrl;

                _context.Entry(image).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return image;
        }

        public Images DeleteImage(int id)
        {
            var image = _context.Images.Find(id);
            if (image != null)
            {
                _context.Images.Remove(image);
                _context.SaveChanges();
            }
            return image;
        }
    }
}