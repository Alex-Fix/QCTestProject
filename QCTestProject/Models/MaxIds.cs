using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Models
{
    public class MaxIds
    {
        public int Id { get; set; }
        public int MaxBookId { get; set; }
        public int MaxAuthorId { get; set; }
        public int MaxCategoryId { get; set; }
        public int MaxLanguageId { get; set; }
        public int MaxPublisherId { get; set; }
    }
}
