using Itinero;
using Itinero.IO.Osm;
using Itinero.Profiles;
using System.IO;
using System.Reflection;

namespace RouterDbCreator
{
    public class DBCreator
    {
        private static string ProfileFileName = "customcar.lua";
        public static void Build(Options opts)
        {
            string executingPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            string profileFile = Path.Combine(executingPath, ProfileFileName);
            var vehicle = DynamicVehicle.LoadFromStream(File.OpenRead(profileFile));

            var routerDb = new RouterDb();
            using (var stream = new FileInfo(opts.OsmFilePath).OpenRead())
            {
                // read data which is relevant for car navigation only
                routerDb.LoadOsmData(stream, vehicle);
            }

            routerDb.AddContracted(Itinero.Osm.Vehicles.Vehicle.Car.Shortest());
            routerDb.AddContracted(Itinero.Osm.Vehicles.Vehicle.Car.Fastest());


            // write the routerdb to disk.
            Directory.CreateDirectory(opts.Outputdirectory);
            using (var stream = new FileInfo(Path.Combine(opts.Outputdirectory, "file.routerdb")).Open(FileMode.Create))
            {
                routerDb.Serialize(stream);
            }

        }
    }
}
