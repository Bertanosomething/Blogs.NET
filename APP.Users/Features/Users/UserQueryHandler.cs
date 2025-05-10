using MediatR;
//using System.ComponentModel.DataAnnotations;
//using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using APP.Users.Domain;
using APP.Users.Features.Roles;
using APP.Users.Features.Skills;
using CORE.APP.Features;
using APP.Users.Features;

namespace APP.Users.Features.Users
{
       public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
        {
            //[JsonIgnore]
            //public override int Id { get => base.Id; set => base.Id = value; }

            //[StringLength(30, MinimumLength = 3)]
            //public string UserName { get; set; }

            //[StringLength(10, MinimumLength = 3)]
            //public string Password { get; set; }
        }
        
        // Kullanıcı sorgulama sonucu DTO'su
        public class UserQueryResponse : QueryResponse
        {
            // user identity
            public string UserName { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public bool IsActive { get; set; }
            public string Active { get; set; }

            // skills
            public List<int> SkillIds { get; set; }
            public string SkillNames { get; set; }
            public List<SkillQueryResponse> Skills { get; set; } 

            // role
            public int RoleId { get; set; }
            public string RoleName { get; set; }
        }

        // Kullanıcı sorgulama işlemini yöneten handler
        public class UserQueryHandler : UsersDbHandler, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
        {
            public UserQueryHandler(UsersDb db) : base(db)
            {
            }

            public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
            {
                var entityQuery = _db.Users
                .Include(u => u.Role)
                .Include(u => u.UserSkills)
                    .ThenInclude(us => us.Skill)
                .OrderByDescending(u => u.IsActive)
                .ThenBy(u => u.UserName)
                .AsQueryable();

                //if (!string.IsNullOrWhiteSpace(request.UserName) &&
                //    !string.IsNullOrWhiteSpace(request.Password))
                //{
                //    entityQuery = entityQuery.Where(u =>
                //        u.UserName == request.UserName &&
                //        u.Password == request.Password &&
                //        u.IsActive);
                //}

                var query = entityQuery.Select(u => new UserQueryResponse
                {
                    // from QueryResponse base:
                    Id = u.Id,
                    IsActive = u.IsActive,
                    Active = u.IsActive ? "Active" : "Not Active",

                    // user fields:
                    UserName = u.UserName,
                    Name = u.Name,
                    Surname = u.Surname,

                    // skills
                    SkillIds = u.SkillIds,
                    SkillNames = string.Join(", ", u.UserSkills.Select(us => us.Skill.Name)),
                    Skills = u.UserSkills.Select(us => new SkillQueryResponse
                    {
                        Name = us.Skill.Name
                    }).ToList(),

                    // role
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name
                });

                return Task.FromResult(query);      
            }
        }
}
