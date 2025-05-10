using APP.BLOG.Features;
using APP.BLOG.Models;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Blogs
{
    /// <summary>
    /// Request to query Blog records.
    /// </summary>
    public class BlogQueryRequest : Request, IRequest<IQueryable<BlogQueryResponse>>
    {
        // Optionally add filters here, e.g., public string TitleContains { get; set; }
    }

    /// <summary>
    /// Represents the data returned when querying blogs.
    /// </summary>
    public class BlogQueryResponse : QueryResponse
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Rating { get; set; }
        public DateTime PublishDate { get; set; }
        //public int UserId { get; set; }

        /// <summary>
        /// List of Tag IDs linked to this blog.
        /// </summary>
        public List<int> TagIds { get; set; }
    }

    /// <summary>
    /// Handles the request to query Blogs.
    /// </summary>
    public class BlogQueryHandler : BlogDbHandler, IRequestHandler<BlogQueryRequest, IQueryable<BlogQueryResponse>>
    {
        public BlogQueryHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public Task<IQueryable<BlogQueryResponse>> Handle(BlogQueryRequest request, CancellationToken cancellationToken)
        {
            // Query Blogs, include BlogTags -> Tag
            var query = _Projectsdb.Blogs
                .Include(b => b.BlogTags)
                .ThenInclude(bt => bt.Tag)
                .Select(b => new BlogQueryResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    Rating = b.Rating,
                    PublishDate = b.PublishDate,
                    //UserId = b.UserId,
                    TagIds = b.BlogTags.Select(bt => bt.TagId).ToList()
                })
                .OrderBy(b => b.Title);

            return Task.FromResult<IQueryable<BlogQueryResponse>>(query);

        }
    }
}