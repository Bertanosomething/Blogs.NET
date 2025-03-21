using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using APP.BLOG.Models;
using APP.BLOG.Features;

namespace APP.Projects.Features.Blogs
{
    /// <summary>
    /// Request to create a new Blog record.
    /// </summary>
    public class BlogCreateRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// Title of the blog (required, must be between 5 and 200 characters).
        /// </summary>
        [Required, StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        /// <summary>
        /// Main content of the blog (required).
        /// </summary>
        [Required, StringLength(4000)]
        public string Content { get; set; }

        /// <summary>
        /// Optional rating for the blog, decimal type.
        /// </summary>
        public decimal? Rating { get; set; }

        /// <summary>
        /// Date/time the blog is published.
        /// </summary>
        [Required]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// The user ID who authored this blog (foreign key to User).
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// List of tag IDs associated with this blog.
        /// </summary>
        public List<int> TagIds { get; set; }
    }

    /// <summary>
    /// Handles the creation of a new Blog.
    /// </summary>
    public class BlogCreateHandler : BlogDbHandler, IRequestHandler<BlogCreateRequest, CommandResponse>
    {
        public BlogCreateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(BlogCreateRequest request, CancellationToken cancellationToken)
        {
            // (Optional) Check if a blog with the same title already exists
            var exists = await _Projectsdb.Blogs.AnyAsync(
                b => b.Title.ToUpper() == request.Title.Trim().ToUpper(),
                cancellationToken
            );
            if (exists)
                return Error("A blog with the same title already exists!");

            // Create the new Blog entity
            var blogEntity = new Blog
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                Rating = request.Rating,
                PublishDate = request.PublishDate,
                UserId = request.UserId
            };

            // Add the blog to the context
            _Projectsdb.Blogs.Add(blogEntity);
            await _Projectsdb.SaveChangesAsync(cancellationToken);

            // If there are tags, create BlogTag records
            if (request.TagIds != null && request.TagIds.Any())
            {
                var blogTags = request.TagIds.Select(tagId => new BlogTag
                {
                    BlogId = blogEntity.Id,
                    TagId = tagId
                });
                _Projectsdb.BlogTags.AddRange(blogTags);
                await _Projectsdb.SaveChangesAsync(cancellationToken);
            }

            return Success("Blog created successfully.", blogEntity.Id);
        }
    }
}