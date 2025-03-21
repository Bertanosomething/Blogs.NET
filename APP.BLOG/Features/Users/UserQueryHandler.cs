using APP.BLOG.Features;
using APP.BLOG.Models;
using APP.Projects.Features.Blogs;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.BLOG.Features.Users
{
    // Kullanıcı sorgulama isteği (filtre eklenebilir)
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    // Kullanıcı sorgulama sonucu DTO'su
    public class UserQueryResponse : QueryResponse
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BlogCount { get; set; }
        public string BlogTitles { get; set; }
        public List<string> BlogTitleList { get; set; }
        public List<BlogQueryResponse> Blogs { get; set; }
    }

    // Kullanıcı sorgulama işlemini yöneten handler
    public class UserQueryHandler : BlogDbHandler, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(ProjectsDb db) : base(db)
        {
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _Projectsdb.Users
                .Include(u => u.Blogs)
                .OrderBy(u => u.UserName)
                .Select(u => new UserQueryResponse
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Name = u.Name,
                    Surname = u.Surname,
                    BlogCount = u.Blogs.Count,
                    BlogTitles = string.Join(", ", u.Blogs.Select(b => b.Title)),
                    BlogTitleList = u.Blogs.Select(b => b.Title).ToList(),
                    Blogs = u.Blogs.Select(b => new BlogQueryResponse
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Content = b.Content,
                        Rating = b.Rating,
                        PublishDate = b.PublishDate,
                        UserId = b.UserId
                    }).ToList()
                });
            return Task.FromResult(query);
        }
    }
}