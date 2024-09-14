using SocialGameAPI.Controllers;
using SocialGameAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the connection string to the service collection
builder.Services.AddSingleton
(
    new SocialGameAPIService
    (
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configuration for using attribute routing
app.MapControllers();

/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Welcome");
});

app.MapGet("/master_data", async context =>
{
    //var controller = new MasterDataController();
    //await controller.Get(context);
});

app.MapPost("/registration", async context =>
{
    var controller = new RegistrationController(app.Services.GetRequiredService<IConfiguration>());
    var result = controller.Registration();
    await context.Response.WriteAsync(result.ToString());
});

app.MapPost("/login", async context =>
{
    var controller = new LoginController(app.Services.GetRequiredService<IConfiguration>());
    var request = await context.Request.ReadFromJsonAsync<LoginRequest>();
    var result = controller.Login(request);
    await context.Response.WriteAsync(result.ToString());
});

app.MapGet("/quest_tutorial", async context =>
{
    //var controller = new QuestController();
    //await controller.Tutorial(context);
});

app.MapGet("/quest_start", async context =>
{
    //var controller = new QuestController();
    //await controller.Start(context);
});

app.MapGet("/quest_end", async context =>
{
    //var controller = new QuestController();
    //await controller.End(context);
});

app.MapGet("/character", async context =>
{
    //var controller = new CharacterController();
    //await controller.GetCharacterList(context);
});

app.MapGet("/character_sell", async context =>
{
    //var controller = new CharacterController();
    //await controller.SellCharacter(context);
});

app.MapGet("/gacha", async context =>
{
    //var controller = new GachaController();
    //await controller.DrawGacha(context);
});

app.MapGet("/shop", async context =>
{
    //var controller = new ShopController();
    //await controller.BuyItem(context);
});

app.MapGet("/present_list", async context =>
{
    //var controller = new PresentController();
    //await controller.GetPresentList(context);
});

app.MapGet("/present", async context =>
{
    //var controller = new PresentController();
    //await controller.GetItem(context);
});
*/

app.Run();
