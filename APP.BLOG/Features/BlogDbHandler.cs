using APP.BLOG.Models;
using System.Globalization;
using System.Reflection.Metadata;
using CORE.APP.Features;


namespace APP.BLOG.Features
{
    public abstract class BlogDbHandler : Handler
    {
        protected readonly ProjectsDb _Projectsdb;

        protected BlogDbHandler(ProjectsDb Projectsdb) : base(new CultureInfo("en-US")) // tr-TR: Turkish
        {
            _Projectsdb = Projectsdb;
        }
    }
}

