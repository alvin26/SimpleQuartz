using Microsoft.AspNetCore.Hosting.Server;
using Quartz;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddQuartz(q=>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.InterruptJobsOnShutdown = true;
    q.InterruptJobsOnShutdownWithWait= true;

    //调度线程一次会拉取NEXT_FIRE_TIME小于
    //（now + idleWaitTime +batchTimeWindow）,
    //大于（now - misfireThreshold）的，
    //min(availThreadCount,maxBatchSize)个triggers，
    //默认情况下，会拉取未来30s，
    //过去60s之间还未fire的1个trigger。
    //随后将这些triggers的状态由WAITING
    //改为ACQUIRED，并插入fired_triggers表。
    q.MaxBatchSize = 150;
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
