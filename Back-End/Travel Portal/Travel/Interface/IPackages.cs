using System.Collections.Generic;
using Travel.Model;

namespace Travel.Interface
{
    public interface IPackagesRepository
    {
        IEnumerable<Packages> GetPackages();
        Packages GetPackageById(int id);
        IEnumerable<Packages> GetPackagesByTravelId(int travelId);
        Packages CreatePackage(Packages package);
        Packages UpdatePackage(int id, Packages updatedPackage);
        Packages DeletePackage(int id);
    }
}
