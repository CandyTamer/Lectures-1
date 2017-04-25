using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs475.Scheduling.Model {
    public enum RegistrationResults : byte {
        Sucess,
        PrerequisiteNotMet,
        TimeConflict,
        AlreadyEnrolled,
        AlreadyCompleted
    }

	public class Student {
		public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<CourseGrade> Transcript { get; set; }
        public virtual ICollection<CourseSection> EnrolledCourses { get; set; }
		//public virtual ICollection<CourseSection> CompletedCourses { get; set; } = new List<CourseSection>();

		public RegistrationResults CanRegisterForCourseSection(CourseSection section) {
            //checks for if they're enrolled in another section of that catalog course in that current semester
            var same = from c in EnrolledCourses
                       where ((c.CatalogCourse.Id.Equals(section.Id)) && (c.Semester.Equals(section.Semester)))
                       select c;

            //they're already enrolled in that course
            if (same.Count() != 0) {
                return RegistrationResults.AlreadyEnrolled;
            }

            //checks for if they've taken and passed that class in the past
            var passed = from g in Transcript
                         where (g.CourseSection.CatalogCourse.Id.Equals(section.CatalogCourse.Id) && //if its the same course
                                (byte)g.Grade >= (byte)CourseGrade.GradeTypes.C) //and they passed
                         select g;
            
            if (passed.Count() != 0) { //they've already taken and passed that course
                return RegistrationResults.AlreadyCompleted;
            }

            //checks each prerequisite to make sure they've passed them, returns not met if they didn't
            foreach (CatalogCourse course in section.CatalogCourse.Prerequisites) {
                var met = from g in Transcript
                          where ((g.CourseSection.CatalogCourse.Id.Equals(course.Id)) && //if its the same course
                                (!g.CourseSection.Semester.Equals(section.Semester)) && //and it isn't from the current semester
                                ((byte)g.Grade >= (byte)CourseGrade.GradeTypes.C)) //and they passed
                          select g;
                if (met.Count() == 0) {
                    return RegistrationResults.PrerequisiteNotMet;
                }
            }

            //checks if wanted section conflicts with any of their already enrolled classes
            //the two time checks are or'd, which is anded with the meeting days, which that result is then 
            //anded with it being the correct semester
            var conflict = from c in EnrolledCourses
                           where ((c.Semester.Equals(section.Semester)) && //if its the current semester
                                    ((((byte)c.MeetingDays & (byte)section.MeetingDays) != 0) && //but the meeting days conflict
                                    //and its start time is contained within any of the times between start and 
                                    //end time of other classes
                                    ((section.StartTime > c.StartTime && section.StartTime < c.EndTime) || 
                                    //or its end time is contained within any of the times between start and
                                    //end time of other classes
                                    (section.EndTime > c.StartTime && section.EndTime < c.EndTime))))
                           select c;

            if (conflict.Count() != 0) {
                return RegistrationResults.TimeConflict;
            }

            return RegistrationResults.Sucess;
		}
	}
}
