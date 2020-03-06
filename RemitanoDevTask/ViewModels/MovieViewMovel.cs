using System.ComponentModel.DataAnnotations;

namespace RemitanoDevTask.ViewModels
{
    public class MovieViewModel
    {
        [Required]
        [Display(Name = "Video URL")]
        [RegularExpression(@"^(http:\/\/|https:\/\/)(vimeo\.com|youtu\.be|www\.youtube\.com)\/([\w\/]+)([\?].*)?$",
            ErrorMessage = "The youtube URL is not valid")]
        public string VideoUrl { get; set; }

    }
}
