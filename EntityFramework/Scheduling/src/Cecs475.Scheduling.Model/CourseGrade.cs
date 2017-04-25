using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs475.Scheduling.Model {
    public class CourseGrade {
        public enum GradeTypes : byte {
            F,
            D,
            C,
            B,
            A
        }

        public int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual CourseSection CourseSection { get; set; }
        public GradeTypes Grade { get; set; }
    }
}
