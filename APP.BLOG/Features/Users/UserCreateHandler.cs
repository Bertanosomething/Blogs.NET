using APP.BLOG.Models;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.BLOG.Features.Users
{
    // Kullanıcı oluşturma isteği
    public class UserCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Surname { get; set; }
    }

    // Kullanıcı oluşturma işlemini yöneten handler
    public class UserCreateHandler : BlogDbHandler, IRequestHandler<UserCreateRequest, CommandResponse>
    {
        public UserCreateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            // Aynı kullanıcı adında kayıt var mı kontrolü
            if (await _Projectsdb.Users.AnyAsync(u => u.UserName.ToUpper() == request.UserName.Trim().ToUpper(), cancellationToken))
                return Error("Aynı kullanıcı adına sahip bir kayıt zaten mevcut!");

            var entity = new User()
            {
                UserName = request.UserName.Trim(),
                Password = request.Password, // Parola güncellemede hash kullanmanız önerilir.
                Name = request.Name.Trim(),
                Surname = request.Surname.Trim(),
                IsActive = true,
                RegistrationDate = DateTime.UtcNow,
                RoleId = 1 // Varsayılan rol; önceden Role tablosunda bu ID'nin var olduğundan emin olun.
            };

            _Projectsdb.Users.Add(entity);
            await _Projectsdb.SaveChangesAsync(cancellationToken);

            return Success("Kullanıcı başarıyla oluşturuldu.", entity.Id);
        }
    }
}