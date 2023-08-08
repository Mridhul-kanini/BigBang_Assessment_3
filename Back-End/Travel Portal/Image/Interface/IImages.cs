using Travel.Model;

namespace Travel.Interface
{
    public interface IImagesRepository
    {
        IEnumerable<Images> GetImages();
        Images GetImageById(int id);
        Images CreateImage(Images image);
        Images UpdateImage(int id, Images updatedImage);
        Images DeleteImage(int id);
    }
}
