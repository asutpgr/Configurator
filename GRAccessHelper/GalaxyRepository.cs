using System;
using System.Collections.Generic;
using System.Linq;

namespace GRAccessHelper
{
    using Exceptions;
    using ArchestrA.GRAccess;
    public static class GalaxyRepository
    {
        private static GRAccessApp _grAccessApp = new GRAccessApp();

        // Получение списка названий галактик на GRNode
        public static List<string> GetGalaxiesNames(string nodeName = null)
        {
            List<string> result = new List<string>();
            IGalaxies gRepos = null;
            if (string.IsNullOrWhiteSpace(nodeName))
                gRepos = _grAccessApp.QueryGalaxies("localhost");
            else
                gRepos = _grAccessApp.QueryGalaxies(nodeName);
            if (!_grAccessApp.CommandResult.Successful)
                throw new GalaxyExceptions(_grAccessApp.CommandResult);
            if (gRepos != null)
                foreach (IGalaxy item in gRepos)
                   result.Add(item.Name);
            return result;
        }
        // Получание спика галактик нa GRNode
        public static IGalaxies GetGalaxies(string nodeName = null)
        {
            _grAccessApp = new GRAccessApp();
            return _grAccessApp.QueryGalaxies(nodeName) ?? throw new GalaxyExceptions($"Отстувуют Galaxy на {nodeName}", _grAccessApp.CommandResult);
        }
        // Создание Galaxy
        public static void CreateGalaxy(string galaxyName, string GRNodeName = null, bool enableSecurity = true,
                                        EAuthenticationMode AuthenticationMode = EAuthenticationMode.osAuthenticationMode, string osUserName = null)
        {
            _grAccessApp.CreateGalaxy(galaxyName, GRNodeName, enableSecurity, AuthenticationMode, osUserName);
            ICommandResult commandResult = _grAccessApp.CommandResult;
            if (!commandResult.Successful)
                throw new GalaxyExceptions(string.Format($"Не удалось создать галактику {galaxyName}! Причина {commandResult.Text}-{commandResult.CustomMessage}"));
        }
        //Созданеи Galaxy
        public static IGalaxy CreateAndGetGalaxy(string galaxyName, string GRNodeName = null, bool enableSecurity = true,
                                          EAuthenticationMode AuthenticationMode = EAuthenticationMode.osAuthenticationMode, string osUserName = null)
        {
            _grAccessApp = new GRAccessApp();
            _grAccessApp.CreateGalaxy(galaxyName, GRNodeName, enableSecurity, AuthenticationMode, osUserName);
            ICommandResult commandResult = _grAccessApp.CommandResult;
            if (!commandResult.Successful)
                throw new Exception(string.Format($"Не удалось создать галактику {galaxyName}! Причина {commandResult.Text}-{commandResult.CustomMessage}"));
            return GetGalaxy(galaxyName, GRNodeName);
        }
        
        // Создание Galaxy из шаблона
        public static void CreateGalaxyFromTemplate(string galaxyTemplateName, string galaxyName, string nodeName = null)
        {
            _grAccessApp = new GRAccessApp();
            _grAccessApp.CreateGalaxyFromTemplate(galaxyTemplateName, galaxyName, nodeName);
            ICommandResult commandResult = _grAccessApp.CommandResult;
            if (!commandResult.Successful)
                throw new Exception(string.Format($"Не удалось создать галактику {galaxyName}! Причина {commandResult.Text}-{commandResult.CustomMessage}"));
        }
        // Создание Galaxy из шаблона
        public static IGalaxy CreateGalaxyAndGetFromTemplate(string galaxyTemplateName, string galaxyName, string nodeName = null)
        {
            _grAccessApp = new GRAccessApp();
            _grAccessApp.CreateGalaxyFromTemplate(galaxyTemplateName, galaxyName, nodeName);
            ICommandResult commandResult = _grAccessApp.CommandResult;
            if (!commandResult.Successful)
                throw new Exception(string.Format($"Не удалось создать галактику {galaxyName}! Причина {commandResult.Text}-{commandResult.CustomMessage}"));
            return GetGalaxy(galaxyName, nodeName);
        }
        // Получение списка шаблонов Galaxies
        public static List<string> GetListTemplateGalaxies(string nodeName = null)
        {
            return _grAccessApp.ListCreateGalaxyTemplates(nodeName).ToList();
        }
        //получение Galaxy по имени
        public static IGalaxy GetGalaxy(string galaxyName, string nodeName)
        {
            if (string.IsNullOrEmpty(galaxyName))  throw new ArgumentException(nameof(galaxyName));
            _grAccessApp = null;
            _grAccessApp = new GRAccessApp();
            var galaxies = _grAccessApp.QueryGalaxies(nodeName);
            GalaxyExceptions.ThrowIfNoSuccess(_grAccessApp.CommandResult);
            IGalaxy galaxy = galaxies[galaxyName];
            GalaxyExceptions.ThrowIfNoSuccess(_grAccessApp.CommandResult);
            return galaxy == null ? throw new GalaxyObjectNotFoundException(galaxyName) : galaxy;
        }
    }
}
