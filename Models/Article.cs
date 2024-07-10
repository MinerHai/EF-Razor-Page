using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// dotnet aspnet-codegenerator razorpage -m DBWeb.Article -dc DBWeb.MyblogContext -outDir Pages/Blog -udl --referenceScriptLibraries
namespace DBWeb{
    public class Article{
        [Key]
        public int Id { get; set; }
        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0} yêu cầu chứa {2} đến {1} kí tự")]
        [Required(ErrorMessage ="{0} không được để trống")]
        [Column(TypeName="nvarchar")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="{0} không được để trống")]
        [DisplayName("Ngày tạo")]

        public DateTime Created { get; set; }
        [Column(TypeName = "ntext")]
        [DisplayName("Nội dung")]
        public string Content { get; set; }
    }
}