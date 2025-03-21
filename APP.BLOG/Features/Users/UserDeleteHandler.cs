using APP.BLOG.Models;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.BLOG.Features.Users
{
    // Kullanıcı silme isteği (ID üzerinden silinecek)
    public class UserDeleteRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

    // Kullanıcı silme işlemini yöneten handler
    public class UserDeleteHandler : BlogDbHandler, IRequestHandler<UserDeleteRequest, CommandResponse>
    {
        public UserDeleteHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
        {
            // Kullanıcının varsa, örneğin kullanıcının yazdığı bloglar kontrol edilebilir
            var entity = await _Projectsdb.Users
                .Include(u => u.Blogs) // Eğer User ile ilişkilendirilmiş Blog varsa
                .SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Kullanıcı bulunamadı!");

            if (entity.Blogs.Any())
                return Error("Kullanıcıya ait bloglar olduğu için silme işlemi gerçekleştirilemez!");

            _Projectsdb.Users.Remove(entity);
            await _Projectsdb.SaveChangesAsync(cancellationToken);

            return Success("Kullanıcı başarıyla silindi.", entity.Id);
        }
    }
}