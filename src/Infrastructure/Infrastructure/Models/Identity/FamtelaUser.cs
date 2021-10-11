//using Famtela.Domain.Contracts;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using Famtela.Application.Interfaces.Chat;
//using Famtela.Application.Models.Chat;
//using Famtela.Domain.Entities.Catalog;
//using Famtela.Domain.Entities.Chicken;
//using Famtela.Domain.Entities.Dairy;

//namespace Famtela.Infrastructure.Models.Identity
//{
//    public class FamtelaUser : IdentityUser<string>, IChatUser, IAuditableEntity<string>
//    {
//        public string FirstName { get; set; }

//        public string LastName { get; set; }

//        public string CreatedBy { get; set; }

//        [Column(TypeName = "text")]
//        public string ProfilePictureDataUrl { get; set; }

//        public DateTime CreatedOn { get; set; }

//        public string LastModifiedBy { get; set; }

//        public DateTime? LastModifiedOn { get; set; }

//        public bool IsDeleted { get; set; }

//        public DateTime? DeletedOn { get; set; }
//        public bool IsActive { get; set; }
//        public string RefreshToken { get; set; }
//        public DateTime RefreshTokenExpiryTime { get; set; }
//        public virtual ICollection<ChatHistory<FamtelaUser>> ChatHistoryFromUsers { get; set; }
//        public virtual ICollection<ChatHistory<FamtelaUser>> ChatHistoryToUsers { get; set; }
//        public virtual ICollection<FarmProfile> FarmProfiles { get; set; }
//        public virtual ICollection<Chick> Chicks { get; set; }
//        public virtual ICollection<ChickenExpense> ChickenExpenses { get; set; }
//        public virtual ICollection<Egg> Eggs { get; set; }
//        public virtual ICollection<Grower> Growers { get; set; }
//        public virtual ICollection<Layer> Layers { get; set; }
//        public virtual ICollection<Cow> Cows { get; set; }
//        public virtual ICollection<DairyExpense> DairyExpenses { get; set; }
//        public virtual ICollection<Milk> Milks { get; set; }

//        public FamtelaUser()
//        {
//            ChatHistoryFromUsers = new HashSet<ChatHistory<FamtelaUser>>();
//            ChatHistoryToUsers = new HashSet<ChatHistory<FamtelaUser>>();
//        }
//    }
//}