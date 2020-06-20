using System;

namespace GRAccessHelper
{
    using ArchestrA.GRAccess;
    using Exceptions.Galaxy;
    using Exceptions.IAttribute;
    using Extensions;

    public class Galaxy
    {
        #region Поле
        //Поле
        private IGalaxy _galaxy;
        #endregion

        #region Свойства
        //Результат команды
        public ICommandResult CommandResult
        {
            get { return _galaxy.CommandResult; }
        }
        #endregion

        #region Конструкторы
        //Создание Galaxy
        public Galaxy(string galaxyName, bool createIfNotExist = false, string hostname = null, string templateName = null)
        {
            _galaxy = RelateToGalaxy(galaxyName, createIfNotExist, hostname);
        }
        #endregion

        #region Методы

        #region Работа с Галактикой
        // Получение Galaxy. Если существует - то получаем, если не существует - создаем новую
        private IGalaxy RelateToGalaxy(string galaxyName, bool createIfnotExist = false, string hostName = null, string templateName = null)
        {
            try
            {
                return GalaxyRepository.GetGalaxy(galaxyName, hostName);
            }
            catch (GalaxyObjectNotFoundException)
            {
                if (createIfnotExist)
                    if (!string.IsNullOrEmpty(templateName))
                        return GalaxyRepository.CreateGalaxyAndGetFromTemplate(templateName, galaxyName, hostName);
                    else return GalaxyRepository.CreateAndGetGalaxy(galaxyName, hostName);
                else throw;
            }
        }

        // Подключение к Galaxy
        public void Login(string userName = null, string password = null, bool? bForceSynchronization = null)
        {
            if (bForceSynchronization == null)
                _galaxy.Login(userName, password);
            else
                _galaxy.LoginEx(userName, password, bForceSynchronization.Value);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
        }
        // Отключенеи от Галактики
        public void Logout()
        {
            if (_galaxy != null)
            {
                try
                {
                    _galaxy.Logout();
                }
                finally
                {
                    _galaxy = null;
                    GC.Collect();
                }
            }
        }
      
        #endregion

