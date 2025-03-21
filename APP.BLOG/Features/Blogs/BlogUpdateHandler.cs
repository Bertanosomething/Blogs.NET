using APP.BLOG.Features;
using APP.BLOG.Models;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Blogs
{
    /// <summary>
    /// Request to update an existing Blog record.
    /// </summary>
    public class BlogUpdateRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// ID of the blog to be updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

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
        /// Publish date for the blog (required).
        /// </summary>
        [Required]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// The user ID who authored this blog.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// New set of tag IDs for the blog.
        /// </summary>
        public List<int> TagIds { get; set; }
    }

    /// <summary>
    /// Handles the update of an existing Blog.
    /// </summary>
    public class BlogUpdateHandler : BlogDbHandler, IRequestHandler<BlogUpdateRequest, CommandResponse>
    {
        public BlogUpdateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(BlogUpdateRequest request, CancellationToken cancellationToken)
        {
            // (Optional) Check for a conflicting title (excluding current blog)
            bool titleConflict = await _Projectsdb.Blogs.AnyAsync(
                b => b.Id != request.Id && b.Title.ToUpper() == request.Title.Trim().ToUpper(),
                cancellationToken
            );
            if (titleConflict)
                return Error("Another blog with the same title already exists!");

            // Retrieve the blog + associated tags
            var blogEntity = await _Projectsdb.Blogs
                .Include(b => b.BlogTags)
                .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (blogEntity == null)
                return Error("Blog not found!");

            // Remove existing BlogTags
            _Projectsdb.BlogTags.RemoveRange(blogEntity.BlogTags);

            // Update properties
            blogEntity.Title = request.Title.Trim();
            blogEntity.Content = request.Content.Trim();
            blogEntity.Rating = request.Rating;
            blogEntity.PublishDate = request.PublishDate;
            blogEntity.UserId = request.UserId;

            // Re-add the new set of Tag IDs
            if (request.TagIds != null && request.TagIds.Any())
            {
                var newTags = request.TagIds.Select(tagId => new BlogTag
                {
                    BlogId = blogEntity.Id,
                    TagId = tagId
                });
                _Projectsdb.BlogTags.AddRange(newTags);
            }

            _Projectsdb.Blogs.Update(blogEntity);
            await _Projectsdb.SaveChangesAsync(cancellationToken);

            return Success("Blog updated successfully.", blogEntity.Id);
        }
    }
}