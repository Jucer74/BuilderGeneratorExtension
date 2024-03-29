﻿using BuilderGeneratorFactory.Options;
using Microsoft.Extensions.Configuration;
using System;

namespace BuilderGeneratorConsole
{
   public class Startup
   {
      public BuilderOptions BuilderOptions { get; private set; }

      public Startup()
      {
         var builder = new ConfigurationBuilder()
                      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                      .AddJsonFile("appsettings.json", optional: false);

         IConfiguration config = builder.Build();

         BuilderOptions = config.GetSection("BuilderOptions").Get<BuilderOptions>();
      }
   }
}