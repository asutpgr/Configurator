using System;
using GRAccessHelper;
using GRAccessHelper.Exceptions.Galaxy;
namespace Galaxy.Models.Model
{
    public class Galaxy
    {
        private GalaxyUtils galaxy;
        private string GalaxyName;
        private string GalaxyServer;
        private string GalaxyUserName;
        private string GalaxyPass;

       
        public GalaxyUtils CurrentGalaxy 
        {
            get
            {
                return galaxy ?? throw new GalaxyNullReferenceException();
            }
            set
            {
                if (value == null) throw new GalaxyNullReferenceException();
                galaxy = value;
            }
        }

        
        public Galaxy(string gname,string gUserName, string gPass)
        {
            GalaxyName = gname;
            GalaxyUserName = gUserName;
            GalaxyPass = gPass;

        }
                



    }
}
