using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BookStore.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BookStore.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }

        public string IdNumber { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            userIdentity.AddClaim(new Claim("Id", Id));
            userIdentity.AddClaim(new Claim("Name", Name));
            userIdentity.AddClaim(new Claim("Address", Address));
            userIdentity.AddClaim(new Claim("City", City));
            userIdentity.AddClaim(new Claim("State", State));
            userIdentity.AddClaim(new Claim("PostalCode", PostalCode));
            userIdentity.AddClaim(new Claim("Country", Country));
            userIdentity.AddClaim(new Claim("Phone", PhoneNo));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<ProductCatergory> ProductCategories { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Library> Libraries { get; set; }
      
        public DbSet<LibraryCatergory> LibraryCatergories { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<BankingDetails> BankingDetails { get; set; }
        public DbSet<BankInfo> BankInfos { get; set; }
        public DbSet<OnlineSalesOrCODInfo> OnlineSalesInfos { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get;set; }

        public DbSet<RNRData> RNRDatas { get; set; }
        public DbSet<ReasonDrop> ReasonDrops { get; set; }
        public DbSet<ReturnAndRefund> ReturnAndRefunds { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<BookStore.Models.RoleViewModel> RoleViewModels { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.RequestBook> RequestBooks { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.ChatBox> ChatBoxes { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.BorrowedBooks> BorrowedBooks { get; set; }
        public System.Data.Entity.DbSet<BookStore.Models.BookQrCodes> BookQrCodes { get; set; }

        //public System.Data.Entity.DbSet<BookStore.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public virtual DbSet<BorrowedBooks> BorrowedBooks { get; set; }



        // public System.Data.Entity.DbSet<BookStore.Models.ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<BookStore.Models.WishLists> WishLists { get; set; }

       // public System.Data.Entity.DbSet<BookStore.Models.ApplicationUser> ApplicationUsers { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.WishListsProducts> WishListsProducts { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.Coupon> Coupons { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.Analytic> Analytics { get; set; }

        public System.Data.Entity.DbSet<BookStore.Models.eBook> eBooks { get; set; }


        public System.Data.Entity.DbSet<BookStore.Models.UsedCouponList> UsedCouponLists { get; set; }
        public System.Data.Entity.DbSet<BookStore.Models.Feedback> Feedbacks { get; set; }
    }
}