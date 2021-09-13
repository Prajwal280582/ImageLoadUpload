using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ImageLoadUpload.Logics;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using static ImageLoadUpload.Models.MongoModel;

namespace ImageLoadUpload
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {     
            //Swagger UI registing
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageLoadUpload", Version = "v1" });
            });

            //Azure Blob Storage registering
            services.AddScoped(_ => {
                return new BlobServiceClient(Configuration.GetConnectionString("AzureBlobStorage"));
            });

            //Registering controllers
            services.AddControllers();

            //Dependancy Injection - Image manager logic
            services.AddScoped<IImageManagerLogic, ImageManagerLogic>();

            //Registing the Azure Cosmos DB - Mongo DB APi
            services.Configure<ImageGalleryDatabaseSettings>(
          Configuration.GetSection(nameof(ImageGalleryDatabaseSettings)));

            //Dependacy Injection  Azure Cosmos DB - Mongo DB APi
            services.AddSingleton<IImageGalleryDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ImageGalleryDatabaseSettings>>().Value);

            //Dependancy Injection - Azure Cosmos DB - Mongodb APi logic
            services.AddSingleton<MongoDbService>();

            //Registing the Azure Cosmos DB - SQL API
            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Executes exception page only when in development
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //configuring Swagger UI page
            app.UseSwagger();
            app.UseSwaggerUI(c =>
              {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageLoadUpload v1");
                    c.RoutePrefix = string.Empty;  // To display the swagger UI in  the index page
              });
            app.UseHttpsRedirection();            
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //Fetching Cosmos DB - Sql API configurations from app settings and Initialized
        private static async Task<CosmosDbLogic> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];
            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbLogic = new CosmosDbLogic(client, databaseName, containerName);
            
            return cosmosDbLogic;
        }
    }
    
}
