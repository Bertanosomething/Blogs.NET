using APP.BLOG.Features;
using APP.BLOG.Models;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Blogs
{
    /// <summary>
    /// Request to delete an existing Blog record.
    /// </summary>
    public class BlogDeleteRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// ID of the blog to be deleted.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the deletion of a Blog.
    /// </summary>
    public class BlogDeleteHandler : BlogDbHandler, IRequestHandler<BlogDeleteRequest, CommandResponse>
    {
        public BlogDeleteHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(BlogDeleteRequest request, CancellationToken cancellationToken)
        {
            // Retrieve the blog with its associated BlogTags
            var blogEntity = await _Projectsdb.Blogs
                .Include(b => b.BlogTags)
                .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (blogEntity == null)
                return Error("Blog not found!");

            // Remove associated BlogTag records
            _Projectsdb.BlogTags.RemoveRange(blogEntity.BlogTags);

            // Remove the blog
            _Projectsdb.Blogs.Remove(blogEntity);
            await _Projectsdb.SaveChangesAsync(cancellationToken);

            return Success("Blog deleted successfully.", blogEntity.Id);
        }
    }
}