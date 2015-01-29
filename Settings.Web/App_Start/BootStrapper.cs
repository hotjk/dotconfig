using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Grit.Tree;
using Grit.Tree.Repository.MySql;
using Ninject;
using Settings.Model;
using Settings.Repository.MySql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Settings.Web
{
    public class BootStrapper
    {
        public static Ninject.IKernel NinjectContainer { get; private set; }
        public static void BootStrap()
        {
            NinjectContainer = new StandardKernel();
            AddIoCBindings();
        }

        private static void AddIoCBindings()
        {
            var settingsSqlOption = new Settings.Repository.MySql.SqlOption { ConnectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString };
            var treeSqlOption = new Grit.Tree.Repository.MySql.SqlOption { ConnectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString };
            var sequenceSqlOption = new Grit.Sequence.Repository.MySql.SqlOption { ConnectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString };

            NinjectContainer.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope()
                .WithConstructorArgument<Grit.Sequence.Repository.MySql.SqlOption>(sequenceSqlOption);
            NinjectContainer.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            NinjectContainer.Bind<ITreeRepository>().To<TreeRepository>().InSingletonScope()
                .WithConstructorArgument<Grit.Tree.Repository.MySql.SqlOption>(treeSqlOption);
            NinjectContainer.Bind<ITreeService>().To<TreeService>().InSingletonScope()
                .WithConstructorArgument("table", "settings_tree");

            NinjectContainer.Bind<INodeRepository>().To<NodeRepository>().InSingletonScope()
                .WithConstructorArgument<Settings.Repository.MySql.SqlOption>(settingsSqlOption);
            NinjectContainer.Bind<INodeService>().To<NodeService>().InSingletonScope();

            NinjectContainer.Bind<IClientRepository>().To<ClientRepository>().InSingletonScope()
                .WithConstructorArgument<Settings.Repository.MySql.SqlOption>(settingsSqlOption);
            NinjectContainer.Bind<IClientService>().To<ClientService>().InSingletonScope();

            NinjectContainer.Bind<IUserRepository>().To<UserRepository>().InSingletonScope()
                .WithConstructorArgument<Settings.Repository.MySql.SqlOption>(settingsSqlOption);
            NinjectContainer.Bind<IUserService>().To<UserService>().InSingletonScope();

            NinjectContainer.Bind<ISqlRepository>().To<SqlRepository>().InSingletonScope()
                .WithConstructorArgument<Settings.Repository.MySql.SqlOption>(settingsSqlOption);
            NinjectContainer.Bind<ISqlService>().To<SqlService>().InSingletonScope();
        }
    }
}