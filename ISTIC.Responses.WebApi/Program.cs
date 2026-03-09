using ISTIC.Responses.Converters;
using ISTIC.Responses.Extensions;
using ISTIC.Responses.Filters;
using ISTIC.Responses.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomActionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ResponseOfJsonConverterFactory());
    options.JsonSerializerOptions.Converters.Add(new ResponseJsonConverterFactory());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(d => d.GetSchemaId());
    options.OperationFilter<ResponseOperationFilter>();
});

builder.Services.AddCors(setup => setup
                    .AddDefaultPolicy(policy =>
                        policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.MapControllers();

app.Run();
