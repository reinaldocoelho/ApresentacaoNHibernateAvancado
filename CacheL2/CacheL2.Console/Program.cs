﻿using CacheL2.Entidades;
using CacheL2.Entidades.Mapeamento;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace CacheL2.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                         .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Data Source=localhost;Initial Catalog=Northwind;User ID=sa;Password=Northwing0123")                         
                         .ShowSql().FormatSql())
                         .Cache(c => c.UseSecondLevelCache().ProviderClass(typeof(NHibernate.Caches.StackExchangeRedis.RedisCacheProvider).AssemblyQualifiedName))                         
                         .Mappings(m =>
                             m.FluentMappings
                             .AddFromAssemblyOf<EmployeesMap>())
                         .ExposeConfiguration(cfg=> cfg.SetProperty("cache.configuration", "localhost:6379"))
                         .BuildSessionFactory();

            ISession session1 = sessionFactory.OpenSession();
            Products product1 = session1.Get<Products>(1);

            System.Console.WriteLine(product1.ProductName);

            ISession session2 = sessionFactory.OpenSession();
            Products product2 = session1.Get<Products>(1);

             System.Console.WriteLine(product2.ProductName);

        }
    }
}