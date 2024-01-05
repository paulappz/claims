using Custodian.Claims.ClaimsProcessing;
using Custodian.Claims.Extensions;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();
builder.AddServices();
var app = builder.Build();
app.ConfigurePipeline();
