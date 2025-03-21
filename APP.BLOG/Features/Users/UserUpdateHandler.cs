using APP.BLOG.Models;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.BLOG.Features.Users
{
    // Kullanıcı güncelleme isteği
    public class UserUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Surname { get; set; }
    }

    // Kullanıcı güncelleme işlemini yöneten handler
    public class UserUpdateHandler : BlogDbHandler, IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        public UserUpdateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            // Aynı kullanıcı adında farklı bir kayıt var mı kontrolü (güncellenmekte olan kaydı hariç tutarak)
            if (await _Projectsdb.Users.AnyAsync(u => u.Id != request.Id && u.UserName.ToUpper() == request.UserName.Trim().ToUpper(), cancellationToken))
                return Error("Aynı kullanıcı adına sahip başka bir kayıt mevcut!");

            var entity = await _Projectsdb.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
                return Error("Kullanıcı bulunamadı!");

            entity.UserName = request.UserName.Trim();
            entity.Password = request.Password; // Parola güncellenirken hashleme yapılabilir
            entity.Name = request.Name.Trim();
            entity.Surname = request.Surname.Trim();

            _Projectsdb.Users.Update(entity);
            await _Projectsdb.SaveChangesAsync(cancellationToken);

            return Success("Kullanıcı başarıyla güncellendi.", entity.Id);
        }
    }
}