using Microsoft.AspNetCore.Hosting.Server;
using Quartz;
using System.Diagnostics.Metrics;
using NLog;
using NLog.Web;
using SimpleQuartz.Models;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.UseNLog();

try
{
    
    //config
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .Build();
    var quartzConfig = config.Get<QuartzCfg>();
    
    // Add services to the container.
    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        q.InterruptJobsOnShutdown = true;
        q.InterruptJobsOnShutdownWithWait = true;

        //调度线程一次会拉取NEXT_FIRE_TIME小于
        //（now + idleWaitTime +batchTimeWindow）,
        //大于（now - misfireThreshold）的，
        //min(availThreadCount,maxBatchSize)个triggers，
        //默认情况下，会拉取未来30s，
        //过去60s之间还未fire的1个trigger。
        //随后将这些triggers的状态由WAITING
        //改为ACQUIRED，并插入fired_triggers表。
        q.MaxBatchSize = quartzConfig.MaxBatchSize;
    });
    builder.Services.AddQuartzHostedService(opt =>
    {
        opt.WaitForJobsToComplete = true;
    });



    builder.Services.AddControllersWithViews();

    var app = builder.Build();


    var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
    var scheduler = await schedulerFactory.GetScheduler();



    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
