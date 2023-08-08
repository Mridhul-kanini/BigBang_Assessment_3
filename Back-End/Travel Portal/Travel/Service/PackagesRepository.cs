using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class PackagesRepository : IPackagesRepository
    {
        private readonly TravelContext _context;

        public PackagesRepository(TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<Packages> GetPackages()
        {
            return _context.Packages.ToList();
        }

        public Packages GetPackageById(int id)
        {
            return _context.Packages.FirstOrDefault(x => x.PackageId == id);
        }

        public IEnumerable<Packages> GetPackagesByTravelId(int travelId) 
        {
            return _context.Packages.Where(p => p.TravelId == travelId).ToList();
        }

        public Packages CreatePackage(Packages package)
        {
            _context.Add(package);
            _context.SaveChanges();
            return package;
        }

        public Packages UpdatePackage(int id, Packages updatedPackage)
        {
            var package = _context.Packages.Find(id);
            if (package != null)
            {
                package.Name = updatedPackage.Name;
                package.Description = updatedPackage.Description;
                package.Location = updatedPackage.Location;
                package.ImageUrl = updatedPackage.ImageUrl;
                package.Price = updatedPackage.Price;

                _context.Entry(package).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return package;
        }

        public Packages DeletePackage(int id)
        {
            var package = _context.Packages.Find(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                _context.SaveChanges();
            }
            return package;
        }
    }
}
