using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using DAL;
using Feed;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using System.Xml;
namespace hakaton
{
    public class Program
    {

    
        public static void Main(string[] args)
        {

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                                  });
            });
            var app = builder.Build();
            app.UseCors(MyAllowSpecificOrigins);
            Context db = new Context();
            app.UseDefaultFiles();
            app.UseStaticFiles();
           
            app.MapGet("/feedback", () =>
            { 

                var feedBacks = db.feedBacks;
                var json = JsonConvert.SerializeObject(feedBacks);
                return json; 
                    
                
            });
            app.MapPost("/feedback",  async (context) =>
            { 
                var Request = context.Request;
                using (var reader = new StreamReader(Request.Body))
                {
                    string json = await reader.ReadToEndAsync();
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    FeedBack feedBack = new FeedBack { Email = values["email"], Message = values["message"], PhoneNumber= values["phone"], Name= values["name"] };
                    //db.feedBacks.Add(feedBack);
                    var result = new Dictionary<string,string> { { "status", "200" } };
                    var json2 = JsonConvert.SerializeObject(result);
                    await context.Response.WriteAsync(json2);
                    db.SaveChanges();
                    
                    
                }


            });

            app.Run();
        }
    }
}
