namespace Talabat.APIS.Extensions
{
    public static class AddSwaggerExtension
    {
        public static WebApplication UseSwaggerMiddelwares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
