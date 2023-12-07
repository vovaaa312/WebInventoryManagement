using Oracle.ManagedDataAccess.Client;
using WebInventoryManagement.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddTransient<StoreService>();
        builder.Services.AddTransient<ShelfService>();
        builder.Services.AddTransient<ItemService>();
        builder.Services.AddTransient<CategoryService>();


        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
            // UseStatusCodePagesWithReExecute middleware for non-development environments
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        //app.MapControllerRoute(
        //    name: "default",
        //    pattern: "{controller=Store}/{action=Index}/{id?}");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run();
    }
}