        #region Методы для шаблонов
        // Получает шаблон, если такой существует
        public ITemplate GetTemplateIfExists(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName)) throw new ArgumentNullException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsTemplate, EConditionType.NameEquals, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            if (objects == null || objects.count == 0) 
                return null;
            return (ITemplate)(objects[tagName]);
        }
        // Возврщает шаблон по имени
        public ITemplate GetTemplate(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) throw new GalaxyExceptions($"Параметр {tagName} не может быть пустым или NULL.");
            var obj = GetTemplateIfExists(tagName);
            if (obj == null) throw new GalaxyObjectNotFoundException(tagName);
            return obj;
        }
        //Возвращает шаблоны производные от заданного шаблона (выдает только первое поколение) 
        public IgObjects GetTemplatesDerivedFrom(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName)) throw new ArgumentNullException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsTemplate, 
                                               EConditionType.derivedOrInstantiatedFrom, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        // Возвращает IgObjects коллекцию по списку имен
        public IgObjects GetTemplates(string[] tagNames)
        {
            var objects = _galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, ref tagNames);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        // Получаем все шаблоны Галактики
        public IgObjects GetAllTemplates()
        {
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsTemplate, EConditionType.namedLike, "$", EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        //Создание шаблона из шаблона
        public ITemplate CreateTemplate(string tagName, ITemplate parent)
        {
            if (string.IsNullOrWhiteSpace(tagName)) throw new ArgumentException(nameof(tagName));
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            ITemplate result = parent.CreateTemplate(tagName, true);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return result;
        }
        //Создание шаблона из шаблона по имени(при условии что шаблон от котрого наследуем уже существует)
        public ITemplate CreateTemplate(string tagName, string parentName)
        {
            return CreateTemplate(tagName, GetTemplate(parentName));
        }

        #endregion

        #region Работа с экземплярами
        // Получить экземпляр если он существует
        public IInstance GetInstanceIfExists(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName)) throw new ArgumentException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.NameEquals, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            if (objects == null || objects.count == 0) return null;
            return (IInstance)(objects[tagName]);
        }
        // Возвращает экземпляр по имени
        public IInstance GetInstance(string tagName)
        {
            var obj = GetInstanceIfExists(tagName);
            return obj == null ? throw new GalaxyObjectNotFoundException(tagName) : obj;
        }
        // Возвращает экземпляры производыне от заданного шаблона
        public IgObjects GetInstancesDerividedFrom(string tagName)
        {
            IgObjects gobjects = null;
            try
            {
                if (string.IsNullOrEmpty(tagName)) throw new ArgumentNullException(nameof(tagName));
                gobjects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.derivedOrInstantiatedFrom, tagName, EMatch.MatchCondition);
                GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
                return gobjects;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                gobjects = null;
                GC.Collect();
            }
        }
        // возвращает экземпляры в указанном контейнере( tagName - имя контейнера)
        public IgObjects GetInstancesContainedIn(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) throw new ArgumentNullException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.containedBy, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }

        // возвращает экземпляры в указанном Area( tagName - имя контейнера)
        public IgObjects GetInstancesInArea(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) throw new ArgumentNullException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.belongsToArea, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        // Возвращает все экземпляры на данной платформе
        public IgObjects GetInstancesAssignedTo(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) throw new ArgumentNullException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.assignedTo, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        // Возвращает набор экземпляров из указанного списка
        public IgObjects GetInstances(string[] tagNames)
        {
            if (tagNames.Length <= 0) throw new Exception("$Список тегов не может быть пустым.");
            foreach (var item in tagNames)
                if (string.IsNullOrWhiteSpace(item)) throw new ArgumentException(nameof(tagNames));
            var objects = _galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, ref tagNames);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        // возвращает набор экземпляров (имена еоторых примерно совпадаю) из указананного списка
        public IgObjects GetInstancesLike(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) throw new ArgumentException(nameof(tagName));
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.namedLike, tagName, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        //Получаем все экземпляры Галактики
        public IgObjects GetAllInstances()
        {
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.namedLike, " ", EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        // Возвращает все редактируемые экземпляры
        public IgObjects GetAllCheckedOutInstances()
        {
            var objects = _galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.checkoutStatusIs, ECheckoutStatus.checkedOutToMe, EMatch.MatchCondition);
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return objects;
        }
        //Создаем и возвращаем экземпляр из указанного шаблона - добавлять area или нет ???? 
        public IInstance CreateInstance(string tagName, ITemplate template, string containerName = null)
        {
            if (string.IsNullOrWhiteSpace(tagName)) throw new ArgumentException(nameof(tagName));
            if (template == null) throw new ArgumentNullException(nameof(template));
            IInstance instance = template.CreateInstance(tagName, true);
            if (instance == null)
            {
                string msg = string.Format($"Объект с именем {tagName} не создан! " +
                                        _galaxy.CommandResult != null && !_galaxy.CommandResult.Successful ? _galaxy.CommandResult.Text + " "
                                        + _galaxy.CommandResult.CustomMessage :
                                         "Возможно такой объект уже существует.");
                throw new GalaxyCannotCreateInstanceException(tagName, msg, template, template.CommandResult);
            }
            if (!string.IsNullOrWhiteSpace(containerName))
            {
                try
                {
                    ((IgObject)instance).CheckOutWithCheckStatus();
                    instance.Container = containerName;
                    var text = instance.CommandResult.Text + " - " + instance.CommandResult.CustomMessage;
                    GalaxyExceptions.ThrowIfNoSuccess(instance.CommandResult, $"Ошибка при назначении объекту {tagName} контейнера {containerName}\n");
                    
                }
                catch { throw; }
                finally 
                { 
                    ((IgObject)instance).SaveAndCheckIn($"Установка для объекта {tagName} Container={containerName}"); 
                }
            }
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);

            return instance;
        }
        //Создает и возвращает экземпляр
        public IInstance CreateInstance(string tagName, string templateName, string containerName = null)
        {
            if (string.IsNullOrWhiteSpace(tagName)) throw new ArgumentException(nameof(tagName));
            if (string.IsNullOrWhiteSpace(templateName)) throw new ArgumentException(nameof(templateName));
            return CreateInstance(tagName, GetTemplate(templateName), containerName);
        }
        // 

        #endregion

        #region Общиее
        //Возвращает пустую коллекцию
        public IgObjects CreateEmptyGalaxyObjectCollection()
        {
            var collection = _galaxy.CreategObjectCollection();
            GalaxyExceptions.ThrowIfNoSuccess(CommandResult);
            return collection;
        }
        //Удаленеи объектов IgObjects коллекции
        public bool DeleteObjectsCollection(params IgObject[] objtodel)
        {
            if (objtodel == null)
            {
                return false;
                throw new ArgumentNullException("Не переданы объекты для удаления.");
            }
            var gobjects = CreateEmptyGalaxyObjectCollection();
            foreach (var gobject in objtodel)
                gobjects.Add(gobject);
            gobjects.CheckOut();
            gobjects.DeleteAllObjects();
            gobjects.CheckIn(); // TODO: надо ли чекинить?? Проверить. Причина: объектов нет, что чекинить??
            return true;
        }
        //Удаленеи объектов по имени
        public void DeleteObjectsCollection(params string[] objtodel)
        {
            foreach (var item in objtodel)
            {
                var inst = GetInstanceIfExists(item);
                if (inst != null) inst.DeleteInstance(EForceDeleteInstanceOption.dontForceInstanceDelete);
            }
        }
        //возвращает причину ошибки последней операции 
        public string GetFailReason() //TODO: как быть с многократным доступом
        {
            if (CommandResult.Successful)
                return null;
            else
                return $"{CommandResult.Text}: {CommandResult.CustomMessage}";
        }
        // Чекиним объект
        public void CheckIn(IgObject igobj, DateTime dt = default(DateTime))
        {
            if (igobj == null) throw new ArgumentNullException($"Объект не может быть NULL");
            if (dt == null)
                igobj.CheckIn(DateTime.Now.ToString("yyyyMMdd HH:mm:ss")); //TODO: проверить как передается время
            else
                igobj.CheckIn(dt.ToString("yyyyMMdd HH:mm:ss"));
        }
        #endregion

        #region Работа с атрибутами
            //TODO: реализовать работу с атрибутами
        #endregion


        #endregion
    }
}