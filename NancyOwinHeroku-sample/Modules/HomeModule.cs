using Nancy;

namespace NancyOwinHeroku_sample.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello World";

            Get["/json"] = _ =>
            {
                var ringo = new[]
                {
                    new { Id = 1, ItemName = "トキ"},
                    new { Id = 2, ItemName = "秋映"}
                };

                return Response.AsJson(ringo);
            };
        }
    }
}
