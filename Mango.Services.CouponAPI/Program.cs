using Mango.Services.CouponAPI;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Repository;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


#region Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(@"Server=PSL-1K727L3\SQLEXPRESS;Database=MangoCouponAPI;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True"));

#endregion

#region Auto Mapper Configuration

builder.Services.AddAutoMapper(typeof(MappingConfig));

#endregion

#region Repository Services

builder.Services.AddScoped<ICouponRepository, CouponRepository>();

#endregion

#region Authentication

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    //Url of Identity Server
    options.Authority = "https://localhost:7188";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };

});

#endregion

#region Authorization

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mango");
    });
});

#endregion

builder.Services.AddSwaggerGen(
    x =>
    {
        x.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Mango.Services.CouponAPI" });
        x.EnableAnnotations();
        x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"Enter 'Bearer' [space] and your token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });


        x.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
        });
    }
 );

//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
