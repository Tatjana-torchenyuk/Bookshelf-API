using BooksMVC;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .RunAsync();